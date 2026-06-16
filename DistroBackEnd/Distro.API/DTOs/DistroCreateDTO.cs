using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Distro.API.DTOs
{
    public class DistroCreateDTO
    {
        [Required]
        public string? Nome { get; set; }

        [Required]
        public string? Descricao { get; set; }

        [Required]
        public string? Iso { get; set; }

        [Required]
        public Guid CategoryId { get; set; }

        [Required]
        public IFormFile? ImageFile { get; set; }
    }
}
