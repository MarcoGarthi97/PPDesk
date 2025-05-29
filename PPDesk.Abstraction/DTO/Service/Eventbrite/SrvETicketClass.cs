using System;

namespace PPDesk.Abstraction.DTO.Service.Eventbrite
{
    public class SrvETicketClass
    {
        public string DisplayName { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string EventId { get; set; }
        public string Id { get; set; }
        public short Capacity { get; set; }
        public short QuantitySold { get; set; }
        public DateTime SalesEnd { get; set; }
    }

}
