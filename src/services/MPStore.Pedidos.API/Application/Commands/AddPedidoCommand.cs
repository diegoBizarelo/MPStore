using FluentValidation;
using MPStore.Core.Messages;
using MPStore.Pedidos.API.Application.DTO;

namespace MPStore.Pedidos.API.Application.Commands
{
    public class AddPedidoCommand : Comando
    {
        public Guid ClienteId { get; set; }
        public decimal? Total { get; set; }
        public List<PedidoItemDTO> PedidoItems { get; set; }
        public EnderecoDTO Endereco { get; set; }
        public string NumeroCartao { get; set; }
        public string Holder { get; set; }
        public string DataValidade { get; set; }
        public string CodigoSeguranca { get; set; }

        public override bool IsValid()
        {
            ValidationResult = new AddOPedidoValidation().Validate(this);
            return ValidationResult.IsValid;
        }

        public class AddOPedidoValidation : AbstractValidator<AddPedidoCommand>
        {
            public AddOPedidoValidation()
            {
                RuleFor(c => c.ClienteId)
                    .NotEqual(Guid.Empty)
                    .WithMessage("Identificação de cliente inválida");

                RuleFor(c => c.PedidoItems.Count)
                    .GreaterThan(0)
                    .WithMessage("O pedido deve ter ao menos um item");

                RuleFor(c => c.Total)
                    .GreaterThan(0)
                    .WithMessage("Total inválido");

                RuleFor(c => c.NumeroCartao)
                    .CreditCard()
                    .WithMessage("Cartão inválido");

                RuleFor(c => c.Holder)
                    .NotNull()
                    .WithMessage("Holder é obrigatório.");

                RuleFor(c => c.CodigoSeguranca.Length)
                    .GreaterThan(2)
                    .LessThan(5)
                    .WithMessage("O código de seguraça deve ter entre 3 ou 4 números.");

                RuleFor(c => c.DataValidade)
                    .NotNull()
                    .WithMessage("Data expiração é obrigatório.");
            }
        }
    }
}
