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
        public static SrvBackgroundServiceConfiguration LiveBackgroundServiceConfiguration { get; private set; }
        public static SrvBackgroundServiceConfiguration BackgroundServiceConfiguration { get; private set; }

        public static void SetOrganization(SrvEOrganization eOrganization)
        {
            EOrganization = eOrganization;
        }

        public static void SetDatabaseConfigurations(SrvDatabaseConfigurationBySQL databaseConfigurationSQL)
        {
            CreateSrvDatabaseConfiguration();
            DatabaseConfiguration.LoadFast = databaseConfigurationSQL.LoadFast;
        }

        public static void SetDatabaseExists(bool exists)
        {
            CreateSrvDatabaseConfiguration();
            DatabaseConfiguration.DatabaseExists = exists;
        }

        private static void CreateSrvDatabaseConfiguration()
        {
            if(DatabaseConfiguration == null)
            {
                DatabaseConfiguration = new SrvDatabaseConfiguration();
            }
        }

        public static void SetLiveBackgroundServiceConfiguration(SrvBackgroundServiceConfiguration srvBackgroundServiceConfiguration)
        {
            LiveBackgroundServiceConfiguration = srvBackgroundServiceConfiguration;
        }

        public static void SetBackgroundServiceConfiguration(SrvBackgroundServiceConfiguration srvBackgroundServiceConfiguration)
        {
            BackgroundServiceConfiguration = srvBackgroundServiceConfiguration;
        }
    }
}
