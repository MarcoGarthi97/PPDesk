using System;

namespace PPDesk.Abstraction.DTO.Service.Eventbrite
{  
    public class SrvEEvent
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string OrganizationId { get; set; }
        public DateTime Created { get; set; }
        public string Status { get; set; }
        public long Id { get; set; }
    }
}
