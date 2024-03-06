using MPStore.Core.DomainObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPStore.Pedidos.Domain.Pedidos
{
    public class Pedido : Entity, IAggregateRoot
    {
        public int Codigo { get; private set; }
        public Guid ClienteId { get; private set; }
        public decimal? Total { get; private set; }
        public DateTime DataAdicao { get; private set; }
        public PedidoStatus PedidoStatus { get; private set; }
        private readonly List<PedidoItem> _pedidoItems;
        public IReadOnlyCollection<PedidoItem> PedidoItems => _pedidoItems;
        public Endereco Endereco { get; private set; }

        public Pedido(Guid clienteId, decimal? total, List<PedidoItem> pedidoItems)
        {
            ClienteId = clienteId;
            Total = total;
            _pedidoItems = pedidoItems;
        }

        protected Pedido() { }

        public void Autorizar()
        {
            PedidoStatus = PedidoStatus.Autorizado;
        }

        public void Cancelar()
        {
            PedidoStatus = PedidoStatus.Cancelado;
        }

        public void Finalizar()
        {
            PedidoStatus = PedidoStatus.Pago;
        }

        public void SetEndereco(Endereco endereco)
        {
            Endereco = endereco;
        }

        public void CalcularValorPedido()
        {
            Total = PedidoItems.Sum(p => p.CalcularValor());
        }

        
    }
}
