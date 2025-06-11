using PPDesk.Abstraction.Enum;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PPDesk.Abstraction.DTO.Repository.Order
{
    [Table("ORDERS")]
    public class MdlInformationOrder
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }
        [Column("IdEventbride")]
        public long IdEventbride { get; set; }
        [Column("TableIdEventbride")]
        public long TableIdEventbride { get; set; }
        [Column("Name")]
        public string Name { get; set; }
        [Column("DateOrder")]
        public DateTime DateOrder { get; set; }
        [Column("Quantity")]
        public int Quantity { get; set; }
        [Column("EventName")]
        public string EventName { get; set; }
        [Column("TypeTable")]
        public EnumTableType TypeTable { get; set; }
        [Column("StatusEvent")]
        public EnumEventStatus StatusEvent { get; set; }
        [Column("GdrName")]
        public string GdrName { get; set; }
        [Column("Master")]
        public string Master { get; set; }
        [Column("UserPresence")]
        public bool? UserPresence { get; set; }
    }
}
