using MPStore.MessageBus;
using MPStore.Core.Utils;
using MPStore.Cliente.API.Services;

namespace MPStore.Cliente.API.Configuration
{
    public static class MessageBusConfig
    {
        public static void AddMessageBusConfiguration(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddMessageBus(configuration.GetMessageQueueConnection("MessageBus"))
                .AddHostedService<NovoClienteHandler>();
        }
    }
}
