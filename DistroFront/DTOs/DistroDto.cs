using System.ComponentModel.DataAnnotations;

namespace DistroFront.DTOs;

public sealed class DistroDto
{
    public Guid DistroId { get; set; }

    [Required(ErrorMessage = "Informe a URL da imagem.")]
    public string? ImageUrl { get; set; }

    [Required(ErrorMessage = "Informe o nome.")]
    public string? Nome { get; set; }

    [Required(ErrorMessage = "Informe a descricao.")]
    public string? Descricao { get; set; }

    [Required(ErrorMessage = "Informe o ISO.")]
    public string? Iso { get; set; }

    [Required(ErrorMessage = "Selecione a categoria.")]
    public Guid CategoryId { get; set; }
}
