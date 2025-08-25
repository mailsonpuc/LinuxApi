
using LinuxApi.Models;

namespace LinuxApi.Repositories.Interfaces
{
    public interface IDistroRepository : IRepository<Distro>
    {
        //metodo especifico so pra essa interface
        IEnumerable<Distro> GetDistroPorCategoria(Guid id);
        
    }
}