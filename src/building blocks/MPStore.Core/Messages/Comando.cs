using FluentValidation.Results;
using MediatR;
using MPStore.Core.Messagem;

namespace MPStore.Core.Messages
{
    public class Comando : Mensagem, IRequest<ValidationResult>
    {
        public DateTime Timestamp { get; private set; }
        public ValidationResult ValidationResult { get; set; }
        public Comando() 
        { 
            Timestamp = DateTime.Now;
        }

        public virtual bool IsValid()
        {
            throw new NotImplementedException();
        }
    }
}
