using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace PPDesk.Abstraction.DTO.Repository
{
    [Table("Version")]
    public class MdlVersion
    {
        [Column("Version")]
        public string Version { get; set; }
        [Column("Rud")]
        public DateTime Rud {  get; set; }
    }
}
