using PPDesk.Abstraction.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PPDesk.Abstraction.DTO.Repository
{
    [Table("TableUsers")]
    public class MdlTableUser
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }
        [Column("UserId")]
        public int UserId { get; set; }
        [Column("TableId")]
        public int TableId { get; set; }
        [Column("TypeUser")]
        public EnumTableUserTypeUser TypeUser { get; set; }
    }
}
