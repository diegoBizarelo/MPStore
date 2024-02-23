using FluentValidation;
using MPStore.Core.Messages;

namespace MPStore.Cliente.API.Application.Commands
{
    public class NovoClienteCommand : Comando
    {
        public Guid Id { get; private set; }
        public string Nome { get; private set; }
        public string Email { get; private set; }
        public string CPF { get; private set; }

        public NovoClienteCommand(Guid id, string name, string email, string cpf)
        {
            AggregateId = id;
            Id = id;
            Nome = name;
            Email = email;
            CPF = cpf;
        }

        public override bool IsValid()
        {
            ValidationResult = new NovoClienteValidation().Validate(this);
            return ValidationResult.IsValid;
        }

        public class NovoClienteValidation : AbstractValidator<NovoClienteCommand>
        {
            public NovoClienteValidation()
            {
                RuleFor(c => c.Id)
                    .NotEqual(Guid.Empty)
                    .WithMessage("Invalid customer id");

                RuleFor(c => c.Nome)
                    .NotEmpty()
                    .WithMessage("Customer name must be set");

                RuleFor(c => c.CPF)
                    .Must(CpfValido)
                    .WithMessage("Invalid SSN.");

                RuleFor(c => c.Email)
                    .Must(HasValidEmail)
                    .WithMessage("Wrong e-mail.");
            }

            protected static bool CpfValido(string cpf)
            {
                return !string.IsNullOrEmpty(cpf);
            }

            protected static bool HasValidEmail(string email)
            {
                return Core.DomainObjects.Email.Validate(email);
            }
        }
    }
}
