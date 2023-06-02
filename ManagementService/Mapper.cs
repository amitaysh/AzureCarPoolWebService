using AutoMapper;
using dm = ManagementService.DomainModels;
using dto = ManagementService.DTO;

namespace TransactionsService.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<dm.Car, dto.Car>();
            CreateMap<dto.CreateCar, dm.Car>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()));
            CreateMap<dm.User, dto.User>();
            CreateMap<dto.CreateUser, dm.User>()
                 .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()));
        }
    }
}
