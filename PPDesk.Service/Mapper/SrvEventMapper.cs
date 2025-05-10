using AutoMapper;
using PPDesk.Abstraction.DTO.Response.Eventbride.Event;
using PPDesk.Abstraction.DTO.Service.Eventbrite;
using System;

namespace PPDesk.Service.Mapper
{
    public class SrvEventMapper : Profile
    {
        public SrvEventMapper()
        {
            CreateMap<EEventResponse, SrvEEvent>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(from => Convert.ToInt64(from.Id)))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(from => from.Name.Text))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(from => from.Description.Text))
                .ForMember(dest => dest.Start, opt => opt.MapFrom(from => from.Start.Local))
                .ForMember(dest => dest.End, opt => opt.MapFrom(from => from.End.Local));

        }
    }
}
