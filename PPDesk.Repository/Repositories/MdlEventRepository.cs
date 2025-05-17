using Dapper;
using PPDesk.Abstraction.DTO.Repository;
using PPDesk.Abstraction.DTO.Service.PP;
using PPDesk.Abstraction.Helper;
using PPDesk.Repository.Factory;
using System.Collections.Generic;
using System.Threading.Tasks;
using Z.Dapper.Plus;

namespace PPDesk.Repository.Repositories
{
    public interface IMdlEventRepository : IForServiceCollectionExtension
    {
        Task CreateTableEventsAsync();
        Task DeleteAllEventsAsync();
        Task<IEnumerable<MdlEvent>> GetEventsAsync(int page, int limit);
        Task InsertEventsAsync(IEnumerable<MdlEvent> events);
        Task UpdateEventsAsync(IEnumerable<MdlEvent> events);
    }

    public class MdlEventRepository : BaseRepository<MdlEvent>, IMdlEventRepository
    {
        public MdlEventRepository(IDatabaseConnectionFactory connectionFactory) : base(connectionFactory)
        {
        }

        public async Task CreateTableEventsAsync()  
        {
            var connection = await _connectionFactory.CreateConnectionAsync();
            await connection.QueryAsync($"CREATE TABLE EVENTS (" +
                $"Id INTEGER PRIMARY KEY AUTOINCREMENT," +
                $"IdEventbride BIGINT NOT NULL," +
                $"OrganizationId BIGINT NOT NULL," +
                $"Name VARCHAR(255) NOT NULL," +
                $"Description VARCHAR(255)," +
                $"Start DATETIME NOT NULL," +
                $"End DATETIME NOT NULL," +
                $"Created DATETIME NOT NULL," +
                $"Status SMALLINT NOT NULL)");
        }

        public async Task<IEnumerable<MdlEvent>> GetEventsAsync(int page, int limit)
        {
            int offset = page * limit;
            string sql = "SELECT * FROM EVENTS " +
                "ORDER BY NAME ASC " +
                "LIMIT @limit OFFSET @offset;";

            var connection = await _connectionFactory.CreateConnectionAsync();
            return await connection.QueryAsync<MdlEvent>(sql, new
            {
                limit,
                offset
            });
        }

        public async Task InsertEventsAsync(IEnumerable<MdlEvent> events)
        {
            var connection = await _connectionFactory.CreateConnectionAsync();
            await connection.BulkInsertAsync(events);
        }

        public async Task UpdateEventsAsync(IEnumerable<MdlEvent> events)
        {
            var connection = await _connectionFactory.CreateConnectionAsync();
            await connection.BulkUpdateAsync(events);
        }

        public async Task DeleteAllEventsAsync()
        {
            string sql = "DELETE FROM EVENTS;";
            var connection = await _connectionFactory.CreateConnectionAsync();

            await connection.ExecuteAsync(sql);
        }
    }
}
