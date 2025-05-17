using AutoMapper;
using PPDesk.Abstraction.DTO.Repository;
using PPDesk.Abstraction.DTO.Response.Eventbride.Order;
using PPDesk.Abstraction.DTO.Service.Eventbrite.Order;
using PPDesk.Abstraction.DTO.Service.PP;
using System;

namespace PPDesk.Service.Mapper
{
    public class SrvOrderMapper : Profile
    {
        public SrvOrderMapper()
        {
            CreateMap<EOrderResponse, SrvEOrder>();

            CreateMap<SrvEAttendee, SrvOrder>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.IdEventbride, opt => opt.MapFrom(from => Convert.ToInt64(from.Id)))
                .ForMember(dest => dest.OrderIdEventbride, opt => opt.MapFrom(from => Convert.ToInt64(from.OrderId)))
                .ForMember(dest => dest.TableIdEventbride, opt => opt.MapFrom(from => Convert.ToInt64(from.TicketClassId)))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(from => from.Profile.Name));

            CreateMap<SrvOrder, MdlOrder>().ReverseMap();
        }
    }
}
