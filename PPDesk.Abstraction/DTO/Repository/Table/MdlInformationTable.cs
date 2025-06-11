using PPDesk.Abstraction.Enum;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PPDesk.Abstraction.DTO.Repository.Table
{
    [Table("Tables")]
    public class MdlInformationTable
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Column("EventName")]
        public string EventName { get; set; }

        [Column("EventStatus")]
        public EnumEventStatus EventStatus { get; set; }

        [Column("GdrName")]
        public string GdrName { get; set; }

        [Column("Capacity")]
        public short Capacity { get; set; }

        [Column("QuantitySold")]
        public short QuantitySold { get; set; }

        [Column("StartDate")]
        public DateTime StartDate { get; set; }

        [Column("EndDate")]
        public DateTime EndDate { get; set; }

        [Column("Master")]
        public string Master { get; set; }

        [Column("Type")]
        public EnumTableType TableType { get; set; }

        [Column("AllUsersPresence")]
        public bool? AllUsersPresence { get; set; }
    }
}
