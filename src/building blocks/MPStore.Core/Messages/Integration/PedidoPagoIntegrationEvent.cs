using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPStore.Core.Messages.Integration
{
    public class PedidoPagoIntegrationEvent : IntegrationEvent
    {
        public Guid CustomerId { get; private set; }
        public Guid OrderId { get; private set; }

        public PedidoPagoIntegrationEvent(Guid clienteId, Guid pedidoId)
        {
            CustomerId = clienteId;
            OrderId = pedidoId;
        }
    }
}
