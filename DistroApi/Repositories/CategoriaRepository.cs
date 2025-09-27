

using DistroApi.Context;
using DistroApi.Models;
using DistroApi.Repositories.interfaces;

namespace DistroApi.Repositories
{
    public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
    {
        public CategoriaRepository(AppDbContext context) : base(context)
        {

        }
    }
}