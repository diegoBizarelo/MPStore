using MPStore.Core.DomainObjects;

namespace MPStore.Cliente.API.Models
{
    public class Endereco : Entity, IAggregateRoot
    {
        public string? Logradouro { get; private set; }
        public string? Numero { get; private set; }
        public string? EnderecoSecundario { get; private set; }
        public string? CEP { get; private set; }
        public string? Cidade { get; private set; }
        public string? Estado { get; private set; }
        public Guid ClienteId { get; private set; }

        public Cliente? Cliente { get; protected set; }

        public Endereco(string logradouro, string numero, string enderecoSecundario, string cEP, string cidade, string estado, Guid clienteId)
        {
            Logradouro = logradouro;
            Numero = numero;
            EnderecoSecundario = enderecoSecundario;
            CEP = cEP;
            Cidade = cidade;
            Estado = estado;
            ClienteId = clienteId;
        }

        protected Endereco() { }
    }
}
