using MPStore.Core.Mediator;
using MPStore.Pedidos.API.Application.Queries;
using MPStore.Pedidos.Domain.Pedidos;
using MPStore.WebAPI.Core.User;
using MPStore.Pedidos.Infra.Repository;
using MPStore.Pedidos.Infra.Context;

namespace MPStore.Pedidos.API.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IAspNetUser, AspNetUser>();

            services.AddScoped<IMediatorHandler, MediatorHandler>();
            services.AddScoped<IPedidoQueries, PedidoQueries>();

            services.AddScoped<IPedidoRepository, PedidoRepository>();
            services.AddScoped<PedidoContext>();
        }
    }
}
