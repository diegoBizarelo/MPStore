using MPStore.Pedidos.API.Application.DTO;
using MPStore.Pedidos.Domain.Pedidos;

namespace MPStore.Pedidos.API.Application.Queries
{
    public class PedidoQueries : IPedidoQueries
    {
        private readonly IPedidoRepository _pedidoRepository;

        public PedidoQueries(IPedidoRepository pedidoRepository)
        {
            _pedidoRepository = pedidoRepository;
        }

        public async Task<PedidoDTO> GetLastOrder(Guid customerId)
        {
            var order = await _pedidoRepository.ObterUltimoPedido(customerId);

            if (order is null)
                return null;

            return MapOrder(order);
        }

        public async Task<IEnumerable<PedidoDTO>> GetByCustomerId(Guid customerId)
        {
            var orders = await _pedidoRepository.ObterClientesPorId(customerId);

            return orders.Select(PedidoDTO.ToPedidoDTO);
        }

        public async Task<PedidoDTO> GetAuthorizedOrders()
        {
            var orders = await _pedidoRepository.ObterUltimoPedidoAutorizado();

            return MapOrder(orders);
        }

        private PedidoDTO MapOrder(Pedido pedido)
        {
            if (pedido is null)
                return null;

            var pedidoDto = new PedidoDTO
            {
                Id = pedido.Id,
                Codigo = pedido.Codigo,
                ClienteId = pedido.ClienteId,
                Status = (int) pedido.PedidoStatus,
                Data = pedido.DataAdicao,
                Total = pedido.Total,
                Endereco = new EnderecoDTO
                {
                    Logradouro = pedido.Endereco.Logradouro,
                    CEP = pedido.Endereco.CEP,
                    Cidade = pedido.Endereco.Cidade,
                    EnderecoSecundario = pedido.Endereco.EnderecoSecundario,
                    Estado = pedido.Endereco.Estado,
                    Numero = pedido.Endereco.Numero
                },
                PedidoItems = pedido.PedidoItems.Select(item => new PedidoItemDTO
                {
                    PedidoId = item.PedidoId,
                    ProdutoId = item.ProdutoId,
                    Nome = item.NomeProduto,
                    Preco = item.Preco,
                    Imagem = item.ImagemProduto,
                    Quantidade = item.Quantidade
                }).ToList()
            };

            return pedidoDto;
        }
    }
}
