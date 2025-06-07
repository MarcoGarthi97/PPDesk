using Dapper;
using PPDesk.Abstraction.DTO.Repository.Event;
using PPDesk.Abstraction.DTO.Service.PP;
using PPDesk.Abstraction.Enum;
using PPDesk.Abstraction.Helper;
using PPDesk.Repository.Factory;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z.Dapper.Plus;

namespace PPDesk.Repository.Repositories
{
    public interface IMdlEventRepository : IForServiceCollectionExtension
    {
        Task<int> CountAllInformationEventsAsync();
        Task<int> CountInformationEventsAsync(string name, EnumEventStatus? status);
        Task CreateTableEventsAsync();
        Task DeleteAllEventsAsync();
        Task<IEnumerable<MdlInformationEvent>> GetAllInformationEventsAsync();
        Task<IEnumerable<MdlEvent>> GetEventsAsync(int page, int limit);
        Task<IEnumerable<MdlInformationEvent>> GetInformationEventsAsync(string name, EnumEventStatus? status, int page, int limit);
        Task InsertEventsAsync(IEnumerable<MdlEvent> events);
        Task UpdateEventsAsync(IEnumerable<MdlEvent> events);
        Task UpsertEventsAsync(IEnumerable<MdlEvent> events);
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
                $"IdEventbride BIGINT NOT NULL UNIQUE," +
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

        public async Task<IEnumerable<MdlInformationEvent>> GetInformationEventsAsync(string name, EnumEventStatus? status, int page, int limit)
        {
            int offset = page * limit;

            string sql = @"SELECT E.*, U.TotalUsers, T.TotalTicket
                        FROM EVENTS E
                        JOIN (
	                        SELECT COUNT(Name) AS TotalUsers, EventIdEventbride
	                        FROM ORDERS
	                        GROUP BY EventIdEventbride
                        ) U ON E.IdEventbride = U.EventIdEventbride
                        JOIN (
	                        SELECT SUM(Quantity) AS TotalTicket, EventIdEventbride
	                        FROM ORDERS
	                        GROUP BY EventIdEventbride
                        ) T ON E.IdEventbride = T.EventIdEventbride WHERE 1 = 1";

            sql += WhereEvents(name, status);

            sql += "ORDER BY Start ASC " +
                "LIMIT @limit OFFSET @offset;";

            var connection = await _connectionFactory.CreateConnectionAsync();
            return await connection.QueryAsync<MdlInformationEvent>(sql, new
            {
                offset,
                limit,
                name,
                status
            });
        }

        public async Task<IEnumerable<MdlInformationEvent>> GetAllInformationEventsAsync()
        {
            string sql = @"SELECT E.*, U.TotalUsers, T.TotalTicket
                        FROM EVENTS E
                        JOIN (
	                        SELECT COUNT(Name) AS TotalUsers, EventIdEventbride
	                        FROM ORDERS
	                        GROUP BY EventIdEventbride
                        ) U ON E.IdEventbride = U.EventIdEventbride
                        JOIN (
	                        SELECT SUM(Quantity) AS TotalTicket, EventIdEventbride
	                        FROM ORDERS
	                        GROUP BY EventIdEventbride
                        ) T ON E.IdEventbride = T.EventIdEventbride WHERE 1 = 1";

            var connection = await _connectionFactory.CreateConnectionAsync();
            return await connection.QueryAsync<MdlInformationEvent>(sql);
        }

        public async Task<int> CountInformationEventsAsync(string name, EnumEventStatus? status)
        {
            string sql = @"SELECT COUNT(*)
                        FROM EVENTS E WHERE 1 = 1";

            sql += WhereEvents(name, status);

            var connection = await _connectionFactory.CreateConnectionAsync();
            return await connection.QueryFirstAsync<int>(sql, new
            {
                name,
                status
            });
        }

        public async Task<int> CountAllInformationEventsAsync()
        {
            string sql = @"SELECT COUNT(*)
                        FROM EVENTS E WHERE 1 = 1";

            var connection = await _connectionFactory.CreateConnectionAsync();
            return await connection.QueryFirstAsync<int>(sql);
        }

        private string WhereEvents(string name, EnumEventStatus? status)
        {
            StringBuilder sb = new StringBuilder();

            if (!string.IsNullOrEmpty(name))
            {
                sb.Append("AND E.Name LIKE %@name%");
            }

            if (status.HasValue)
            {
                sb.AppendLine("AND E.Status = @status");
            }

            return sb.ToString();
        }

        public async Task InsertEventsAsync(IEnumerable<MdlEvent> events)
        {
            var connection = await _connectionFactory.CreateConnectionAsync();
            await connection.BulkInsertAsync(events);
        }

        public async Task UpsertEventsAsync(IEnumerable<MdlEvent> events)
        {
            const string upsertSql = @"
        INSERT INTO EVENTS (IdEventbride, Name, Description, Start, End, OrganizationId, Created, Status) 
        VALUES (@IdEventbride, @Name, @Description, @Start, @End, @OrganizationId, @Created, @Status)
        ON CONFLICT(IdEventbride) DO UPDATE SET
            Name = excluded.Name,
            Description = excluded.Description,
            Start = excluded.Start,
            End = excluded.End,
            OrganizationId = excluded.OrganizationId,
            Status = excluded.Status";

            var connection = await _connectionFactory.CreateConnectionAsync();
            await connection.ExecuteAsync(upsertSql, events);
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
