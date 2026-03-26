using Ecommerce.Api.Middlewares;
using Ecommerce.Application;
using Ecommerce.Infrastructure;
using Ecommerce.Infrastructure.Configurations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Ecommerce.Api
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAppDI(this IServiceCollection services)
        {
            services.AddApplicationDI().AddInfrastructureDI();

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap.Clear();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer();

            services.PostConfigure<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                using var scope = services.BuildServiceProvider().CreateScope();
                var jwtSettings = scope.ServiceProvider.GetRequiredService<IOptions<JwtOptions>>();

                options.TokenValidationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Value.SecretKey));
                options.TokenValidationParameters.ValidIssuer = jwtSettings.Value.Issuer;
                options.TokenValidationParameters.ValidAudience = jwtSettings.Value.Audience;

                options.TokenValidationParameters.NameClaimType = JwtRegisteredClaimNames.PreferredUsername;
                options.TokenValidationParameters.RoleClaimType = ClaimTypes.Role;

                options.Events = new JwtBearerEvents
                {
                    OnChallenge = async context =>
                    {
                        context.HandleResponse();

                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        var response = new
                        {
                            Status = StatusCodes.Status401Unauthorized,
                            Title = "Unauthorized",
                            Detail = "You are not authorized to access this resource. Please provide a valid token."
                        };
                        await context.Response.WriteAsJsonAsync(response);
                    },
                    OnForbidden = async context =>
                    {
                        context.Response.StatusCode = StatusCodes.Status403Forbidden;
                        var response = new
                        {
                            Status = StatusCodes.Status403Forbidden,
                            Title = "Forbidden",
                            Detail = "You do not have the required permissions to perform this action."
                        };
                        await context.Response.WriteAsJsonAsync(response);
                    }
                };
            });

            services.AddExceptionHandler<GlobalExceptionHandler>();
            services.AddProblemDetails();

            return services;
        }
    }
}
