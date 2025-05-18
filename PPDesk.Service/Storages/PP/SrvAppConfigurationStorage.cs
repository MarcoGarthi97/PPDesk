using PPDesk.Abstraction.DTO.Service.Eventbrite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPDesk.Service.Storages.PP
{
    public static class SrvAppConfigurationStorage
    {
        public static SrvEOrganization EOrganization { get; private set; }
        public static bool LoadFast { get; private set; }

        public static void SetOrganization(SrvEOrganization eOrganization)
        {
            EOrganization = eOrganization;
        }

        public static void SetLoadFast(bool loadFast)
        {
            LoadFast = loadFast;
        }
    }
}
