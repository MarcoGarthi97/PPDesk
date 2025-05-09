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
        public static IEnumerable<SrvEOrganization> Organizations { get; private set; }

        public static void SetOrganizations(IEnumerable<SrvEOrganization> organizations)
        {
            Organizations = organizations;
        }
    }
}
