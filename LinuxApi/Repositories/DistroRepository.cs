using Microsoft.EntityFrameworkCore;
using LinuxApi.Context;
using LinuxApi.Models;
using LinuxApi.Repositories.Interfaces;
using LinuxApi.Pagination;
using System.Linq;
using System.Threading.Tasks;

namespace LinuxApi.Repositories
{
    public class DistroRepository : Repository<Distro>, IDistroRepository
    {
        public DistroRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<PagedList<Distro>> GetDistrosAsync(DistrosParameters distrosParams)
        {
            // Obtem as distros de forma assíncrona
            var distros = await GetAllAsync();

            // Ordena
            var distrosOrdenadas = distros.OrderBy(p => p.DistroId).AsQueryable();

            // Paginacao
            return PagedList<Distro>.ToPagedList(
                distrosOrdenadas,
                distrosParams.PageNumber,
                distrosParams.PageSize);
        }

        public async Task<PagedList<Distro>> GetDistrosFiltroNomeAsync(DistroFiltroNome distrosParams)
        {
            // Obtem todas as distros de forma assíncrona
            var distros = await GetAllAsync();

            // Converte para IQueryable
            var distrosQueryable = distros.AsQueryable();

            // Filtra se necessário
            if (!string.IsNullOrEmpty(distrosParams.Nome))
            {
                distrosQueryable = distrosQueryable.Where(c => c.Nome.Contains(distrosParams.Nome));
            }

            // Paginacao
            return PagedList<Distro>.ToPagedList(
                distrosQueryable,
                distrosParams.PageNumber,
                distrosParams.PageSize);
        }
    }
}
