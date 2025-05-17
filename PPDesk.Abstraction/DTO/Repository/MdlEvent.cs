using PPDesk.Abstraction.Enum;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PPDesk.Abstraction.DTO.Repository
{
    [Table("EVENTS")]
    public class MdlEvent
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }
        [Column("IdEventbride")]
        public long IdEventbride { get; set; }
        [Column("Name")]
        public string Name { get; set; }
        [Column("Description")]
        public string Description { get; set; }
        [Column("Start")]
        public DateTime Start { get; set; }
        [Column("End")]
        public DateTime End { get; set; }
        [Column("OrganizationId")]
        public long OrganizationId { get; set; }
        [Column("Created")]
        public DateTime Created { get; set; }
        [Column("Status")]
        public EnumEventStatus Status { get; set; }
    }
}
