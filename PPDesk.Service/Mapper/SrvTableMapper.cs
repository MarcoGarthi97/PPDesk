using AutoMapper;
using PPDesk.Abstraction.DTO.Repository.Table;
using PPDesk.Abstraction.DTO.Service.Eventbrite;
using PPDesk.Abstraction.DTO.Service.PP.Table;

namespace PPDesk.Service.Mapper
{
    public class SrvTableMapper : Profile
    {
        public SrvTableMapper()
        {
            CreateMap<SrvTable, MdlTable>().ReverseMap();

            CreateMap<SrvInformationTable, MdlInformationTable>();

            CreateMap<MdlInformationTable, SrvInformationTable>();

            CreateMap<SrvETicketClass, SrvTable>()
                .ForMember(dest => dest.IdEventbride, opt => opt.MapFrom(from => from.Id))
                .ForMember(dest => dest.Id, opt => opt.Ignore());
        }
    }
}
