using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPStore.Core.Messages.Integration
{
    public class PedidoFeitoIntegrationEvent : IntegrationEvent
    {
        public Guid ClienteId { get; private set; }

        public PedidoFeitoIntegrationEvent(Guid clienteId)
        {
            ClienteId = clienteId;
        }
    }
}
