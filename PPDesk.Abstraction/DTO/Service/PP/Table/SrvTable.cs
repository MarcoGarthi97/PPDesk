using PPDesk.Abstraction.Enum;
using System;

namespace PPDesk.Abstraction.DTO.Service.PP.Table
{
    public class SrvTable
    {
        public int Id { get; set; }
        public long IdEventbride { get; set; }
        public long EventIdEventbride { get; set; }
        public string GdrName { get; set; }
        public string Description { get; set; }
        public short Capacity { get; set; }
        public short QuantitySold { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Master {  get; set; }
        public EnumTableStatus Status { get; set; }
        public EnumTableType Type { get; set; }
    }
}
