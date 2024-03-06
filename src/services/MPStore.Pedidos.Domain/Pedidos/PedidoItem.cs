using MPStore.Core.DomainObjects;

namespace MPStore.Pedidos.Domain.Pedidos
{
    public class PedidoItem : Entity
    {
        public Guid PedidoId { get; private set; }
        public Guid ProdutoId { get; private set; }
        public string? NomeProduto { get; private set; }
        public int Quantidade { get; private set; }
        public decimal Preco { get; private set; }
        public string? ImagemProduto { get; set; }

        // EF Rel.
        public Pedido? Pedido { get; set; }

        public PedidoItem(Guid produtoId, string nomeProduto, int quantidade,
            decimal preco, string? imagemProduto = null)
        {
            ProdutoId = produtoId;
            NomeProduto = nomeProduto;
            Quantidade = quantidade;
            Preco = preco;
            ImagemProduto = imagemProduto;
        }

        // EF ctor
        protected PedidoItem() { }

        internal decimal CalcularValor()
        {
            return Quantidade * Preco;
        }
    }
}
