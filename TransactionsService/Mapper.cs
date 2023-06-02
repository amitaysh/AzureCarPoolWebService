using AutoMapper;
using dm = TransactionsService.DomainModels;
using dto = TransactionsService.DTO;

namespace TransactionsService.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<dm.Transaction, dto.Transaction>();
            CreateMap<dto.CreateTransaction, dm.Transaction>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()));
        }
    }
}
