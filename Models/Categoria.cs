

namespace LinuxApi.Models
{
    public class Categoria
    {
        public Guid CategoriaId { get; set; }
        public string? Nome { get; set; }

        //categoria tem muitas distros
        public ICollection<Distro>? Distros { get; set; }
    }
}