using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PPDesk.Abstraction.DTO.Repository
{
    [Table("HELPERS")]
    public class MdlHelper
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }
        [Column("Key")]
        public string Key { get; set; }
        [Column("Json")]
        public string Json { get; set; }
    }
}
