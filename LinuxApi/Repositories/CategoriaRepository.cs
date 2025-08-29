

using LinuxApi.Context;
using LinuxApi.Models;
using LinuxApi.Repositories.Interfaces;

namespace LinuxApi.Repositories
{
    public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
    {
        //se precisa de contexto busca na class base, repository
        public CategoriaRepository(AppDbContext context) : base(context)
        {

        }
    }
}