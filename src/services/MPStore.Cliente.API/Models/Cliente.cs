using MPStore.Core.DomainObjects;

namespace MPStore.Cliente.API.Models
{
    public class Cliente : Entity, IAggregateRoot
    {
        public string Nome { get; private set; }
        public Email Email { get; private set; }
        public string CPF { get; private set; }
        public bool Excluido { get; private set; }
        public Endereco Endereco { get; private set; }
        
        public Cliente(Guid id, string nome, string email, string cpf)
        {
            Id = id;
            Nome = nome;
            Email = new Email(email);
            CPF = cpf;
            Excluido = false;
        }

        protected Cliente() { }

        public void TrocarEmail(string email)
        {
            Email = new Email(email);
        }

        public void AtribuirEndereco(Endereco endereco)
        {
            Endereco = endereco;
        }
    }
}
