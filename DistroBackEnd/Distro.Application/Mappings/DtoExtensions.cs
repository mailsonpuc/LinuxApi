using System;
using System.Collections.Generic;
using System.Linq;
using Distro.Application.DTOs;
using Distro.Domain.Entities;
using DomainDistro = Distro.Domain.Entities.Distro;

namespace Distro.Application.Mappings
{
    public static class DtoExtensions
    {
        public static CategoryDTO ToDto(this Category entity)
        {
            if (entity == null) return null;

            return new CategoryDTO
            {
                CategoryId = entity.CategoryId,
                Name = entity.Name
            };
        }

        public static Category ToEntity(this CategoryDTO dto)
        {
            if (dto == null) return null;

            return dto.CategoryId == Guid.Empty
                ? new Category(dto.Name)
                : new Category(dto.CategoryId, dto.Name);
        }

        public static IEnumerable<CategoryDTO> ToDto(this IEnumerable<Category> entities)
        {
            if (entities == null) return Enumerable.Empty<CategoryDTO>();
            return entities.Select(x => x.ToDto());
        }

        public static DistroDTO ToDto(this DomainDistro entity)
        {
            if (entity == null) return null;

            return new DistroDTO
            {
                DistroId = entity.DistroId,
                ImageUrl = entity.ImageUrl,
                Nome = entity.Nome,
                Descricao = entity.Descricao,
                Iso = entity.Iso,
                CategoryId = entity.CategoryId
            };
        }

        public static DomainDistro ToEntity(this DistroDTO dto)
        {
            if (dto == null) return null;

            return dto.DistroId == Guid.Empty
                ? new DomainDistro(dto.ImageUrl, dto.Nome, dto.Descricao, dto.Iso, dto.CategoryId)
                : new DomainDistro(dto.DistroId, dto.ImageUrl, dto.Nome, dto.Descricao, dto.Iso, dto.CategoryId);
        }

        public static IEnumerable<DistroDTO> ToDto(this IEnumerable<DomainDistro> entities)
        {
            if (entities == null) return Enumerable.Empty<DistroDTO>();
            return entities.Select(x => x.ToDto());
        }
    }
}
