using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Logging;
using MPStore.WebAPI.Core.Extensions;
using NetDevPack.Security.JwtExtensions;
using System.Diagnostics;

namespace MPStore.WebAPI.Core.Identity
{
    public static class JwtConfig
    {
        public static void AddJwtConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var appSettingsSection = configuration.GetSection("AppSettings");
            services.Configure<JwkOptions>(appSettingsSection);

            var jwkOptions = appSettingsSection.Get<JwkOptions>();
            jwkOptions.KeepFor = TimeSpan.FromMinutes(15);
            if (Debugger.IsAttached)
            {
                IdentityModelEventSource.ShowPII = true;
            }

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.BackchannelHttpHandler = HttpExtensions.ConfigureClientHandler();
                options.SaveToken = true;
                options.SetJwksOptions(jwkOptions);
            });

            services.AddAuthorization();
        }
    }
}
