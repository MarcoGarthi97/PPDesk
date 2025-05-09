using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPDesk.Abstraction.DTO.Service.Eventbrite
{
    public class SrvEApiKey
    {
        public string ApiKey { get; set; }
        public string ClientSecret { get; set; }
        public string PrivateToken { get; set; }
        public string PublicToken { get; set; }
        public string RedirectUri { get; set; }
    }
}
