using Ecommerce.Application.Common.Interfaces;
using Ecommerce.Infrastructure.Configurations;
using Ecommerce.Infrastructure.Persistence;
using Ecommerce.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Ecommerce.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureDI(this IServiceCollection services)
        {
            services.AddOptions<DatabaseOptions>().BindConfiguration(DatabaseOptions.SectionName);

            services.AddDbContext<ApplicationDbContext>((sp, options) =>
            {
                var dbOptions = sp.GetRequiredService<IOptions<DatabaseOptions>>().Value;
                options.UseNpgsql(dbOptions.DefaultConnection).UseSnakeCaseNamingConvention();
            });

            services.AddScoped<IApplicationDbContext, ApplicationDbContext>(sp =>
                    sp.GetRequiredService<ApplicationDbContext>());

            services.AddSingleton<IPasswordHasher, PasswordHasher>();

            return services;
        }
    }
}
