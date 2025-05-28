using Microsoft.Extensions.Logging;
using PPDesk.Abstraction.Helper;
using PPDesk.Repository.Repositories;
using PPDesk.Service.Services.Eventbrite;
using PPDesk.Service.Storages.PP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPDesk.Service.Services.PP
{
    public interface ISrvDatabaseService : IForServiceCollectionExtension
    {
        Task CreateTablesAsync();
        Task LoadDatabaseExists();
    }

    public class SrvDatabaseService : ISrvDatabaseService
    {
        private readonly ISrvEOrganizationService _eOrganizationService;
        private readonly ISrvEOrderService _eOrderService;
        private readonly ISrvEEventService _eEventService;
        private readonly ISrvETicketClassService _eTicketClassService;
        private readonly ISrvUserService _userService;
        private readonly ISrvVersionService _versionService;
        private readonly ISrvTableService _tableService;
        private readonly ISrvOrderService _orderService;
        private readonly ISrvEventService _eventService;

        public SrvDatabaseService(ISrvUserService userService, ISrvVersionService versionService, ISrvTableService tableService, ISrvEventService eventService, ISrvOrderService orderService, ISrvEOrganizationService eOrganizationService, ISrvEOrderService eOrderService, ISrvEEventService eEventService, ISrvETicketClassService eTicketClassService)
        {
            _userService = userService;
            _versionService = versionService;
            _tableService = tableService;
            _eventService = eventService;
            _orderService = orderService;
            _eOrganizationService = eOrganizationService;
            _eOrderService = eOrderService;
            _eEventService = eEventService;
            _eTicketClassService = eTicketClassService;
        }

        public async Task CreateTablesAsync()
        {
            string version = await _versionService.GetVersionAsync();

            if(string.IsNullOrEmpty(version))
            {
                await _versionService.CreateTableVersionAsync();
                await _userService.CreateTableUsersAsync();
                await _tableService.CreateTableTablesAsync();
                await _orderService.CreateTableOrdersAsync();
                await _eventService.CreateTableEventsAsync();

                await _versionService.InsertVersionAsync();
            }

            await LoadDatabaseExists();
        }

        public async Task LoadDatabaseExists()
        {
            string version = await _versionService.GetVersionAsync();

            SrvAppConfigurationStorage.DatabaseConfiguration.DatabaseExists = !string.IsNullOrEmpty(version);
        }

        public async Task LoadAllDataAsync()
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
    }
}
