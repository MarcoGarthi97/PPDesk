using AutoMapper;
using PPDesk.Abstraction.DTO.Repository;
using PPDesk.Abstraction.DTO.Service.Eventbrite.Order;
using PPDesk.Abstraction.DTO.Service.PP;

namespace PPDesk.Service.Mapper
{
    public class SrvUserMapper : Profile
    {
        public SrvUserMapper()
        {
            CreateMap<SrvEProfile, SrvUser>();
            CreateMap<SrvUser, MdlUser>().ReverseMap();
        }
    }
}
