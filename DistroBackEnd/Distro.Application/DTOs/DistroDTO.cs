using System.ComponentModel.DataAnnotations;

public class DistroDTO
{
    public Guid DistroId { get; set; }

    [Required]
    public string? ImageUrl { get; set; }

    [Required]
    public string? Nome { get; set; }

    [Required]
    public string? Descricao { get; set; }

    [Required]
    public string? Iso { get; set; }

    [Required]
    public Guid CategoryId { get; set; } 

  
}
