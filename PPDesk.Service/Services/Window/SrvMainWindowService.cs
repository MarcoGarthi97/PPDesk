using PPDesk.Abstraction.Helper;
using PPDesk.Service.Services.Eventbrite;
using PPDesk.Service.Services.PP;
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
        private readonly ISrvEOrganizationService _eOrganizationService;
        private readonly ISrvEOrderService _eOrderService;
        private readonly ISrvUserService _userService;

        public SrvMainWindowService(ISrvEOrganizationService eOrganizationService, ISrvEOrderService eOrderService, ISrvUserService userService)
        {
            _eOrganizationService = eOrganizationService;
            _eOrderService = eOrderService;
            _userService = userService;
        }

        public async Task Test()
        {
            try
            {
                await _eOrganizationService.LoadOrganizationsAsync();

                var orders = await _eOrderService.GetListOrdersByOrganizationIdAsync();
                
                var users = _userService.GetUsersByEOrders(orders);
                await _userService.InsertUsersAsync(users);
            }
            catch(Exception ex)
            {

            }
        }
    }
}
