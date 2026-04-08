using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PPDesk.Abstraction.DTO.Repository.Order
{
    [Table("ORDERS")]
    public class MdlOrder
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }
        [Column("IdEventbride")]
        public long IdEventbride { get; set; }
        [Column("EventIdEventbride")]
        public long EventIdEventbride { get; set; }
        [Column("OrderIdEventbride")]
        public long OrderIdEventbride { get; set; }
        [Column("TableIdEventbride")]
        public long TableIdEventbride { get; set; }
        [Column("Name")]
        public string Name { get; set; }
        [Column("Created")]
        public DateTime Created { get; set; }
        [Column("Quantity")]
        public int Quantity { get; set; }
        [Column("Cancelled")]
        public bool Cancelled { get; set; }
        [Column("UserPresence")]
        public bool? UserPresence { get; set; }
    }
}
