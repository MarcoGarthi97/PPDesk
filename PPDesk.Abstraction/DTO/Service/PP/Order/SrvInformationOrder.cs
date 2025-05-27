using PPDesk.Abstraction.Enum;
using System;

namespace PPDesk.Abstraction.DTO.Service.PP.Order
{
    public class SrvInformationOrder
    {
        public int Id { get; set; }
        public long IdEventbride { get; set; }
        public string Name { get; set; }
        public DateTime DateOrder { get; set; }
        public int Quantity { get; set; }
        public string NameEvent { get; set; }
        public EnumEventStatus StatusEvent { get; set; }
        public string GdrName { get; set; }
        public string Master { get; set; }
    }
}
