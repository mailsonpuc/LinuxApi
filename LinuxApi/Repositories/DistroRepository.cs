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

      

        public PagedList<Distro> GetDistros(DistrosParameters distrosParams)
        {
            var distros = GetAll().OrderBy(p => p.DistroId).AsQueryable();
            var distrosOrdenadas = PagedList<Distro>.ToPagedList(distros,
                        distrosParams.PageNumber, distrosParams.PageSize);
            return distrosOrdenadas;
        }

        public PagedList<Distro> GetDistrosFiltroNome(DistroFiltroNome distrosParams)
        {
            var distros = GetAll().AsQueryable();
            if (!string.IsNullOrEmpty(distrosParams.Nome))
            {
                distros = distros.Where(c => c.Nome.Contains(distrosParams.Nome));
            }
            var distrosFiltradas = PagedList<Distro>.ToPagedList(distros,
            distrosParams.PageNumber, distrosParams.PageSize);

            return distrosFiltradas;
        }
    }
}
