using MPStore.Core.Data;

namespace MPStore.Catalogo.API.Models
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        void Adicionar(Produto produto);
        void Atualizar(Produto produto);
        Task<ResultadoPaginado<Produto>> BuscarTodos(int pageSize, int pageIndex, string? query = null);
        Task<Produto> BuscarPorId(Guid id);
        Task<List<Produto>> BuscarProdutosPorId(string ids);
    }
}
