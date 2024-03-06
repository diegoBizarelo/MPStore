using FluentValidation.Results;
using MediatR;
using MPStore.Core.Messages;
using MPStore.Core.Messages.Integration;
using MPStore.MessageBus;
using MPStore.Pedidos.API.Application.DTO;
using MPStore.Pedidos.API.Application.Events;
using MPStore.Pedidos.Domain.Pedidos;


namespace MPStore.Pedidos.API.Application.Commands
{
    public class PedidoCommandHandler : ComandoHandler,
        IRequestHandler<AddPedidoCommand, ValidationResult>
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly IMessageBus _bus;

        public PedidoCommandHandler(IPedidoRepository pedidoRepository,
                                   IMessageBus bus)
        {
            _pedidoRepository = pedidoRepository;
            _bus = bus;
        }

        public async Task<ValidationResult> Handle(AddPedidoCommand message, CancellationToken cancellationToken)
        {
            if (!message.IsValid()) return message.ValidationResult;

            var pedido = MapPedido(message);

            if (!PedidoValido(pedido)) return ValidationResult;

            if (!await FazerPagamento(pedido, message)) return ValidationResult;

            pedido.Autorizar();

            pedido.AdcionarEvento(new PedidoFeitoEvent(pedido.Id, pedido.ClienteId));

            _pedidoRepository.Add(pedido);

            return await PersistirDado(_pedidoRepository.UnitOfWork);
        }

        private Pedido MapPedido(AddPedidoCommand message)
        {
            var address = new Endereco
            {
                Logradouro = message.Endereco.Logradouro,
                Numero = message.Endereco.Numero,
                EnderecoSecundario = message.Endereco.EnderecoSecundario,
                CEP = message.Endereco.CEP,
                Cidade = message.Endereco.Cidade,
                Estado = message.Endereco.Estado
            };

            var pedido = new Pedido(message.ClienteId, message.Total, message.PedidoItems.Select(PedidoItemDTO.ToPedidoItem).ToList());

            pedido.SetEndereco(address);
            return pedido;
        }

        private bool PedidoValido(Pedido pedido)
        {
            var pedidoAmount = pedido.Total;

            pedido.CalcularValorPedido();

            if (pedido.Total != pedidoAmount)
            {
                AddError("O valor total do pedido é diferente do valor total de itens individuais");
                return false;
            }

            return true;
        }

        public async Task<bool> FazerPagamento(Pedido pedido, AddPedidoCommand message)
        {
            var pedidoStarted = new PedidoIniciadontegrationEvent
            {
                PedidoId = pedido.Id,
                ClienteId = pedido.ClienteId,
                Total = pedido.Total,
                TipoPagamento = 1,
                Holder = message.Holder,
                NumeroCartao = message.NumeroCartao,
                DataValidade = message.DataValidade,
                CodigoSeguraca = message.CodigoSeguranca
            };

            var result = await _bus
                .RequestAsync<PedidoIniciadontegrationEvent, ResponseMessage>(pedidoStarted);

            if (result.ValidationResult.IsValid) return true;

            foreach (var erro in result.ValidationResult.Errors)
            {
                AddError(erro.ErrorMessage);
            }

            return false;
        }
    }
}
