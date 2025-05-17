using PPDesk.Abstraction.DTO.Service.Eventbrite.Order;
using PPDesk.Abstraction.Enum;
using System;

namespace PPDesk.Abstraction.DTO.Service.PP
{
    public class SrvEvent
    {
        public int Id { get; set; }
        public long IdEventbride { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public long OrganizationId { get; set; }
        public DateTime Created { get; set; }
        public EnumEventStatus Status { get; set; }
    }
}
