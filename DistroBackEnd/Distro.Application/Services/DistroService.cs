using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Distro.Application.DTOs;
using Distro.Application.Interfaces;
using Distro.Application.Mappings;
using Distro.Domain.Entities;
using Distro.Domain.Interfaces;
using Distro.Domain.Pagination;
using DistroEntity = Distro.Domain.Entities.Distro;

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

        public async Task<PagedList<DistroDTO>> GetDistrosPaged(int pageNumber = 1, int pageSize = 10)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;

            var query = _distroRepository.GetAllDistrosAsQueryable();
            var pagedList = await PagedList<DistroEntity>.ToPagedListAsync(query, pageNumber, pageSize);
            
            return new PagedList<DistroDTO>(
                pagedList.Select(d => d.ToDto()).ToList(),
                pagedList.TotalCount,
                pagedList.CurrentPage,
                pagedList.PageSize
            );
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

        public async Task<IEnumerable<DistroDTO>> FindDistrosByName(string nome)
        {
            var distros = await _distroRepository.FindDistrosByNameAsync(nome);
            return distros.ToDto();
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
