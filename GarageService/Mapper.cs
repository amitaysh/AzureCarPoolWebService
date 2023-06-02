using AutoMapper;
using dm = GarageService.DomainModels;
using dto = GarageService.DTO;

namespace GarageService.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<dm.Request, dto.Request>();
        }
    }
}
