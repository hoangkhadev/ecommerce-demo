using Ecommerce.Application;
using Ecommerce.Infrastructure;
using Ecommerce.Infrastructure.Configurations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
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
                options.TokenValidationParameters.RoleClaimType = "role";
            });

            return services;
        }
    }
}
