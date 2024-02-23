using MPStore.Cliente.API.Data;
using MPStore.Cliente.API.Data.Repository;
using MPStore.Cliente.API.Models;
using MPStore.WebAPI.Core.User;

namespace MPStore.Cliente.API.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IAspNetUser, AspNetUser>();

            services.AddScoped<IClienteRepository, ClienteRepository>();
            services.AddScoped<ClienteContexto>();
        }
    }
}
