using System.ComponentModel.DataAnnotations;

namespace LinuxApi.DTOS
{
    public class CategoriaDTO
    {
        public Guid CategoriaId { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(100,
        MinimumLength = 3,
        ErrorMessage = "O campo {0} deve ter no mínimo {2} e no máximo {1} caracteres.")]
        public string? Nome { get; set; }
    }
}
