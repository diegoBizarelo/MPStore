using MPStore.CarrinhoCompras.API.Data;
using MPStore.WebAPI.Core.User;

namespace MPStore.CarrinhoCompras.API.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IAspNetUser, AspNetUser>();
            services.AddScoped<ShoppingCartContext>();
            services.AddScoped<ShoppingCart>();
        }
    }
}
