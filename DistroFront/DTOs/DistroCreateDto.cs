using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Components.Forms;

namespace DistroFront.DTOs;

public sealed class DistroCreateDto
{
    [Required(ErrorMessage = "Informe o nome.")]
    public string? Nome { get; set; }

    [Required(ErrorMessage = "Informe a descricao.")]
    public string? Descricao { get; set; }

    [Required(ErrorMessage = "Informe o ISO.")]
    public string? Iso { get; set; }

    [Required(ErrorMessage = "Selecione a categoria.")]
    public Guid CategoryId { get; set; }

    public IBrowserFile? ImageFile { get; set; }
}
