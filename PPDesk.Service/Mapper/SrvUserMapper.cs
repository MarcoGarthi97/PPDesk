using AutoMapper;
using PPDesk.Abstraction.DTO.Repository.User;
using PPDesk.Abstraction.DTO.Service.Eventbrite.Order;
using PPDesk.Abstraction.DTO.Service.PP.User;

namespace PPDesk.Service.Mapper
{
    public class SrvUserMapper : Profile
    {
        public SrvUserMapper()
        {
            CreateMap<SrvEProfile, SrvUser>();
            CreateMap<SrvUser, MdlUser>().ReverseMap();
            CreateMap<SrvInformationUser, MdlInformationUser>().ReverseMap();
        }
    }
}
