using System.Security.Claims;

namespace MPStore.WebAPI.Core.User
{
    public interface IAspNetUser
    {
        string Nome { get; }
        Guid ObterUsuarioId();
        string ObterUsuarioEmail();
        string ObterUsuarioToken();
        string ObterUsuarioRefreshToken();
        bool EstaAutenticado();
        bool PossuiPermisao(string role);
        IEnumerable<Claim> GetClaims();
        HttpContext GetHttpContext();
    }
}
