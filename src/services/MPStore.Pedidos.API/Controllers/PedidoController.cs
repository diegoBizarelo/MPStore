using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MPStore.Core.Mediator;
using MPStore.Pedidos.API.Application.Commands;
using MPStore.Pedidos.API.Application.DTO;
using MPStore.Pedidos.API.Application.Queries;
using MPStore.WebAPI.Core.Controllers;
using MPStore.WebAPI.Core.User;

namespace MPStore.Pedidos.API.Controllers
{
    [Authorize, Route("pedidos")]
    public class PedidoController : MainController
    {
        private readonly IMediatorHandler _mediator;
        private readonly IAspNetUser _user;
        private readonly IPedidoQueries _pedidoQueries;

        public PedidoController(IMediatorHandler mediator,
            IAspNetUser user,
            IPedidoQueries pedidoQueries)
        {
            _mediator = mediator;
            _user = user;
            _pedidoQueries = pedidoQueries;
        }

        [HttpPost("")]
        public async Task<IActionResult> AddPedido(AddPedidoCommand order)
        {
            order.ClienteId = _user.ObterUsuarioId();
            return CustomResponse(await _mediator.SendCommand(order));
        }

        [HttpGet("ultimo")]
        public async Task<ActionResult<PedidoDTO>> UltimoPedido()
        {
            var pedido = await _pedidoQueries.GetLastOrder(_user.ObterUsuarioId());

            return pedido == null ? NoContent() : CustomResponse(pedido);
        }

        [HttpGet("clientes")]
        public async Task<ActionResult<IEnumerable<PedidoDTO>>> Clientes()
        {
            var pedidos = await _pedidoQueries.GetByCustomerId(_user.ObterUsuarioId());

            return pedidos == null ? NoContent() : CustomResponse(pedidos);
        }
    }
}
