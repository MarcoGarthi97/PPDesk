using PPDesk.Abstraction.Enum;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PPDesk.Abstraction.DTO.Repository.Table
{
    [Table("Tables")]
    public class MdlTable
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Column("EventIdEventbride")]
        public long EventIdEventbride { get; set; }

        [Column("IdEventbride")]
        public long IdEventbride { get; set; }

        [Column("GdrName")]
        public string GdrName { get; set; }

        [Column("Description")]
        public string Description { get; set; }

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

        [Column("Status")]
        public EnumTableStatus Status { get; set; }

        [Column("Type")]
        public EnumTableType Type { get; set; }

        [Column("AllUsersPresence")]
        public bool? AllUsersPresence { get; set; }
    }
}
