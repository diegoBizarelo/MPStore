using MPStore.MessageBus;
using MPStore.Core.Utils;
using MPStore.CarrinhoCompras.API.Services;

namespace MPStore.CarrinhoCompras.API.Configuration
{
    public static class MessageBusConfig
    {
        public static void AddMessageBusConfiguration(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddMessageBus(configuration.GetMessageQueueConnection("MessageBus"))
                .AddHostedService<ShoppingCartIntegrationHandler>();
        }
    }
}
