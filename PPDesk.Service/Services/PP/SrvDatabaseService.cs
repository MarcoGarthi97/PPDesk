using Microsoft.Extensions.Logging;
using PPDesk.Abstraction.DTO.Service.Eventbrite;
using PPDesk.Abstraction.DTO.Service.PP;
using PPDesk.Abstraction.Helper;
using PPDesk.Repository.Repositories;
using PPDesk.Service.Services.Eventbrite;
using PPDesk.Service.Storages.PP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PPDesk.Service.Services.PP
{
    public interface ISrvDatabaseService : IForServiceCollectionExtension
    {
        Task CreateTablesAsync(IProgress<string> progress = null);
        Task LoadAllDataAsync();
        Task LoadAllDataAsync(IProgress<string> progress = null);
        Task LoadDatabaseExists();
        Task UpdateLiveDataAsync();
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
        private readonly ISrvHelperService _helperService;

        public SrvDatabaseService(ISrvUserService userService, ISrvVersionService versionService, ISrvTableService tableService, ISrvEventService eventService, ISrvOrderService orderService, ISrvEOrganizationService eOrganizationService, ISrvEOrderService eOrderService, ISrvEEventService eEventService, ISrvETicketClassService eTicketClassService, ISrvHelperService helperService)
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
            _helperService = helperService;
        }

        public async Task LoadDatabaseExists()
        {
            string version = await _versionService.GetVersionAsync();

            SrvAppConfigurationStorage.SetDatabaseExists(!string.IsNullOrEmpty(version));
        }

        public async Task LoadAllDataAsync()
        {
            await _eOrganizationService.LoadOrganizationsAsync();

            var eOrders = await _eOrderService.GetListOrdersByOrganizationIdAsync();

            var users = _userService.GetUsersByEOrders(eOrders);
            await _userService.UpsertUsersAsync(users);

            var orders = _orderService.GetOrdersByEOrders(eOrders);
            await _orderService.UpsertOrdersAsync(orders);

            var eEvents = await _eEventService.GetListEventsByOrganizationIdAsync();

            var events = _eventService.GetEventsByEEvents(eEvents);
            await _eventService.UpsertEventsAsync(events);

            var tickets = await _eTicketClassService.GetListTicketClassesByEventIdsAsync(eEvents.Select(x => x.Id));

            var tables = _tableService.GetTablesByETicketClasses(tickets);
            await _tableService.UpsertTablesAsync(tables);
        }

        public async Task UpdateLiveDataAsync()
        {
            if (SrvAppConfigurationStorage.EOrganization == null)
            {
                await _eOrganizationService.LoadOrganizationsAsync();
            }

            IEnumerable<SrvEEvent> liveEvents = await _eEventService.GetListEventsByOrganizationIdAsync();
            liveEvents = liveEvents.Where(x => x.Status != "completed" && x.Status != "canceled");

            foreach (var liveEvent in liveEvents)
            {
                var eOrders = await _eOrderService.GetListOrdersByEventIdAsync(liveEvent.Id);

                var users = _userService.GetUsersByEOrders(eOrders);
                await _userService.UpsertUsersAsync(users);

                var orders = _orderService.GetOrdersByEOrders(eOrders);
                await _orderService.UpsertOrdersAsync(orders);

                var eEvents = await _eEventService.GetListEventsByOrganizationIdAsync();

                var events = _eventService.GetEventsByEEvents(eEvents);
                await _eventService.UpsertEventsAsync(events);

                var tickets = await _eTicketClassService.GetListTicketClassesByEventIdsAsync(eEvents.Select(x => x.Id));

                var tables = _tableService.GetTablesByETicketClasses(tickets);
                await _tableService.UpsertTablesAsync(tables);
            }
        }

        public async Task CreateTablesAsync(IProgress<string> progress = null)
        {
            progress?.Report("Controllo versione database...");
            string version = await _versionService.GetVersionAsync();

            if (string.IsNullOrEmpty(version))
            {
                progress?.Report("Creazione tabella Versioni...");
                await _versionService.CreateTableVersionAsync();

                progress?.Report("Creazione tabella Utenti...");
                await _userService.CreateTableUsersAsync();

                progress?.Report("Creazione tabella Tables...");
                await _tableService.CreateTableTablesAsync();

                progress?.Report("Creazione tabella Ordini...");
                await _orderService.CreateTableOrdersAsync();

                progress?.Report("Creazione tabella Eventi...");
                await _eventService.CreateTableEventsAsync();

                progress?.Report("Creazione tabella Helpers...");
                await _helperService.CreateTableHelpersAsync();

                progress?.Report("Inserimento configurazione database iniziale...");
                var helpers = new List<SrvHelper>
                {
                    new SrvHelper
                    {
                        Key = "DatabaseConfiguration",
                        Json = JsonSerializer.Serialize(new SrvDatabaseConfigurationBySQL(true))
                    },
                    new SrvHelper
                    {
                        Key = "LiveBackgroundService",
                        Json = JsonSerializer.Serialize(new SrvBackgroundServiceConfiguration(10))
                    },
                    new SrvHelper
                    {
                        Key = "BackgroundService",
                        Json = JsonSerializer.Serialize(new SrvBackgroundServiceConfiguration(60))
                    }
                };
                await _helperService.InsertHelperAsync(helpers);

                progress?.Report("Inserimento versione iniziale...");
                await _versionService.InsertVersionAsync();

                progress?.Report("Database creato con successo!");
            }
            else
            {
                progress?.Report("Database già esistente, nessuna creazione necessaria.");
            }

            await LoadDatabaseExists();
        }

        public async Task LoadAllDataAsync(IProgress<string> progress = null)
        {
            progress?.Report("Caricamento organizzazioni...");
            await _eOrganizationService.LoadOrganizationsAsync();

            progress?.Report("Recupero ordini da Eventbrite...");
            var eOrders = await _eOrderService.GetListOrdersByOrganizationIdAsync();

            progress?.Report("Elaborazione utenti...");
            var users = _userService.GetUsersByEOrders(eOrders);
            await _userService.UpsertUsersAsync(users);

            progress?.Report("Elaborazione ordini...");
            var orders = _orderService.GetOrdersByEOrders(eOrders);
            await _orderService.UpsertOrdersAsync(orders);

            progress?.Report("Recupero eventi da Eventbrite...");
            var eEvents = await _eEventService.GetListEventsByOrganizationIdAsync();

            progress?.Report("Elaborazione eventi...");
            var events = _eventService.GetEventsByEEvents(eEvents);
            await _eventService.UpsertEventsAsync(events);

            progress?.Report("Recupero biglietti...");
            var tickets = await _eTicketClassService.GetListTicketClassesByEventIdsAsync(eEvents.Select(x => x.Id));

            progress?.Report("Elaborazione tabelle...");
            var tables = _tableService.GetTablesByETicketClasses(tickets);
            await _tableService.UpsertTablesAsync(tables);

            progress?.Report("Caricamento dati completato!");
        }
    }
}
