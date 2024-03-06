using MPStore.Core.Data;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPStore.Pedidos.Domain.Pedidos
{
    public interface IPedidoRepository : IRepository<Pedido>
    {
        Task<Pedido> GetById(Guid id);
        Task<IEnumerable<Pedido>> ObterClientesPorId(Guid clienteId);
        void Add(Pedido pedido);
        void Update(Pedido pedido);
        DbConnection GetConnection();
        Task<Pedido> ObterUltimoPedido(Guid clienteId);
        Task<Pedido> ObterUltimoPedidoAutorizado();
        Task<PedidoItem> ObterItemPorId(Guid id);
        Task<PedidoItem> ObterItemPorPedido(Guid pedidoId, Guid produtoId);
    }
}
