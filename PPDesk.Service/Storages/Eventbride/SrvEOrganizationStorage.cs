using PPDesk.Abstraction.DTO.Service.Eventbrite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPDesk.Service.Storages.Eventbride
{
    public static class SrvEOrganizationStorage
    {
    }

    public static class SrvEApiKeyStorage
    {
        public static SrvEApiKey Configuration { get; private set; }
        
        public static void SetpiKeyStorage(SrvEApiKey srvEApiKey)
        {
            Configuration = srvEApiKey;
        }
    }
}
