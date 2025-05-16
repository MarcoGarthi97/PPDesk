using AutoMapper;
using PPDesk.Abstraction.DTO.Response.Eventbride.TicketClass;
using PPDesk.Abstraction.DTO.Service.Eventbrite;
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
}
