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

        public IEnumerable<Distro> GetDistroPorCategoria(Guid id)
        {
            return _context.Distros
                .Include(d => d.Categoria) // carrega também a categoria
                .Where(d => d.CategoriaId == id)
                .ToList();
        }

        // public IEnumerable<Distro> GetDistros(DistrosParameters distrosParameters)
        // {
        //     return _context.Distros
        //         .Include(d => d.Categoria) //  trazer junto a categoria
        //         .OrderBy(p => p.Nome)
        //         .Skip((distrosParameters.PageNumber - 1) * distrosParameters.PageSize) 
        //         .Take(distrosParameters.PageSize)
        //         .ToList();
        // }
        public PagedList<Distro> GetDistros(DistrosParameters distrosParameters)
        {
            var distros = GetAll().OrderBy(d => d.DistroId).AsQueryable();
            var distrosOrdenados = PagedList<Distro>.ToPagedList(distros, distrosParameters.PageNumber, distrosParameters.PageSize);
            return distrosOrdenados;
        }
    }
}
