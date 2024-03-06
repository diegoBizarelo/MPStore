using MPStore.Pedidos.API.Application.DTO;

namespace MPStore.Pedidos.API.Application.Queries
{
    public interface IPedidoQueries
    {
        Task<PedidoDTO> GetLastOrder(Guid clienteId);
        Task<IEnumerable<PedidoDTO>> GetByCustomerId(Guid clienteId);
        Task<PedidoDTO> GetAuthorizedOrders();
    }
}
