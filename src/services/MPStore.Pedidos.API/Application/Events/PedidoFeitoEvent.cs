using MPStore.Core.Messages;

namespace MPStore.Pedidos.API.Application.Events
{
    public class PedidoFeitoEvent : Evento
    {
        public Guid PedidoId { get; private set; }
        public Guid ClienteId { get; private set; }

        public PedidoFeitoEvent(Guid pedidoId, Guid clienteId)
        {
            PedidoId = pedidoId;
            ClienteId = clienteId;
        }
    }
}
