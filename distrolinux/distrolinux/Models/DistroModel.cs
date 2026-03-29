namespace Distrolinux.Models
{
    public class DistroModel
    {
        public Guid DistroId { get; set; }
        public string? ImageUrl { get; set; }
        public string? Nome { get; set; }
        public string? Descricao { get; set; }
        public string? Iso { get; set; }
        public Guid CategoryId { get; set; }
    }
}