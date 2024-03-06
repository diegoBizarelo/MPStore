using MPStore.Pedidos.Domain.Pedidos;

namespace MPStore.Pedidos.API.Application.DTO
{
    public class PedidoDTO
    {
        public Guid Id { get; set; }
        public int Codigo { get; set; }

        public Guid ClienteId { get; set; }
        public int Status { get; set; }
        public DateTime Data { get; set; }
        public decimal? Total { get; set; }

        public List<PedidoItemDTO> PedidoItems { get; set; }
        public EnderecoDTO Endereco { get; set; }

        public static PedidoDTO ToPedidoDTO(Pedido order)
        {
            var orderDTO = new PedidoDTO
            {
                Id = order.Id,
                Codigo = order.Codigo,
                Status = (int)order.PedidoStatus,
                Data = order.DataAdicao,
                Total = order.Total,
                PedidoItems = new List<PedidoItemDTO>(),
                Endereco = new EnderecoDTO()
            };

            foreach (var item in order.PedidoItems)
            {
                orderDTO.PedidoItems.Add(new PedidoItemDTO
                {
                    Nome = item.NomeProduto,
                    Imagem = item.ImagemProduto,
                    Quantidade = item.Quantidade,
                    ProdutoId = item.ProdutoId,
                    Preco = item.Preco,
                    PedidoId = item.PedidoId
                });
            }

            orderDTO.Endereco = new EnderecoDTO
            {
                Logradouro = order.Endereco.Logradouro,
                Numero = order.Endereco.Numero,
                EnderecoSecundario = order.Endereco.EnderecoSecundario,
                CEP = order.Endereco.CEP,
                Cidade = order.Endereco.Cidade,
                Estado = order.Endereco.Estado,
            };

            return orderDTO;
        }
    }
}
