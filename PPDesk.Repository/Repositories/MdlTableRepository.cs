using Dapper;
using Microsoft.Extensions.Logging;
using PPDesk.Abstraction.DTO.Repository;
using PPDesk.Abstraction.Helper;
using PPDesk.Repository.Factory;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Threading.Tasks;
using Z.Dapper.Plus;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PPDesk.Repository.Repositories
{
    public interface IMdlTableRepository : IForServiceCollectionExtension
    {
        Task CreateTableTablesAsync();
        Task DeleteAllTablesAsync();
        Task<IEnumerable<MdlTable>> GetAllTablesAsync();
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
                EventId BIGINT NOT NULL,
                IdEventbride BIGINT NOT NULL,
                GdrName NVARCHAR(255), --oppure specifica una lunghezza come NVARCHAR(255) se preferisci
                Description NVARCHAR(255),
                Capacity SMALLINT NOT NULL,
                QuantitySold SMALLINT NOT NULL,
                StartDate DATETIME NOT NULL,
                EndDate DATETIME NOT NULL,
                Master NVARCHAR(255),
                Status SMALLINT NOT NULL, --Enum rappresentato come INT
                Type SMALLINT NOT NULL-- Enum rappresentato come INT
                )");
        }

        public async Task InsertTablesAsync(IEnumerable<MdlTable> tables)
        {
            var connection = await _connectionFactory.CreateConnectionAsync();
            await connection.BulkInsertAsync(tables);
        }

        public async Task<IEnumerable<MdlTable>> GetAllTablesAsync()
        {
            string sql = "SELECT * FROM Tables;";
            var connection = await _connectionFactory.CreateConnectionAsync();

            return await connection.QueryAsync<MdlTable>(sql);
        }

        public async Task DeleteAllTablesAsync()
        {
            string sql = "DELETE FROM Tables;";
            var connection = await _connectionFactory.CreateConnectionAsync();

            await connection.ExecuteAsync(sql);
        }
    }
}
