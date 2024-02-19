using MPStore.Core.DomainObjects;

namespace MPStore.Catalogo.API.Models
{
    public class Produto : Entity ,IAggregateRoot
    {
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public bool Ativo { get; set; }
        public DateTime DataAdicao { get; set; }
        public string Imagem { get; set; }
        public int QtdEstoque { get; set; }
        public decimal Preco { get; set; }

        public void RemoverProdutoEstoque(int qtd)
        {
            if (QtdEstoque >= qtd)
                QtdEstoque -= qtd;
        }

        public bool EstaDisponivel(int qtd)
        {
            return Ativo && QtdEstoque >= qtd;
        }
    }
}