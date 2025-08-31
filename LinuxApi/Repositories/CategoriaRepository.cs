using LinuxApi.Context;
using LinuxApi.Models;
using LinuxApi.Pagination;
using LinuxApi.Repositories.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace LinuxApi.Repositories
{
    public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
    {
        public CategoriaRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<PagedList<Categoria>> GetCategoriasAsync(CategoriaParameters categoriaParameters)
        {
            // Obtém todos os registros de Categoria
            var categorias = await GetAllAsync();

            // Ordena por CategoriaId
            var categoriasOrdenadas = categorias.OrderBy(p => p.CategoriaId).AsQueryable();

            // Retorna a paginação
            return PagedList<Categoria>.ToPagedList(
                categoriasOrdenadas,
                categoriaParameters.PageNumber,
                categoriaParameters.PageSize);
        }
    }
}
