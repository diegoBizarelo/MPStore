using FluentValidation.Results;
using MediatR;
using MPStore.Core.Messages;

namespace MPStore.Core.Mediator
{
    public class MediatorHandler : IMediatorHandler
    {
        private readonly IMediator _mediator;

        public MediatorHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<ValidationResult> SendCommand<T>(T comando) where T : Comando
        {
            return await _mediator.Send(comando);
        }

        public async Task PublishEvent<T>(T evento) where T : Evento
        {
            await _mediator.Publish(evento);
        }
    }
}
