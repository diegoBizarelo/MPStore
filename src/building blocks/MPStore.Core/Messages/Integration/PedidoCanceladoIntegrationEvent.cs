using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPStore.Core.Messages.Integration
{
    public class PedidoCanceladoIntegrationEvent : IntegrationEvent
    {
        public Guid CustomerId { get; private set; }
        public Guid OrderId { get; private set; }

        public PedidoCanceladoIntegrationEvent(Guid customerId, Guid orderId)
        {
            CustomerId = customerId;
            OrderId = orderId;
        }
    }
}
