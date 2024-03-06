using MPStore.Core.DomainObjects;
using MPStore.Core.Messages.Integration;
using MPStore.MessageBus;
using MPStore.Pedidos.API.Application.Queries;
using MPStore.Pedidos.Domain.Pedidos;

namespace MPStore.Pedidos.API.Services
{
    public class PedidoOrquestratorIntegrationHandler : IHostedService, IDisposable
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<PedidoOrquestratorIntegrationHandler> _logger;
        private Timer _timer;

        public PedidoOrquestratorIntegrationHandler(ILogger<PedidoOrquestratorIntegrationHandler> logger,
            IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Pedido service inicializado.");

            _timer = new Timer(OrquestrateOrders, null, TimeSpan.Zero,
                TimeSpan.FromSeconds(15));

            return Task.CompletedTask;
        }

        private async void OrquestrateOrders(object state)
        {
            using var scope = _serviceProvider.CreateScope();

            var orderQueries = scope.ServiceProvider.GetRequiredService<IPedidoQueries>();
            var order = await orderQueries.GetAuthorizedOrders();

            if (order == null) return;

            var bus = scope.ServiceProvider.GetRequiredService<IMessageBus>();

            var authorizedOrder = new OrderAuthorizedIntegrationEvent(order.ClienteId, order.Id,
                order.PedidoItems.ToDictionary(p => p.ProdutoId, p => p.Quantidade));

            await bus.PublishAsync(authorizedOrder);

            var message = $"Pedido ID: {order.Id} enviado para baixa no estoque.";
            _logger.LogInformation(message);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Pedido service finalizado.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
