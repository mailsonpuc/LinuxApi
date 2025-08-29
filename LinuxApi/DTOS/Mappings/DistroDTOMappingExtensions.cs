using LinuxApi.Models;

namespace LinuxApi.DTOS.Mappings
{
    public static class DistroDTOMappingExtensions
    {
        public static DistroDTO? ToDistroDTO(this Distro distro)
        {
            if (distro is null)
                return null;

            return new DistroDTO
            {
                DistroId = distro.DistroId,
                ImageUrl = distro.ImageUrl,
                Nome = distro.Nome,
                Descricao = distro.Descricao,
                Iso = distro.Iso,
                CategoriaId = distro.CategoriaId
            };
        }

        public static Distro? ToDistro(this DistroDTO distroDto)
        {
            if (distroDto is null) return null;

            return new Distro
            {
                DistroId = distroDto.DistroId,
                ImageUrl = distroDto.ImageUrl,
                Nome = distroDto.Nome,
                Descricao = distroDto.Descricao,
                Iso = distroDto.Iso,
                CategoriaId = distroDto.CategoriaId
            };
        }

        public static IEnumerable<DistroDTO> ToDistroDTOList(this IEnumerable<Distro> distros)
        {
            if (distros is null || !distros.Any())
            {
                return new List<DistroDTO>();
            }

            return distros.Select(distro => new DistroDTO
            {
                DistroId = distro.DistroId,
                ImageUrl = distro.ImageUrl,
                Nome = distro.Nome,
                Descricao = distro.Descricao,
                Iso = distro.Iso,
                CategoriaId = distro.CategoriaId
            }).ToList();
        }
    }
}
