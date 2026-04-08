using PPDesk.Abstraction.Enum;
using System;

namespace PPDesk.Abstraction.DTO.Service.PP.Event
{
    public class SrvInformationEvent
    {
        public int Id { get; set; }
        public long IdEventbride { get; set; }
        public string Name { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public int TotalUsers { get; set; }
        public int TotalTicket { get; set; }
        public EnumEventStatus Status { get; set; }
    }
}
