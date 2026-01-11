using System.ComponentModel.DataAnnotations;

namespace Distro.Application.DTOs
{
    public class CategoryDTO
    {
        public Guid CategoryId { get; set; }

        [Required(ErrorMessage = "The name is Required")]
        [MinLength(3, ErrorMessage = "The name must have at least 3 characters")]
        [MaxLength(50, ErrorMessage = "The name must have a maximum of 50 characters")]
        public string? Name { get; set; }

    }
}