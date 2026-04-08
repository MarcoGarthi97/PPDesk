using System;

namespace PPDesk.Abstraction.DTO.Service.PP.Order
{
    public class SrvOrder
    {
        public int Id { get; set; }
        public long IdEventbride { get; set; }
        public long EventIdEventbride { get; set; }
        public long OrderIdEventbride { get; set; }
        public long TableIdEventbride { get; set; }
        public string Name { get; set; }
        public DateTime Created { get; set; }
        public int Quantity { get; set; }
        public bool Cancelled { get; set; }
        public bool UserPresence { get; set; }
    }
}
