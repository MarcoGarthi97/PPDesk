using PPDesk.Abstraction.Helper;
using PPDesk.Service.Services.Eventbrite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PPDesk.Service.Services.Window
{
    public interface ISrvMainWindowService : IForServiceCollectionExtension
    {
        Task Test();
    }

    public class SrvMainWindowService : ISrvMainWindowService
    {
        private readonly ISrvEAuthenticationService _eAuthenticationService;
        private readonly ISrvEOrganizationService _eOrganizationService;

        public SrvMainWindowService(ISrvEOrganizationService eOrganizationService, ISrvEAuthenticationService eAuthenticationService)
        {
            _eOrganizationService = eOrganizationService;
            _eAuthenticationService = eAuthenticationService;
        }

        public async Task Test()
        {
            await _eAuthenticationService.GetAuthenticationAsync();
            await _eOrganizationService.LoadOrganizationsAsync();
        }
    }
}
