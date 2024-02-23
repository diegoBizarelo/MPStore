using FluentValidation.Results;
using MediatR;
using MPStore.Cliente.API.Application.Commands;
using MPStore.Core.Messages.Integration;
using MPStore.MessageBus;
using NetDevPack.Messaging;
using ResponseMessage = MPStore.Core.Messages.Integration.ResponseMessage;

namespace MPStore.Cliente.API.Services
{
    public class NovoClienteHandler : BackgroundService
    {
        private readonly IMessageBus _bus;
        private readonly IServiceProvider _serviceProvider;

        public NovoClienteHandler(
                            IServiceProvider serviceProvider,
                            IMessageBus bus)
        {
            _serviceProvider = serviceProvider;
            _bus = bus;
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _bus.RespondAsync<UsuarioRegistroIntegracaoEvent, ResponseMessage>(AddCustomer);

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromSeconds(15), stoppingToken);
            }
        }

        private async Task<ResponseMessage> AddCustomer(UsuarioRegistroIntegracaoEvent message)
        {
            var customerCommand = new NovoClienteCommand(message.Id, message.Nome, message.Email, message.CPF);
            ValidationResult sucesso;

            using (var scope = _serviceProvider.CreateScope())
            {
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                sucesso = await mediator.Send(customerCommand);
            }

            return new ResponseMessage(sucesso);
        }

    }
}
