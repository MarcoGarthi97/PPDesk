using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PPDesk.Abstraction.DTO.Repository.User
{
    [Table("USERS")]
    public class MdlInformationUser
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }
        [Column("FirstName")]
        public string FirstName { get; set; }
        [Column("LastName")]
        public string LastName { get; set; }
        [Column("CellPhone")]
        public string CellPhone { get; set; }
        [Column("Email")]
        public string Email { get; set; }
        [Column("Name")]
        public string Name { get; set; }
        [Column("EventsQuantity")]
        public int EventsQuantity { get; set; }
        [Column("OrdersQuantity")]
        public int OrdersQuantity { get; set; }
    }
}
