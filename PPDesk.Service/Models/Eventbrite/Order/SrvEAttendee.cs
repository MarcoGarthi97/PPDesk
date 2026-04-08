using System;

namespace PPDesk.Abstraction.DTO.Service.Eventbrite.Order
{
    public class SrvEAttendee
    {
        public string Id { get; set; }
        public DateTime Created { get; set; }
        public int Quantity { get; set; }
        public SrvEProfile Profile { get; set; }
        public bool Cancelled { get; set; }
        public string EventId { get; set; }
        public string OrderId { get; set; }
        public string TicketClassId { get; set; }
    }
}
