using System;
using System.Collections.Generic;

namespace PPDesk.Abstraction.DTO.Service.Eventbrite.Order
{
    public class SrvEOrder
    {
        public string Id { get; set; }
        public DateTime Created { get; set; }
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Status { get; set; }
        public IEnumerable<SrvEAttendee> Attendees { get; set; }
    }
}
