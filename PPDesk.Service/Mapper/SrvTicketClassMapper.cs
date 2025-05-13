using AutoMapper;
using PPDesk.Abstraction.DTO.Response.Eventbride.TicketClass;
using PPDesk.Abstraction.DTO.Service.Eventbrite;
using PPDesk.Abstraction.DTO.Service.PP;
using System;

namespace PPDesk.Service.Mapper
{
    public class SrvTicketClassMapper : Profile
    {
        public SrvTicketClassMapper()
        {
            CreateMap<ETicketClassResponse, SrvETicketClass>();
        }
    }

    public class SrvTableMapper : Profile
    {
        public SrvTableMapper()
        {
            CreateMap<SrvETicketClass, SrvTable>()
                .ForMember(dest => dest.IdEventbride, opt => opt.MapFrom(from => from.Id))
                .ForMember(dest => dest.Id, opt => opt.Ignore());
        }
    }
}
