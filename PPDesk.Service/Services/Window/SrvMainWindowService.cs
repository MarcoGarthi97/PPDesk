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
        private readonly ISrvEAuthenticationService _eAuthenticationService;
        private readonly ISrvEOrganizationService _eOrganizationService;
        private readonly ISrvEEventService _eEventService;
        private readonly ISrvEOrderService _eOrderService;
        private readonly ISrvETicketClassService _eTicketClassService;
        private readonly ISrvTableService _tableService;

        public SrvMainWindowService(ISrvEOrganizationService eOrganizationService, ISrvEAuthenticationService eAuthenticationService, ISrvEEventService eEventService, ISrvEOrderService eOrderService, ISrvETicketClassService eTicketClassService, ISrvTableService tableService)
        {
            _eOrganizationService = eOrganizationService;
            _eAuthenticationService = eAuthenticationService;
            _eEventService = eEventService;
            _eOrderService = eOrderService;
            _eTicketClassService = eTicketClassService;
            _tableService = tableService;
        }

        public async Task Test()
        {
            await _eAuthenticationService.GetAuthenticationAsync();
            await _eOrganizationService.LoadOrganizationsAsync();
            var events = await _eEventService.GetListEventsByOrganizationIdAsync();
            //var orders = await _eOrderService.GetListOrdersByOrganizationIdAsync();
            var ticketClasses = await _eTicketClassService.GetListTicketClassesByEventIdsAsync(events.Select(x => x.Id));
            var tables = _tableService.GetTablesByETicketClasses(ticketClasses);
        }
    }
}
