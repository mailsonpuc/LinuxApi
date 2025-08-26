using System.ComponentModel.DataAnnotations;

namespace LinuxApi.DTOS
{
    public class DistroDTO
    {
        public Guid DistroId { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [Url(ErrorMessage = "O campo {0} deve ser uma URL válida.")]
        [StringLength(700, MinimumLength = 10,
        ErrorMessage = "O campo {0} deve ter no mínimo {2} e no máximo {1} caracteres.")]
        public string? ImageUrl { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(100, MinimumLength = 3,
        ErrorMessage = "O campo {0} deve ter no mínimo {2} e no máximo {1} caracteres.")]
        public string? Nome { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(400, MinimumLength = 10,
        ErrorMessage = "O campo {0} deve ter no mínimo {2} e no máximo {1} caracteres.")]
        public string? Descricao { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [Url(ErrorMessage = "O campo {0} deve ser uma URL válida.")]
        [StringLength(400, MinimumLength = 10,
        ErrorMessage = "O campo {0} deve ter no mínimo {2} e no máximo {1} caracteres.")]
        public string? Iso { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public Guid CategoriaId { get; set; }
    }
}
