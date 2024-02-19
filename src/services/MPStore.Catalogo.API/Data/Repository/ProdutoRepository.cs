using Microsoft.EntityFrameworkCore;
using MPStore.Catalogo.API.Models;
using MPStore.Core.Data;

namespace MPStore.Catalogo.API.Data.Repository
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly CatalogoContexto _context;

        public ProdutoRepository(CatalogoContexto context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public void Adicionar(Produto produto)
        {
            _context.Produtos.Add(produto);
        }

        public void Atualizar(Produto produto)
        {
            _context.Update(produto);
        }

        public async Task<Produto> BuscarPorId(Guid id)
        {
            return await _context.Produtos.FindAsync(id);
        }

        public async Task<List<Produto>> BuscarProdutosPorId(string ids)
        {
            var idsGuid = ids.Split(',').Select(id => (Ok: Guid.TryParse(id, out var x), Value: x));

            if (!idsGuid.All(nid => nid.Ok)) 
                return new List<Produto>();

            var idsValue = idsGuid.Select(id => id.Value);

            return await _context.Produtos
                            .AsNoTracking()
                            .Where(p => idsValue.Contains(p.Id) && p.Ativo)
                            .ToListAsync();
        }

        public async Task<ResultadoPaginado<Produto>> BuscarTodos(int pageSize, int pageIndex, string? query = null)
        {
            var catalogoQuery = _context.Produtos.AsQueryable();

            var catalogo = await catalogoQuery.AsNoTrackingWithIdentityResolution()
                                            .Where(x => EF.Functions.Like(x.Nome, $"%{query}%"))
                                            .OrderBy(x => x.Nome)
                                            .Skip(pageSize * (pageIndex - 1))
                                            .Take(pageSize).ToListAsync();

            var total = await catalogoQuery.AsNoTrackingWithIdentityResolution()
                                          .Where(x => EF.Functions.Like(x.Nome, $"%{query}%"))
                                          .CountAsync();


            return new ResultadoPaginado<Produto>()
            {
                Lista = catalogo,
                ResultadoTotal = total,
                IndexPagina = pageIndex,
                TamanhoTotalPagina = pageSize,
                Query = query
            };
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
