

using LinuxApi.Context;
using LinuxApi.Models;
using LinuxApi.Repositories.Interfaces;

namespace LinuxApi.Repositories
{
    public class DistroRepository : Repository<Distro>, IDistroRepository
    {
        //se precisa de contexto busca na class base, repository
        public DistroRepository(AppDbContext context) : base(context)
        {

        }


        public IEnumerable<Distro> GetDistroPorCategoria(Guid id)
        {
            return GetAll().Where(c => c.CategoriaId == id);
        }
        
    }
}