using FluentValidation;
using MPStore.Core.Messages;

namespace MPStore.Cliente.API.Application.Commands
{
    public class AddEnderecoCommand : Comando
    {
        public Guid ClienteId { get; set; }
        public string Logradouro { get; set; }
        public string Numero { get; set; }
        public string EnderecoSecundario { get; set; }
        public string Cep { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }

        public AddEnderecoCommand()
        {
        }

        public AddEnderecoCommand(Guid clienteId, string logradouro, string numero, string enderecoSecundario,
            string cep, string cidade, string estado)
        {
            AggregateId = clienteId;
            ClienteId = clienteId;
            Logradouro = logradouro;
            Numero = numero;
            EnderecoSecundario = enderecoSecundario;
            Cep = cep;
            Cidade = cidade;
            Estado = estado;
        }

        public override bool IsValid()
        {
            ValidationResult = new EnderecoValidation().Validate(this);
            return ValidationResult.IsValid;
        }

        public class EnderecoValidation : AbstractValidator<AddEnderecoCommand>
        {
            public EnderecoValidation()
            {
                RuleFor(c => c.Logradouro)
                    .NotEmpty()
                    .WithMessage("Street Address must be set");

                RuleFor(c => c.Numero)
                    .NotEmpty()
                    .WithMessage("Building number must be set");

                RuleFor(c => c.Cep)
                    .NotEmpty()
                    .WithMessage("Zip code must be set");

                RuleFor(c => c.Cidade)
                    .NotEmpty()
                    .WithMessage("City must be set");

                RuleFor(c => c.Estado)
                    .NotEmpty()
                    .WithMessage("State must be set");
            }
        }
    }
}
