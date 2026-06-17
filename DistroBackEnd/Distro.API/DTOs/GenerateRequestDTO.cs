using System.ComponentModel.DataAnnotations;

namespace Distro.API.DTOs
{
    public class GenerateRequestDTO
    {
        [Required]
        public string? Model { get; set; }

        [Required]
        public string? Prompt { get; set; }
    }
}
