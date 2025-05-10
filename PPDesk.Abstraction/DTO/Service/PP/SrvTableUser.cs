using PPDesk.Abstraction.Enum;

namespace PPDesk.Abstraction.DTO.Service.PP
{
    public class SrvTableUser
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int TableId { get; set; }
        public EnumTableUserTypeUser TypeUser { get; set; } 
    }
}
