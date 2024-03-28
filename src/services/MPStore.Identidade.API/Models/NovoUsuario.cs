using System.ComponentModel.DataAnnotations;

namespace MPStore.Identidade.API.Models;

public class NovoUsuario
{
    [Required(ErrorMessage = "O campo {0} é obrigatóio")]
    public string? Nome { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatóio")]
    public string? CPF { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatóio")]
    [EmailAddress(ErrorMessage = "Campo no formato inválido {0}")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatóio")]
    [StringLength(100, ErrorMessage = "O campo {0} dever se entre {2} e {1} caracteres", MinimumLength = 6)]
    public string? Password { get; set; }

    [Compare("Password", ErrorMessage = "As senhas precisam ser iguais.")]
    public string? ConfirmPassword { get; set; }
}

public class UserLogin
{
    [Required(ErrorMessage = "O campo {0} é obrigatóio")]
    [EmailAddress(ErrorMessage = "Campo no formato inválido {0}")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatóio")]
    [StringLength(100, ErrorMessage = "O campo {0} dever se entre {2} e {1} caracteres", MinimumLength = 6)]
    public string? Password { get; set; }
}

public class UserLoginResponse
{
    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }
    public double ExpiresIn { get; set; }
    public UserToken? UserToken { get; set; }
}

public class UserToken
{
    public string? Id { get; set; }
    public string? Email { get; set; }
    public IEnumerable<UserClaim>? Claims { get; set; }
}

public class UserClaim
{
    public string? Value { get; set; }
    public string? Type { get; set; }
}
