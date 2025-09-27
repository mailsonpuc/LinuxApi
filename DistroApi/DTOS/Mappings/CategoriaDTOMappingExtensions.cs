

using DistroApi.Models;

namespace DistroApi.DTOS.Mappings
{
    public static class CategoriaDTOMappingExtensions
    {
        public static CategoriaDTO? ToCategoiaDTO(this Categoria categoria)
        {
            if (categoria is null)
            {
                return null;
            }

            return new CategoriaDTO
            {
                CategoriaId = categoria.CategoriaId,
                Nome = categoria.Nome,
                Distros = categoria.Distros
            };
        }




        public static Categoria? ToCategoria(this CategoriaDTO categoriaDto)
        {
            if (categoriaDto is null)
            {
                return null;
            }

            return new Categoria
            {
                CategoriaId = categoriaDto.CategoriaId,
                Nome = categoriaDto.Nome,
                Distros = categoriaDto.Distros
            };
        }




        public static IEnumerable<CategoriaDTO> ToCategoriaoDTOList(this IEnumerable<Categoria> categorias)
        {

            if (categorias is null || !categorias.Any())
            {
                return new List<CategoriaDTO>();
            }


            return categorias.Select(ag => new CategoriaDTO
            {
                CategoriaId = ag.CategoriaId,
                Nome = ag.Nome,
                Distros = ag.Distros
            }).ToList();

        }



    }
}