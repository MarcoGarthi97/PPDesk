using PPDesk.Abstraction.DTO.Service.Eventbrite;
using PPDesk.Abstraction.DTO.Service.PP;
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
        public static SrvDatabaseConfiguration DatabaseConfiguration { get; private set; }

        public static void SetOrganization(SrvEOrganization eOrganization)
        {
            EOrganization = eOrganization;
        }

        public static void SetDatabaseConfigurations(SrvDatabaseConfiguration databaseConfiguration)
        {
            DatabaseConfiguration = databaseConfiguration;
        }
    }
}
