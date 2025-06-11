using PPDesk.Abstraction.Enum;
using System;

namespace PPDesk.Abstraction.DTO.Service.PP.Table
{
    public class SrvInformationTable
    {
        public int Id { get; set; }
        public string EventName { get; set; }
        public EnumEventStatus EventStatus { get; set; }
        public string GdrName { get; set; }
        public short Capacity { get; set; }
        public short QuantitySold { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Master { get; set; }
        public EnumTableType TableType { get; set; }
        public bool AllUsersPresence { get; set; }
        public string Position { get; set; }
    }
}
