using Dapper;
using Microsoft.Extensions.Logging;
using Microsoft.UI.Xaml.Controls;
using PPDesk.Abstraction.DTO.Repository.Table;
using PPDesk.Abstraction.Enum;
using PPDesk.Abstraction.Helper;
using PPDesk.Repository.Factory;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Z.Dapper.Plus;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PPDesk.Repository.Repositories
{
    public interface IMdlTableRepository : IForServiceCollectionExtension
    {
        Task<int> CountAllInformationTablesAsync();
        Task<int> CountInformationTablesAsync(string eventName, string gdrName, string master, EnumEventStatus? eventStatus, EnumTableType? tableType);
        Task CreateTableTablesAsync();
        Task DeleteAllTablesAsync();
        Task<IEnumerable<MdlInformationTable>> GetAllInformationTablesAsync();
        Task<IEnumerable<MdlTable>> GetAllTablesAsync();
        Task<IEnumerable<MdlInformationTable>> GetInformationTablesAsync(string eventName, string gdrName, string master, EnumEventStatus? eventStatus, EnumTableType? tableType, int page, int limit);
        Task InsertTablesAsync(IEnumerable<MdlTable> tables);
    }

    public class MdlTableRepository : BaseRepository<MdlTable>, IMdlTableRepository
    {
        public MdlTableRepository(IDatabaseConnectionFactory connectionFactory) : base(connectionFactory)
        {
        }

        public async Task CreateTableTablesAsync()
        {
            var connection = await _connectionFactory.CreateConnectionAsync();
            await connection.QueryAsync(@$"CREATE TABLE TABLES ( 
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                EventIdEventbride BIGINT NOT NULL,
                IdEventbride BIGINT NOT NULL,
                GdrName NVARCHAR(255), 
                Description NVARCHAR(255),
                Capacity SMALLINT NOT NULL,
                QuantitySold SMALLINT NOT NULL,
                StartDate DATETIME NOT NULL,
                EndDate DATETIME NOT NULL,
                Master NVARCHAR(255),
                Status SMALLINT NOT NULL, 
                Type SMALLINT NOT NULL
                )");
        }

        public async Task InsertTablesAsync(IEnumerable<MdlTable> tables)
        {
            var connection = await _connectionFactory.CreateConnectionAsync();
            await connection.BulkInsertAsync(tables);
        }

        public async Task<IEnumerable<MdlTable>> GetAllTablesAsync()
        {
            string sql = "SELECT * FROM TABLES;";
            var connection = await _connectionFactory.CreateConnectionAsync();

            return await connection.QueryAsync<MdlTable>(sql);
        }

        public async Task DeleteAllTablesAsync()
        {
            string sql = "DELETE FROM TABLES;";
            var connection = await _connectionFactory.CreateConnectionAsync();

            await connection.ExecuteAsync(sql);
        }

        public async Task<IEnumerable<MdlInformationTable>> GetInformationTablesAsync(string eventName, string gdrName, string master, EnumEventStatus? eventStatus, EnumTableType? tableType, int page, int limit)
        {
            int offset = page * limit;
            string sql = @"SELECT t.Id, e.Name as EventName, e.Status as EventStatus, t.GdrName, t.Capacity, t.QuantitySold, t.StartDate, t.EndDate, t.Master, t.Type as TableType
                from TABLES t
                join EVENTS e
                on t.EventIdEventbride = e.IdEventbride WHERE 1 = 1";

            sql += WhereTables(eventName, gdrName, master, eventStatus, tableType);

            sql += "ORDER BY EventName ASC " +
                "LIMIT @limit OFFSET @offset;";

            var connection = await _connectionFactory.CreateConnectionAsync();
            return await connection.QueryAsync<MdlInformationTable>(sql, new
            {
                eventName,
                gdrName,
                master,
                eventStatus,
                tableType,
                limit,
                offset
            });
        }

        public async Task<IEnumerable<MdlInformationTable>> GetAllInformationTablesAsync()
        {
            string sql = @"SELECT t.Id, e.Name as EventName, e.Status as EventStatus, t.GdrName, t.Capacity, t.QuantitySold, t.StartDate, t.EndDate, t.Master, t.Type as TableType
                from TABLES t
                join EVENTS e
                on t.EventIdEventbride = e.IdEventbride ORDER BY EventName ASC";

            var connection = await _connectionFactory.CreateConnectionAsync();
            return await connection.QueryAsync<MdlInformationTable>(sql);
        }

        public async Task<int> CountInformationTablesAsync(string eventName, string gdrName, string master, EnumEventStatus? eventStatus, EnumTableType? tableType)
        {
            string sql = @"SELECT COUNT(*)
                from TABLES t
                join EVENTS e
                on t.EventIdEventbride = e.IdEventbride WHERE 1 = 1";

            sql += WhereTables(eventName, gdrName, master, eventStatus, tableType);

            var connection = await _connectionFactory.CreateConnectionAsync();
            return await connection.QueryFirstAsync<int>(sql, new
            {
                eventName,
                gdrName,
                master,
                eventStatus,
                tableType,
            });
        }

        public async Task<int> CountAllInformationTablesAsync()
        {
            string sql = @"SELECT COUNT(*)
                from TABLES t
                join EVENTS e
                on t.EventIdEventbride = e.IdEventbride";

            var connection = await _connectionFactory.CreateConnectionAsync();
            return await connection.QueryFirstAsync<int>(sql);
        }

        private string WhereTables(string eventName, string gdrName, string master, EnumEventStatus? eventStatus, EnumTableType? tableType)
        {
            StringBuilder sb = new StringBuilder();

            if(!string.IsNullOrEmpty(eventName))
            {
                sb.Append("AND e.Name LIKE %@eventName%");
            }

            if (!string.IsNullOrEmpty(gdrName))
            {
                sb.AppendLine("AND g.Name LIKE %@gdrName%");
            }

            if (!string.IsNullOrEmpty(master))
            {
                sb.AppendLine("AND t.Master LIKE %@master%");
            }

            if (eventStatus.HasValue)
            {
                sb.AppendLine("AND e.Status = @eventStatus");
            }

            if (tableType.HasValue)
            {
                sb.AppendLine("AND t.Type = @tableType");
            }
            return sb.ToString();
        }
    }
}
