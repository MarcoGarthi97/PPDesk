using Microsoft.Extensions.Logging;
using PPDesk.Abstraction.Helper;
using PPDesk.Repository.Repositories;
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
    }

    public class SrvDatabaseService : ISrvDatabaseService
    {
        private readonly ISrvUserService _userService;
        private readonly ISrvVersionService _versionService;
        private readonly ISrvTableService _tableService;
        private readonly ISrvOrderService _orderService;
        private readonly ISrvEventService _eventService;

        public SrvDatabaseService(ISrvUserService userService, ISrvVersionService versionService, ISrvTableService tableService, ISrvEventService eventService, ISrvOrderService orderService)
        {
            _userService = userService;
            _versionService = versionService;
            _tableService = tableService;
            _eventService = eventService;
            _orderService = orderService;
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
        }
    }
}
