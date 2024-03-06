using Microsoft.EntityFrameworkCore;
using MPStore.Core.Data;
using MPStore.Pedidos.Domain.Pedidos;
using MPStore.Pedidos.Infra.Context;
using System.Data.Common;

namespace MPStore.Pedidos.Infra.Repository
{
    public class PedidoRepository : IPedidoRepository
    {
        private readonly PedidoContext _context;

        public PedidoRepository(PedidoContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public DbConnection GetConnection() => _context.Database.GetDbConnection();

        public async Task<Pedido> GetById(Guid id) => await _context.Pedido.FindAsync(id);

        public async Task<IEnumerable<Pedido>> ObterClientesPorId(Guid clienteId)
        {
            return await _context.Pedido
                .Include(p => p.PedidoItems)
                .AsNoTracking()
                .Where(p => p.ClienteId == clienteId)
                .ToListAsync();
        }

        public void Add(Pedido pedido)
        {
            _context.Pedido.Add(pedido);
        }

        public void Update(Pedido pedido)
        {
            _context.Pedido.Update(pedido);
        }


        public async Task<PedidoItem> ObterItemPorId(Guid id) => await _context.PedidoItems.FindAsync(id);
        

        public async Task<PedidoItem> ObterItemPorPedido(Guid pedidoId, Guid produtoId)
        {
            return await _context.PedidoItems
                            .FirstOrDefaultAsync(p => p.ProdutoId == produtoId && p.PedidoId == pedidoId);
        }

        public Task<Pedido> ObterUltimoPedido(Guid clienteId)
        {
            var fiveMinutesAgo = DateTime.Now.AddMinutes(-5);

            return _context.Pedido
                    .Include(i => i.PedidoItems)
                    .Where(o => o.ClienteId == clienteId && o.DataAdicao > fiveMinutesAgo && o.DataAdicao <= DateTime.Now)
                    .OrderByDescending(o => o.DataAdicao).FirstOrDefaultAsync();
        }

        public Task<Pedido> ObterUltimoPedidoAutorizado()
        {
            return _context.Pedido.Include(i => i.PedidoStatus)
                    .Where(o => o.PedidoStatus == PedidoStatus.Autorizado)
                    .OrderBy(o => o.DataAdicao).FirstOrDefaultAsync();
        }

        public void Dispose() => _context.Dispose();
    }
}
