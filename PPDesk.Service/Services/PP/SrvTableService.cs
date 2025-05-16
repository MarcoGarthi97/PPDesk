using AutoMapper;
using PPDesk.Abstraction.DTO.Repository;
using PPDesk.Abstraction.DTO.Service.Eventbrite;
using PPDesk.Abstraction.DTO.Service.PP;
using PPDesk.Abstraction.Helper;
using PPDesk.Repository.Repositories;
using PPDesk.Service.Builder;
using PPDesk.Service.Builder.Table;
using PPDesk.Service.Services.Eventbrite;
using System;
using System.Collections;
using System.Collections.Generic;
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
                    temp.EventId = Convert.ToInt64(ticketClass.EventId);

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
    }
}
