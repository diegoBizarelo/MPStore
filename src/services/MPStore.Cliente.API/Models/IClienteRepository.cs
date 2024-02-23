using MPStore.Core.Data;

namespace MPStore.Cliente.API.Models
{
    public interface IClienteRepository : IRepository<Cliente>
    {
        void Add(Cliente cliente);
        Task<IEnumerable<Cliente>> GetAll();
        Task<Cliente> BuscarPorCPF(string cpf);
        void AddEndereco(Endereco endereco);
        Task<Endereco> BuscarEnderecoPorIdCliente(Guid id);
    }
}
