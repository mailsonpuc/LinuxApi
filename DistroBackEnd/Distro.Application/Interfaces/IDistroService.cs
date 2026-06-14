using Distro.Application.DTOs;
using Distro.Domain.Pagination;

namespace Distro.Application.Interfaces
{
    public interface IDistroService
    {
        Task<IEnumerable<DistroDTO>> GetDistros();
        Task<PagedList<DistroDTO>> GetDistrosPaged(int pageNumber = 1, int pageSize = 10);
        Task<DistroDTO> GetDistroById(Guid? id);
        Task<DistroDTO> CreateDistro(DistroDTO DistroDTO);
        Task<DistroDTO> UpdateDistro(DistroDTO DistroDTO);
        Task<bool> DeleteDistro(Guid? id);
    }
}