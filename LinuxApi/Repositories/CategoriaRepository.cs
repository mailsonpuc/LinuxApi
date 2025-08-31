

using LinuxApi.Context;
using LinuxApi.Models;
using LinuxApi.Pagination;
using LinuxApi.Repositories.Interfaces;

namespace LinuxApi.Repositories
{
    public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
    {
        //se precisa de contexto busca na class base, repository
        public CategoriaRepository(AppDbContext context) : base(context)
        {

        }

        public PagedList<Categoria> GetCategorias(CategoriaParameters categoriaParameters)
        {
            var categorias = GetAll().OrderBy(p => p.CategoriaId).AsQueryable();
            var categoriasOrdenadas = PagedList<Categoria>.ToPagedList(categorias,
                        categoriaParameters.PageNumber, categoriaParameters.PageSize);
            return categoriasOrdenadas;
        }



    }
}