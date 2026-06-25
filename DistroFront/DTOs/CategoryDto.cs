using System.ComponentModel.DataAnnotations;

namespace DistroFront.DTOs;

public sealed class CategoryDto
{
    public Guid CategoryId { get; set; }

    [Required(ErrorMessage = "Informe o nome.")]
    [MinLength(3, ErrorMessage = "Use pelo menos 3 caracteres.")]
    [MaxLength(50, ErrorMessage = "Use no maximo 50 caracteres.")]
    public string? Name { get; set; }
}
