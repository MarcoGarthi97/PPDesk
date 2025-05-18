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
        private readonly ISrvEEventService _eEventService;
        private readonly ISrvETicketClassService _eTicketClassService;
        private readonly ISrvUserService _userService;
        private readonly ISrvEventService _eventService;
        private readonly ISrvOrderService _orderService;
        private readonly ISrvTableService _tableService;

        public SrvMainWindowService(ISrvEOrganizationService eOrganizationService, ISrvEOrderService eOrderService, ISrvUserService userService, ISrvEEventService eEventService, ISrvETicketClassService eTicketClassService, ISrvEventService eventService, ISrvOrderService orderService, ISrvTableService tableService)
        {
            _eOrganizationService = eOrganizationService;
            _eOrderService = eOrderService;
            _userService = userService;
            _eEventService = eEventService;
            _eTicketClassService = eTicketClassService;
            _eventService = eventService;
            _orderService = orderService;
            _tableService = tableService;
        }

        public async Task Test()
        {
            try
            {
                await _userService.DeleteAllUsers();
                await _eventService.DeleteAllEvents();
                await _orderService.DeleteAllOrders();
                await _tableService.DeleteAllTablesAsync();

                await _eOrganizationService.LoadOrganizationsAsync();

                var eOrders = await _eOrderService.GetListOrdersByOrganizationIdAsync();
                
                var users = _userService.GetUsersByEOrders(eOrders);
                await _userService.InsertUsersAsync(users);

                var orders = _orderService.GetOrdersByEOrders(eOrders);
                await _orderService.InsertOrdersAsync(orders);

                var eEvents = await _eEventService.GetListEventsByOrganizationIdAsync();

                var events = _eventService.GetEventsByEEvents(eEvents);
                await _eventService.InsertEventsAsync(events);

                var tickets = await _eTicketClassService.GetListTicketClassesByEventIdsAsync(eEvents.Select(x => x.Id));
                
                var tables = _tableService.GetTablesByETicketClasses(tickets);
                await _tableService.InsertTablesAsync(tables);
            }
            catch(Exception ex)
            {

            }
        }
    }
}
