using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Distro.Application.DTOs;
using Distro.Application.Interfaces;
using Distro.Application.Mappings;
using Distro.Domain.Entities;
using Distro.Domain.Interfaces;

namespace Distro.Application.Services
{
    public class DistroService : IDistroService
    {
        private readonly IDistroRepository _distroRepository;

        public DistroService(IDistroRepository distroRepository)
        {
            _distroRepository = distroRepository;
        }

        public async Task<IEnumerable<DistroDTO>> GetDistros()
        {
            var distrosEntity = await _distroRepository.GetAllDistrosAsync();
            return distrosEntity.ToDto();
        }

        public async Task<DistroDTO> GetDistroById(Guid? id)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            var distroEntity = await _distroRepository.GetDistroByIdAsync(id.Value);

            if (distroEntity == null)
                return null;

            return distroEntity.ToDto();
        }

        public async Task<DistroDTO> CreateDistro(DistroDTO distroDTO)
        {
            var entity = distroDTO.ToEntity();

            await _distroRepository.AddDistroAsync(entity);

            return entity.ToDto();
        }


        public async Task<DistroDTO> UpdateDistro(DistroDTO distroDTO)
        {
            if (distroDTO == null)
                throw new ArgumentNullException(nameof(distroDTO));

            var distroEntity = await _distroRepository.GetDistroByIdAsync(distroDTO.DistroId);

            if (distroEntity == null)
                return null;

            distroEntity.Update(
                distroDTO.ImageUrl,
                distroDTO.Nome,
                distroDTO.Descricao,
                distroDTO.Iso,
                distroDTO.CategoryId
            );

            await _distroRepository.UpdateDistroAsync(distroEntity);

            return distroEntity.ToDto();
        }




        public async Task<bool> DeleteDistro(Guid? id)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            await _distroRepository.DeleteDistroAsync(id.Value);
            return true;
        }
    }
}
