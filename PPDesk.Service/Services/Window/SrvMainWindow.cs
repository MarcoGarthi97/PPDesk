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
        Task TestAuth();
    }

    public class SrvMainWindow : ISrvMainWindow
    {
        private readonly ISrvEAuthenticationService _eAuthenticationService;

        public SrvMainWindow(ISrvEAuthenticationService eAuthenticationService)
        {
            _eAuthenticationService = eAuthenticationService;
        }

        public async Task TestAuth()
        {
            await _eAuthenticationService.GetAuthenticationTestAsync();
        }
    }
}
