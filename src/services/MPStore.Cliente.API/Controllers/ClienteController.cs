using MediatR;
using Microsoft.AspNetCore.Mvc;
using MPStore.Cliente.API.Application.Commands;
using MPStore.Cliente.API.Models;
using MPStore.WebAPI.Core.Controllers;
using MPStore.WebAPI.Core.User;

namespace MPStore.Cliente.API.Controllers
{
    [Route("cliente")]
    public class ClienteController : MainController
    {
        private readonly IClienteRepository _clienteRepository;
        private readonly IMediator _mediator;
        private readonly IAspNetUser _usuario;

        public ClienteController(IClienteRepository clienteRepository ,IMediator mediator, IAspNetUser usuario)
        {
            _clienteRepository = clienteRepository;
            _mediator = mediator;
            _usuario = usuario;
        }

        [HttpGet("endereo")]
        public async Task<IActionResult> GetEndereco()
        {
            var endereco = await _clienteRepository.BuscarEnderecoPorIdCliente(_usuario.ObterUsuarioId());

            return endereco != null ? CustomResponse(endereco) : NotFound();
        }

        [HttpPost("endereco")]
        public async Task<IActionResult> AddAddress(AddEnderecoCommand address)
        {
            address.ClienteId = _usuario.ObterUsuarioId();
            return CustomResponse(await _mediator.Send(address));
        }
    }
}