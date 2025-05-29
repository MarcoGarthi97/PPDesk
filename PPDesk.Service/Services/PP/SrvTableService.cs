using AutoMapper;
using Microsoft.UI.Xaml.Controls;
using PPDesk.Abstraction.DTO.Repository.Table;
using PPDesk.Abstraction.DTO.Service.Eventbrite;
using PPDesk.Abstraction.DTO.Service.PP.Table;
using PPDesk.Abstraction.Enum;
using PPDesk.Abstraction.Helper;
using PPDesk.Repository.Repositories;
using PPDesk.Service.Builder;
using PPDesk.Service.Builder.Table;
using PPDesk.Service.Services.Eventbrite;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace PPDesk.Service.Services.PP
{
    public interface ISrvTableService : IForServiceCollectionExtension
    {
        Task CreateTableTablesAsync();
        Task DeleteAllTablesAsync();
        Task<IEnumerable<SrvTable>> GetAllTablesAsync();
        IEnumerable<SrvTable> GetTablesByETicketClasses(IEnumerable<SrvETicketClass> ticketClasses);
        Task InsertTablesAsync(IEnumerable<SrvTable> srvTables);
        Task<int> CountAllInformationTablesAsync();
        Task<int> CountInformationTablesAsync(string eventName, string gdrName, string master, EnumEventStatus? eventStatus, EnumTableType? tableType);
        Task<IEnumerable<SrvInformationTable>> GetInformationTablesAsync(string eventName, string gdrName, string master, EnumEventStatus? eventStatus, EnumTableType? tableType, int page, int limit = 50);
        Task<IEnumerable<SrvInformationTable>> GetAllInformationTablesAsync();
    }

    public class SrvTableService : ISrvTableService
    {
        private readonly IMdlTableRepository _tableRepository;
        private readonly ISrvTableOrchestratorBuilder _tableBuilder;
        private readonly IMapper _mapper;

        public SrvTableService(IMdlTableRepository tableRepository, IMapper mapper, ISrvTableOrchestratorBuilder tableBuilder)
        {
            _tableRepository = tableRepository;
            _mapper = mapper;
            _tableBuilder = tableBuilder;
        }

        public async Task CreateTableTablesAsync()
        {
            await _tableRepository.CreateTableTablesAsync();
        }

        public IEnumerable<SrvTable> GetTablesByETicketClasses(IEnumerable<SrvETicketClass> ticketClasses)
        {
            var tables = new List<SrvTable>();
            foreach (var ticketClass in ticketClasses)
            {
                try
                {
                    var temp = _tableBuilder.TableByTickeClassBuilder(ticketClass);
                    temp.IdEventbride = Convert.ToInt64(ticketClass.Id);
                    temp.QuantitySold = ticketClass.QuantitySold;
                    temp.Capacity = ticketClass.Capacity;
                    temp.EventIdEventbride = Convert.ToInt64(ticketClass.EventId);

                    tables.Add(temp);
                }
                catch(Exception ex)
                {

                }
            }

            return tables;
        }

        public async Task InsertTablesAsync(IEnumerable<SrvTable> srvTables)
        {
            var mdlTables = _mapper.Map<IEnumerable<MdlTable>>(srvTables);

            await _tableRepository.InsertTablesAsync(mdlTables);
        }

        public async Task<IEnumerable<SrvTable>> GetAllTablesAsync()
        {
            var mdlTables = await _tableRepository.GetAllTablesAsync();

            return _mapper.Map<IEnumerable<SrvTable>>(mdlTables);
        }

        public async Task DeleteAllTablesAsync()
        {
            await _tableRepository.DeleteAllTablesAsync();
        }

        public async Task<int> CountAllInformationTablesAsync()
        {
            return await _tableRepository.CountAllInformationTablesAsync();
        }

        public async Task<int> CountInformationTablesAsync(string eventName, string gdrName, string master, EnumEventStatus? eventStatus, EnumTableType? tableType)
        {
            return await _tableRepository.CountInformationTablesAsync(eventName, gdrName, master, eventStatus, tableType);
        }

        public async Task<IEnumerable<SrvInformationTable>> GetInformationTablesAsync(string eventName, string gdrName, string master, EnumEventStatus? eventStatus, EnumTableType? tableType, int page, int limit = 50)
        {
            var mdlInformationsTable = await _tableRepository.GetInformationTablesAsync(eventName, gdrName, master, eventStatus, tableType, page, limit);
            return _mapper.Map<IEnumerable<SrvInformationTable>>(mdlInformationsTable);
        }

        public async Task<IEnumerable<SrvInformationTable>> GetAllInformationTablesAsync()
        {
            var mdlInformationsTable = await _tableRepository.GetAllInformationTablesAsync();
            return _mapper.Map<IEnumerable<SrvInformationTable>>(mdlInformationsTable);
        }
    }
}
