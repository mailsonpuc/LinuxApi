using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Distro.Domain.Interfaces;
using Distro.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Distro.Infra.Data.Repositories;

public class DistroRepository : IDistroRepository
{
    private readonly ApplicationDbContext _context;

    public DistroRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddDistroAsync(Domain.Entities.Distro distro)
    {
        _context.Distros.Add(distro);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteDistroAsync(Guid distroId)
    {
        var distro = await _context.Distros.FindAsync(distroId);
        if (distro is null) return;

        _context.Distros.Remove(distro);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Domain.Entities.Distro>> GetAllDistrosAsync()
    {
        return await _context.Distros.ToListAsync();
    }

    public async Task<Domain.Entities.Distro?> GetDistroByIdAsync(Guid distroId)
    {
        return await _context.Distros.FindAsync(distroId);
    }

    public async Task<Domain.Entities.Distro?> GetDistroWithCategoryAsync(Guid distroId)
    {
        return await _context.Distros
            .Include(d => d.Category)
            .FirstOrDefaultAsync(d => d.DistroId == distroId);
    }

    public async Task UpdateDistroAsync(Domain.Entities.Distro distro)
    {
        _context.Distros.Update(distro);
        await _context.SaveChangesAsync();
    }
}
