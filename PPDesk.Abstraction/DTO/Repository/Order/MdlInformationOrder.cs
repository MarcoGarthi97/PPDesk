using PPDesk.Abstraction.Enum;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PPDesk.Abstraction.DTO.Repository.Order
{
    [Table("ORDES")]
    public class MdlInformationOrder
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }
        [Column("IdEventbride")]
        public long IdEventbride { get; set; }
        [Column("Name")]
        public string Name { get; set; }
        [Column("DateOrder")]
        public DateTime DateOrder { get; set; }
        [Column("Quantity")]
        public int Quantity { get; set; }
        [Column("NameEvent")]
        public string NameEvent { get; set; }
        [Column("StatusEvent")]
        public EnumEventStatus StatusEvent { get; set; }
        [Column("GdrName")]
        public string GdrName { get; set; }
        [Column("Master")]
        public string Master { get; set; }
    }
}
