using Microsoft.AspNetCore.Identity;
using MPStore.Identidade.API.Data;
using NetDevPack.Identity.Jwt;
using NetDevPack.Security.PasswordHasher.Core;
using MPStore.WebAPI.Core.Database;
using static MPStore.WebAPI.Core.Database.ProviderConfiguration;

namespace MPStore.Identidade.API.Configuration
{
    public static class IdentityConfig
    {
        public static IServiceCollection AddIdentityConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.ConfigureProviderForContext<ApplicationDbContext>(DetectDatabase(configuration));

            services.AddMemoryCache()
                    .AddDataProtection();

            services.AddJwtConfiguration(configuration, "AppSettings")
                    .AddNetDevPackIdentity<IdentityUser>()
                    .PersistKeysToDatabaseStore<ApplicationDbContext>();

            services.AddIdentity<IdentityUser, IdentityRole>(o =>
            {
                o.Password.RequireDigit = false;
                o.Password.RequireLowercase = false;
                o.Password.RequireNonAlphanumeric = false;
                o.Password.RequireUppercase = false;
                o.Password.RequiredUniqueChars = 0;
                o.Password.RequiredLength = 8;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.UpgradePasswordSecurity()
                .WithStrenghten(PasswordHasherStrenght.Moderate)
                .UseArgon2<IdentityUser>();

            return services;
        }
    }
}
