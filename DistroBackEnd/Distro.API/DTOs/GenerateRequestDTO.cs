using System.ComponentModel.DataAnnotations;

namespace Distro.API.DTOs
{
    public class GenerateRequestDTO
    {
        [Required]
        public string? Model { get; set; } //usando o qwen2.5:3b

        [Required]
        public string? Prompt { get; set; }
    }
}
