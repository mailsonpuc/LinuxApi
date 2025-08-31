

using LinuxApi.Models;
using LinuxApi.Pagination;

namespace LinuxApi.Repositories.Interfaces
{
    public interface ICategoriaRepository : IRepository<Categoria>
    {
        
        Task<PagedList<Categoria>> GetCategoriasAsync(CategoriaParameters categoriaParameters);
    }
}