using AutoMapper;
using Distro.Application.DTOs;

namespace Distro.Application.Mappings
{
    public class DomainToDTOMappingProfile : Profile
    {
        public DomainToDTOMappingProfile()
        {
            CreateMap<Distro.Domain.Entities.Distro, DistroDTO>();
            CreateMap<Distro.Domain.Entities.Category, CategoryDTO>();
        }
    }
}