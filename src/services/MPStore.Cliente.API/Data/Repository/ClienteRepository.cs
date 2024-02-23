using Microsoft.EntityFrameworkCore;
using MPStore.Cliente.API.Models;
using MPStore.Core.Data;
using System.Runtime.Intrinsics.X86;

namespace MPStore.Cliente.API.Data.Repository
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly ClienteContexto _context;
        public IUnitOfWork UnitOfWork => _context;

        public ClienteRepository(ClienteContexto context)
        {
            _context = context;
        }

        public void Add(Models.Cliente cliente)
        {
            _context.Clientes.Add(cliente);
        }

        public void AddEndereco(Endereco endereco)
        {
            _context.Enderecos.Add(endereco);
        }

        public async Task<Endereco> BuscarEnderecoPorIdCliente(Guid id)
        {
            return await _context.Enderecos.FirstOrDefaultAsync(e => e.ClienteId == id);
        }

        public Task<Models.Cliente> BuscarPorCPF(string cpf)
        {
            return _context.Clientes.FirstOrDefaultAsync(c => c.CPF == cpf);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task<IEnumerable<Models.Cliente>> GetAll()
        {
            return await _context.Clientes.AsNoTracking().ToListAsync();
        }
    }
}
