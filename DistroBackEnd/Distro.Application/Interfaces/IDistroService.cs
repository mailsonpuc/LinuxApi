using Distro.Application.DTOs;

namespace Distro.Application.Interfaces
{
    public interface IDistroService
    {
        Task<IEnumerable<DistroDTO>> GetDistros();
        Task<DistroDTO> GetDistroById(Guid? id);
        Task<DistroDTO> CreateDistro(DistroDTO DistroDTO);
        Task<DistroDTO> UpdateDistro(DistroDTO DistroDTO);
        Task<bool> DeleteDistro(Guid? id);
    }
}