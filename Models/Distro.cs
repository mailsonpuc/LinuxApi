

namespace LinuxApi.Models
{
    public class Distro
    {
        public Guid DistroId { get; set; }
        public string? ImageUrl { get; set; }
        public string? Nome { get; set; }
        public string? Descricao { get; set; }
        public string? Iso { get; set; }

        //explicitando forenkeyid
        public Guid CategoriaId { get; set; }
        public Categoria? Categoria { get; set; }
    }
}