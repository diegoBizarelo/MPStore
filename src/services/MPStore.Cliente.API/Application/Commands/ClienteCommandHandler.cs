using FluentValidation.Results;
using MediatR;
using MPStore.Cliente.API.Application.Events;
using MPStore.Cliente.API.Models;
using MPStore.Core.Messages;

namespace MPStore.Cliente.API.Application.Commands
{
    public class ClienteCommandHandler : ComandoHandler,
        IRequestHandler<NovoClienteCommand, ValidationResult>,
        IRequestHandler<AddEnderecoCommand, ValidationResult>
    {
        private readonly IClienteRepository _clienteRepository;

        public ClienteCommandHandler(IClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }

        public async Task<ValidationResult> Handle(NovoClienteCommand message, CancellationToken cancellationToken)
        {
            if (!message.IsValid()) return message.ValidationResult;

            var cliente = new MPStore.Cliente.API.Models.Cliente(message.Id, message.Nome, message.Email, message.CPF);

            var clienteExist = await _clienteRepository.BuscarPorCPF(cliente.CPF);

            if (clienteExist != null)
            {
                AddError("Already has this social number.");
                return ValidationResult;
            }

            _clienteRepository.Add(cliente);

            cliente.AdcionarEvento(new NovoClienteAdicionadoEvent(message.Id, message.Nome, message.Email, message.CPF));

            return await PersistirDado(_clienteRepository.UnitOfWork);
        }

        public async Task<ValidationResult> Handle(AddEnderecoCommand message, CancellationToken cancellationToken)
        {
            if (!message.IsValid()) return message.ValidationResult;

            var endereco = new Endereco(message.Logradouro, message.Numero, message.EnderecoSecundario, message.Cep, message.Cidade, message.Estado, message.ClienteId);
            _clienteRepository.AddEndereco(endereco);

            return await PersistirDado(_clienteRepository.UnitOfWork);
        }
    }
}
