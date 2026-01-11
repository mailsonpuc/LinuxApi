using System.ComponentModel.DataAnnotations;
using Distro.Domain.Entities;

namespace Distro.Application.DTOs
{
    public class DistroDTO
    {
        public Guid DistroId { get; set; }

        [Required(ErrorMessage = "The image URL is Required")]
        public string? ImageUrl { get; set; }

        [Required(ErrorMessage = "The name is Required")]
        [MinLength(3, ErrorMessage = "The name must have at least 3 characters")]
        [MaxLength(50, ErrorMessage = "The name must have a maximum of 50 characters")]
        public string? Nome { get; set; }

        [Required(ErrorMessage = "The description is Required")]
        [MinLength(10, ErrorMessage = "The description must have at least 10 characters")]
        [MaxLength(500, ErrorMessage = "The description must have a maximum of 500 characters")]
        public string? Descricao { get; set; }

        [Required(ErrorMessage = "The ISO is Required")]
        public string? Iso { get; set; }


        public Guid CategoryId { get; private set; }
        public Category? Category { get; private set; }
    }
}