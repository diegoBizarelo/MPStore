using MPStore.Core.Utils;
using MPStore.MessageBus;
using MPStore.Pedidos.API.Services;

namespace MPStore.Pedidos.API.Configuration
{
    public static class MessageBusConfig
    {
        public static void AddMessageBusConfiguration(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddMessageBus(configuration.GetMessageQueueConnection("MessageBus"))
                .AddHostedService<PedidoOrquestratorIntegrationHandler>()
                .AddHostedService<PedidoIntegrationHandler>();
        }
    }
}
