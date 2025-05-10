using AutoMapper;
using PPDesk.Abstraction.DTO.Response.Eventbride;
using PPDesk.Abstraction.DTO.Service.Eventbrite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPDesk.Service.Mapper
{
    public class SrvOrganizationMapper : Profile
    {
        public SrvOrganizationMapper()
        {
            CreateMap<EOrganizationResponse, SrvEOrganization>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(from => Convert.ToInt64(from.Id)));
        }
    }
}
