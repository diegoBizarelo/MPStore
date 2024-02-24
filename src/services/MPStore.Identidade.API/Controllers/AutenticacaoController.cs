using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MPStore.Core.Messages.Integration;
using MPStore.Identidade.API.Models;
using MPStore.MessageBus;
using MPStore.WebAPI.Core.Controllers;
using NetDevPack.Identity.Interfaces;
using NetDevPack.Identity.Jwt;

namespace MPStore.Identidade.API.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("api/identidade")]
    public class AutenticacaoController : MainController
    {
        private readonly IJwtBuilder _jwtBuilder;
        private readonly IMessageBus _bus;
        private readonly SignInManager<IdentityUser> SignInManager;
        private readonly UserManager<IdentityUser> UserManager;

        public AutenticacaoController(IJwtBuilder jwtBuilder,
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            IMessageBus bus)
        {
            _jwtBuilder = jwtBuilder;
            SignInManager = signInManager;
            UserManager = userManager;
            _bus = bus;
        }

        [HttpPost("nova-conta")]
        public async Task<ActionResult> Registrar(NovoUsuario novoUsuario)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var usuario = new IdentityUser
            {
                UserName = novoUsuario.Email,
                Email = novoUsuario.Email,
                EmailConfirmed = true,
            };

            var result = await UserManager.CreateAsync(usuario, novoUsuario.Password);
            if (result.Succeeded)
            {
                var clienteResultado = await RegistrarUsuario(novoUsuario);
                if (!clienteResultado.ValidationResult.IsValid)
                {
                    await UserManager.DeleteAsync(usuario);
                    return CustomResponse(clienteResultado.ValidationResult);
                }

                var jwt = await _jwtBuilder.WithEmail(novoUsuario.Email)
                                            .WithJwtClaims()
                                            .WithUserClaims()
                                            .WithUserRoles()
                                            .WithRefreshToken()
                                            .BuildUserResponse();

                return CustomResponse(jwt);
            }

            foreach (var error in result.Errors)
            {
                AddErroParaPilha(error.Description);
            }

            return CustomResponse();
        }

        private async Task<ResponseMessage> RegistrarUsuario(NovoUsuario novoUsuario)
        {
            var user = await UserManager.FindByEmailAsync(novoUsuario.Email);

            var userRegistered = new UsuarioRegistroIntegracaoEvent(Guid.Parse(user.Id), novoUsuario.Nome, novoUsuario.Email, novoUsuario.CPF);

            try
            {
                return await _bus.RequestAsync<UsuarioRegistroIntegracaoEvent, ResponseMessage>(userRegistered);
            }
            catch (Exception)
            {
                await UserManager.DeleteAsync(user);
                throw;
            }
        }

        [HttpPost("autenticar")]
        public async Task<ActionResult> Login(UserLogin userLogin)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var result = await SignInManager.PasswordSignInAsync(userLogin.Email, userLogin.Password,
                false, true);

            if (result.Succeeded)
            {

                var jwt = await _jwtBuilder
                                            .WithEmail(userLogin.Email)
                                            .WithJwtClaims()
                                            .WithUserClaims()
                                            .WithUserRoles()
                                            .WithRefreshToken()
                                            .BuildUserResponse();
                return CustomResponse(jwt);
            }

            if (result.IsLockedOut)
            {
                AddErroParaPilha("Usuário temporariamente bloqueado. Muitas tentativas.");
                return CustomResponse();
            }

            AddErroParaPilha("Senha de usuário incorreta");
            return CustomResponse();
        }


        [HttpPost("refresh-token")]
        public async Task<ActionResult> RefreshToken([FromBody] string refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken))
            {
                AddErroParaPilha("Token inválido");
                return CustomResponse();
            }

            var token = await _jwtBuilder.ValidateRefreshToken(refreshToken);

            if (!token.IsValid)
            {
                AddErroParaPilha("Token expirado");
                return CustomResponse();
            }

            var jwt = await _jwtBuilder
                                        .WithUserId(token.UserId)
                                        .WithJwtClaims()
                                        .WithUserClaims()
                                        .WithUserRoles()
                                        .WithRefreshToken()
                                        .BuildUserResponse();

            return CustomResponse(jwt);
        }
    }
}
