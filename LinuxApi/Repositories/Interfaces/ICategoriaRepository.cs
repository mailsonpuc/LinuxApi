

using LinuxApi.Models;
using LinuxApi.Pagination;

namespace LinuxApi.Repositories.Interfaces
{
    public interface ICategoriaRepository : IRepository<Categoria>
    {
        
        PagedList<Categoria> GetCategorias(CategoriaParameters categoriaParameters);
    }
}