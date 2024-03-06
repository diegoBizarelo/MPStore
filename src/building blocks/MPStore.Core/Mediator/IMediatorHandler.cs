using FluentValidation.Results;
using MPStore.Core.Messages;

namespace MPStore.Core.Mediator
{
    public interface IMediatorHandler
    {
        Task PublishEvent<T>(T evento) where T : Evento;
        Task<ValidationResult> SendCommand<T>(T comando) where T : Comando;
    }
}
