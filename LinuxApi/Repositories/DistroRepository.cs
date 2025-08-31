using Microsoft.EntityFrameworkCore;
using LinuxApi.Context;
using LinuxApi.Models;
using LinuxApi.Repositories.Interfaces;
using LinuxApi.Pagination;

namespace LinuxApi.Repositories
{
    public class DistroRepository : Repository<Distro>, IDistroRepository
    {
        public DistroRepository(AppDbContext context) : base(context)
        {
        }

        // public IEnumerable<Distro> GetDistros(DistrosParameters distrosParams)
        // {
        //     return GetAll()
        //         .OrderBy(p => p.Nome)
        //         .Skip((distrosParams.PageNumber - 1) * distrosParams.PageSize)
        //         .Take(distrosParams.PageSize)
        //         .ToList();
        // }

        public PagedList<Distro> GetDistros(DistrosParameters distrosParams)
        {
            var distros = GetAll().OrderBy(p => p.DistroId).AsQueryable();
            var distrosOrdenadas = PagedList<Distro>.ToPagedList(distros,
                        distrosParams.PageNumber, distrosParams.PageSize);
            return distrosOrdenadas;
        }




        public IEnumerable<Distro> GetDistroPorCategoria(Guid id)
        {
            throw new NotImplementedException();
        }


    }
}
