

using System.Text.Json.Serialization;

namespace LinuxApi.Models
{
    public class Categoria
    {
        public Guid CategoriaId { get; set; }
        public string? Nome { get; set; }

        //categoria tem muitas distros
        [JsonIgnore] 
        public ICollection<Distro>? Distros { get; set; }
    }
}