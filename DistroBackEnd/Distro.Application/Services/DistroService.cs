using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Distro.Application.DTOs;
using Distro.Application.Interfaces;
using Distro.Domain.Entities;
using Distro.Domain.Interfaces;

namespace Distro.Application.Services
{
    public class DistroService : IDistroService
    {
        private readonly IDistroRepository _distroRepository;
        private readonly IMapper _mapper;

        public DistroService(
            IDistroRepository distroRepository,
            IMapper mapper)
        {
            _distroRepository = distroRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<DistroDTO>> GetDistros()
        {
            var distrosEntity = await _distroRepository.GetAllDistrosAsync();
            return _mapper.Map<IEnumerable<DistroDTO>>(distrosEntity);
        }

        public async Task<DistroDTO> GetDistroById(Guid? id)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            var distroEntity = await _distroRepository.GetDistroByIdAsync(id.Value);

            if (distroEntity == null)
                return null;

            return _mapper.Map<DistroDTO>(distroEntity);
        }

        public async Task<DistroDTO> CreateDistro(DistroDTO distroDTO)
        {
            var entity = new Domain.Entities.Distro(
                imageUrl: distroDTO.ImageUrl,
                nome: distroDTO.Nome,
                descricao: distroDTO.Descricao,
                iso: distroDTO.Iso,
                categoryId: distroDTO.CategoryId
            );

            await _distroRepository.AddDistroAsync(entity);

            return _mapper.Map<DistroDTO>(entity);
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

            return _mapper.Map<DistroDTO>(distroEntity);
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
