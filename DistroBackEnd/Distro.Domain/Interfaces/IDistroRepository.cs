using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Distro.Domain.Interfaces;

public interface IDistroRepository
{
    Task<IEnumerable<Entities.Distro>> GetAllDistrosAsync();
    IQueryable<Entities.Distro> GetAllDistrosAsQueryable();
    Task<Entities.Distro?> GetDistroByIdAsync(Guid distroId);
    Task<Entities.Distro?> GetDistroWithCategoryAsync(Guid distroId);

    Task AddDistroAsync(Entities.Distro distro);
    Task UpdateDistroAsync(Entities.Distro distro);
    Task DeleteDistroAsync(Guid distroId);
    Task<IEnumerable<Entities.Distro>> FindDistrosByNameAsync(string nome);
}
