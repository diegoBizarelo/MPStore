using MPStore.Core.DomainObjects;
using MPStore.Core.Messages.Integration;
using MPStore.MessageBus;
using MPStore.Pedidos.Domain.Pedidos;

namespace MPStore.Pedidos.API.Services
{
    public class PedidoIntegrationHandler : BackgroundService
    {
        private readonly IMessageBus _bus;
        private readonly IServiceProvider _serviceProvider;

        public PedidoIntegrationHandler(IServiceProvider serviceProvider, IMessageBus bus)
        {
            _serviceProvider = serviceProvider;
            _bus = bus;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _bus.SubscribeAsync<PedidoCanceladoIntegrationEvent>("PedidoCancelado", CancelarPedido);

            await _bus.SubscribeAsync<PedidoPagoIntegrationEvent>("PedidoPago", FinalizarPedido);
        }

        private async Task CancelarPedido(PedidoCanceladoIntegrationEvent message)
        {
            using var scope = _serviceProvider.CreateScope();

            var pedidoRepository = scope.ServiceProvider.GetRequiredService<IPedidoRepository>();

            var pedido = await pedidoRepository.GetById(message.OrderId);
            pedido.Cancelar();

            pedidoRepository.Update(pedido);

            if (!await pedidoRepository.UnitOfWork.CommitAsync())
            {
                throw new DomainException($"Houve um problema ao tentar cancelar o pedido {message.OrderId}");
            }
        }

        private async Task FinalizarPedido(PedidoPagoIntegrationEvent message)
        {
            using var scope = _serviceProvider.CreateScope();

            var pedidoRepository = scope.ServiceProvider.GetRequiredService<IPedidoRepository>();

            var pedido = await pedidoRepository.GetById(message.OrderId);
            pedido.Finalizar();

            pedidoRepository.Update(pedido);

            if (!await pedidoRepository.UnitOfWork.CommitAsync())
            {
                throw new DomainException($"Houve um problema ao tentar finalizar o pedido {message.OrderId}");
            }
        }
    }
}
