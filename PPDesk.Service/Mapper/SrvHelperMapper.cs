using AutoMapper;
using PPDesk.Abstraction.DTO.Repository;
using PPDesk.Abstraction.DTO.Service.PP;

namespace PPDesk.Service.Mapper
{
    public class SrvHelperMapper : Profile
    {
        public SrvHelperMapper()
        {
            CreateMap<SrvHelper, MdlHelper>().ReverseMap();
        }
    }
}
