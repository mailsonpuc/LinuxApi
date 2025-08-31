
using LinuxApi.Models;
using LinuxApi.Pagination;

namespace LinuxApi.Repositories.Interfaces
{
    public interface IDistroRepository : IRepository<Distro>
    {
        //metodo especifico so pra essa interface
        // IEnumerable<Distro> GetDistros(DistrosParameters distrosParameters);

        Task<PagedList<Distro>> GetDistrosAsync(DistrosParameters distrosParameters);
        
        Task<PagedList<Distro>> GetDistrosFiltroNomeAsync(DistroFiltroNome distrosParams);

        
    }
}