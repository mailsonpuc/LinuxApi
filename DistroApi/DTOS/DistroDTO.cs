using System.Text.Json.Serialization;
using DistroApi.Models;

namespace DistroApi.DTOS
{
    public class DistroDTO
    {
        public Guid DistroId { get; set; }
        public string? ImageUrl { get; set; }
        public string? Nome { get; set; }
        public string? Descricao { get; set; }
        public string? Iso { get; set; }

        // FK
        public Guid CategoriaId { get; set; }

        // Categoria (opcional: se quiser mostrar a categoria dentro da distro)
        [JsonIgnore] 
        public Categoria? Categoria { get; set; }
    }
}
