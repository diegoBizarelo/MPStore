using MPStore.Pedidos.Domain.Pedidos;

namespace MPStore.Pedidos.API.Application.DTO
{
    public class PedidoItemDTO
    {
        public Guid PedidoId { get; set; }
        public Guid ProdutoId { get; set; }
        public string Nome { get; set; }
        public decimal Preco { get; set; }
        public string Imagem { get; set; }
        public int Quantidade { get; set; }

        public static PedidoItem ToPedidoItem(PedidoItemDTO orderItemDto)
        {
            return new PedidoItem(orderItemDto.PedidoId, orderItemDto.Nome, orderItemDto.Quantidade,
                orderItemDto.Preco, orderItemDto.Imagem);
        }
    }
}
