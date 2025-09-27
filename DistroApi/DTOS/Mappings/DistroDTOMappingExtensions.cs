using DistroApi.Models;

namespace DistroApi.DTOS.Mappings
{
    public static class DistroDTOMappingExtensions
    {
        public static DistroDTO? ToDistroDTO(this Distro distro)
        {
            if (distro is null) return null;

            return new DistroDTO
            {
                DistroId = distro.DistroId,
                ImageUrl = distro.ImageUrl,
                Nome = distro.Nome,
                Descricao = distro.Descricao,
                Iso = distro.Iso,
                CategoriaId = distro.CategoriaId,
                Categoria = distro.Categoria
            };
        }

        public static Distro? ToDistro(this DistroDTO dto)
        {
            if (dto is null) return null;

            return new Distro
            {
                DistroId = dto.DistroId,
                ImageUrl = dto.ImageUrl,
                Nome = dto.Nome,
                Descricao = dto.Descricao,
                Iso = dto.Iso,
                CategoriaId = dto.CategoriaId,
                Categoria = dto.Categoria
            };
        }

        public static IEnumerable<DistroDTO> ToDistroDTOList(this IEnumerable<Distro> distros)
        {
            if (distros is null || !distros.Any())
                return new List<DistroDTO>();

            return distros.Select(d => new DistroDTO
            {
                DistroId = d.DistroId,
                ImageUrl = d.ImageUrl,
                Nome = d.Nome,
                Descricao = d.Descricao,
                Iso = d.Iso,
                CategoriaId = d.CategoriaId,
                Categoria = d.Categoria
            }).ToList();
        }
    }
}
