using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Distro.Domain.Interfaces;

public interface IDistroRepository
{
    Task<IEnumerable<Entities.Distro>> GetAllDistrosAsync();
    Task<Entities.Distro?> GetDistroByIdAsync(Guid distroId);
    Task<Entities.Distro?> GetDistroWithCategoryAsync(Guid distroId);

    Task AddDistroAsync(Entities.Distro distro);
    Task UpdateDistroAsync(Entities.Distro distro);
    Task DeleteDistroAsync(Guid distroId);
}
