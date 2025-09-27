using DistroApi.Context;
using DistroApi.Models;
using DistroApi.Repositories.interfaces;
using Microsoft.EntityFrameworkCore;

namespace DistroApi.Repositories
{
    public class DistroRepository : Repository<Distro>, IDistroRepository
    {
        public DistroRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Distro>> GetDistrosWithCategoriasAsync()
        {
            return await _context.Distros
                .Include(d => d.Categoria)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
