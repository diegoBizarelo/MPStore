using MediatR;
using MPStore.Core.Messages.Integration;
using MPStore.MessageBus;

namespace MPStore.Pedidos.API.Application.Events
{
    public class PedidoEventHandler : INotificationHandler<PedidoFeitoEvent>
    {
        private readonly IMessageBus _bus;

        public PedidoEventHandler(IMessageBus bus)
        {
            _bus = bus;
        }

        public async Task Handle(PedidoFeitoEvent message, CancellationToken cancellationToken)
        {
            await _bus.PublishAsync(new PedidoFeitoIntegrationEvent(message.ClienteId));
        }
    }
}
