using PPDesk.Abstraction.Enum;

namespace PPDesk.Abstraction.DTO.Service.PP
{
    public class SrvTable
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public string Gdr { get; set; }
        public EnumTableStatus Status { get; set; }
    }
}
