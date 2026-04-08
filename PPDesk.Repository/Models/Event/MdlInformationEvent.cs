using PPDesk.Abstraction.Enum;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PPDesk.Abstraction.DTO.Repository.Event
{
    [Table("EVENTS")]
    public class MdlInformationEvent
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }
        [Column("IdEventbride")]
        public long IdEventbride { get; set; }
        [Column("Name")]
        public string Name { get; set; }
        [Column("Start")]
        public DateTime Start { get; set; }
        [Column("End")]
        public DateTime End { get; set; }
        [Column("Status")]
        public EnumEventStatus Status { get; set; }
        [Column("TotalUsers")]
        public int TotalUsers { get; set; }
        [Column("TotalTicket")]
        public int TotalTicket { get; set; }
    }
}
