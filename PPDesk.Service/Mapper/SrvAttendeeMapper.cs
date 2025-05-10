using AutoMapper;
using PPDesk.Abstraction.DTO.Response.Eventbride.Order;
using PPDesk.Abstraction.DTO.Service.Eventbrite.Order;

namespace PPDesk.Service.Mapper
{
    public class SrvAttendeeMapper : Profile
    {
        public SrvAttendeeMapper()
        {
            CreateMap<EAttendeeResponse, SrvEAttendee>();
        }
    }
}
