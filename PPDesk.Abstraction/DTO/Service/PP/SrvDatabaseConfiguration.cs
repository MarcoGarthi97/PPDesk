using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPDesk.Abstraction.DTO.Service.PP
{
    public class SrvDatabaseConfiguration
    {
        public string Path { get; set; }
        public bool DatabaseExists { get; set; }
        public bool LoadFast {  get; set; }
    }
}
