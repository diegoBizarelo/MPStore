using FluentValidation.Results;
using MPStore.Core.Data;

namespace MPStore.Core.Messages
{
    public class ComandoHandler
    {
        protected ValidationResult ValidationResult;

        protected ComandoHandler() 
        { 
            ValidationResult = new ValidationResult();
        }

        protected void AddError(string mensagem) 
        {
            ValidationResult.Errors.Add(new ValidationFailure(string.Empty, mensagem));
        }

        protected async Task<ValidationResult> PersistirDado(IUnitOfWork uow) 
        {
            if (!await uow.CommitAsync()) AddError("Um error ocorreu enquanto tentava salvar.");
            return ValidationResult;
        }
    }
}
