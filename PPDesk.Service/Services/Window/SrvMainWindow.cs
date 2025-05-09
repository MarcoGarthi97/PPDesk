using PPDesk.Abstraction.Helper;
using PPDesk.Service.Services.Eventbrite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPDesk.Service.Services.Window
{
    public interface ISrvMainWindow : IForServiceCollectionExtension
    {
        Task Test();
    }

    public class SrvMainWindow : ISrvMainWindow
    {
        private readonly ISrvEOrganizationService _eOrganizationService;

        public SrvMainWindow(ISrvEOrganizationService eOrganizationService)
        {
            _eOrganizationService = eOrganizationService;
        }

        public async Task Test()
        {
            await _eOrganizationService.LoadOrganizationsAsync();
        }
    }
}
