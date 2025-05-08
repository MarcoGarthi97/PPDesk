using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PPDesk.Abstraction.DTO.Repository
{
    [Table("Events")]
    public class MdlEvent
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }
        [Column("Name")]
        public string Name { get; set; }
    }
}
