using PPDesk.Abstraction.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PPDesk.Abstraction.DTO.Repository
{
    [Table("Tables")]
    public class MdlTable
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }
        [Column("eventId")]
        public int EventId { get; set; }
        [Column("Gdr")]
        public string Gdr { get; set; }
        [Column("Status")]
        public EnumTableStatus Status { get; set; }
    }
}
