using System.ComponentModel.DataAnnotations;

namespace DistroFront.DTOs;

public sealed class LoginRequestDto
{
    [Required(ErrorMessage = "Informe o e-mail.")]
    [EmailAddress(ErrorMessage = "Informe um e-mail valido.")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Informe a senha.")]
    [MinLength(6, ErrorMessage = "Use pelo menos 6 caracteres.")]
    [MaxLength(20, ErrorMessage = "Use no maximo 20 caracteres.")]
    public string? Password { get; set; }

    [Compare(nameof(Password), ErrorMessage = "As senhas nao conferem.")]
    public string? ConfirmPassword { get; set; }
}

public sealed class RegisterRequestDto
{
    [Required(ErrorMessage = "Informe o e-mail.")]
    [EmailAddress(ErrorMessage = "Informe um e-mail valido.")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Informe a senha.")]
    public string? Password { get; set; }

    [Compare(nameof(Password), ErrorMessage = "As senhas nao conferem.")]
    public string? ConfirmPassword { get; set; }
}

public sealed class UserTokenDto
{
    public string? Token { get; set; }
    public DateTime Expiration { get; set; }
}
