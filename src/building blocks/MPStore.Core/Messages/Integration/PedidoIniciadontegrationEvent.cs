using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPStore.Core.Messages.Integration
{
    public class PedidoIniciadontegrationEvent : IntegrationEvent
    {
        public Guid ClienteId { get; set; }
        public Guid PedidoId { get; set; }
        public int TipoPagamento { get; set; }
        public decimal? Total { get; set; }
        public string Holder { get; set; }
        public string NumeroCartao { get; set; }
        public string DataValidade { get; set; }
        public string CodigoSeguraca { get; set; }
    }
}
