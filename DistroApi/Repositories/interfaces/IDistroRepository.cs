using DistroApi.Models;

namespace DistroApi.Repositories.interfaces
{
    public interface IDistroRepository : IRepository<Distro>
    {
        //  métodos específicos:
        Task<IEnumerable<Distro>> GetDistrosWithCategoriasAsync();
    }
}