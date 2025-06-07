using Dapper;
using PPDesk.Abstraction.DTO.Repository.Order;
using PPDesk.Abstraction.Enum;
using PPDesk.Abstraction.Helper;
using PPDesk.Repository.Factory;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z.Dapper.Plus;

namespace PPDesk.Repository.Repositories
{
    public interface IMdlOrderRepository : IForServiceCollectionExtension
    {
        Task<int> CountAllInformationOrdersAsync();
        Task<int> CountInformationOrdersAsync(string name, string eventName, string gdrName, string master, EnumEventStatus? status, EnumTableType? type);
        Task CreateTableOrdersAsync();
        Task DeleteAllOrdersAsync();
        Task<IEnumerable<MdlInformationOrder>> GetAllInformationOrdersAsync();
        Task<IEnumerable<MdlInformationOrder>> GetInformationOrdersAsync(string name, string eventName, string gdrName, string master, EnumEventStatus? status, EnumTableType? type, int page, int limit);
        Task<IEnumerable<MdlOrder>> GetOrdersAsync(int page, int limit);
        Task InsertOrdersAsync(IEnumerable<MdlOrder> orders);
        Task UpsertOrdersAsync(IEnumerable<MdlOrder> orders);
    }

    public class MdlOrderRepository : BaseRepository<MdlOrder>, IMdlOrderRepository
    {
        public MdlOrderRepository(IDatabaseConnectionFactory connectionFactory) : base(connectionFactory)
        {
        }

        public async Task CreateTableOrdersAsync()
        {
            var connection = await _connectionFactory.CreateConnectionAsync();
            await connection.QueryAsync(@$"CREATE TABLE ORDERS ( 
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                IdEventbride BIGINT NOT NULL UNIQUE,
                EventIdEventbride BIGINT NOT NULL,
                OrderIdEventbride BIGINT NOT NULL,
                TableIdEventbride BIGINT NOT NULL,
                Name NVARCHAR(255), 
                Quantity SMALLINT NOT NULL,
                Created DATETIME NOT NULL, 
                Cancelled SMALLINT NOT NULL
                )");
        }

        public async Task<IEnumerable<MdlInformationOrder>> GetInformationOrdersAsync(string name, string eventName, string gdrName, string master, EnumEventStatus? status, EnumTableType? type, int page, int limit)
        {
            int offset = page * limit;

            string sql = @"SELECT O.Id, O.IdEventbride, O.Name, O.Quantity, O.Created AS DateOrder, E.Name AS EventName, E.Status AS StatusEvent, T.GdrName AS GdrName, T.Master, T.Type as TypeTable
                        from ORDERS O
                        JOIN EVENTS E
                        ON O.EventIdEventbride = E.IdEventbride
                        JOIN TABLES T
                        ON O.TableIdEventbride = T.IdEventbride WHERE 1 = 1";

            sql += WhereOrders(name, eventName, gdrName, master, status, type);

            sql += "ORDER BY Start ASC " +
                "LIMIT @limit OFFSET @offset;";

            var connection = await _connectionFactory.CreateConnectionAsync();
            return await connection.QueryAsync<MdlInformationOrder>(sql, new
            {
                name,
                eventName,
                gdrName,
                master,
                status,
                type,
                offset,
                limit,
            });
        }

        public async Task<IEnumerable<MdlInformationOrder>> GetAllInformationOrdersAsync()
        {
            string sql = @"SELECT O.Id, O.IdEventbride, O.Name, O.Quantity, O.Created AS DateOrder, E.Name AS EventName, E.Status AS StatusEvent, T.GdrName AS GdrName, T.Master, T.Type as TypeTable
                        from ORDERS O
                        JOIN EVENTS E
                        ON O.EventIdEventbride = E.IdEventbride
                        JOIN TABLES T
                        ON O.TableIdEventbride = T.IdEventbride WHERE 1 = 1";

            var connection = await _connectionFactory.CreateConnectionAsync();
            return await connection.QueryAsync<MdlInformationOrder>(sql);
        }

        public async Task<int> CountInformationOrdersAsync(string name, string eventName, string gdrName, string master, EnumEventStatus? status, EnumTableType? type)
        {
            string sql = @"SELECT COUNT(*)
                        FROM EVENTS E WHERE 1 = 1";

            sql += WhereOrders(name, eventName, gdrName, master, status, type);

            var connection = await _connectionFactory.CreateConnectionAsync();
            return await connection.QueryFirstAsync<int>(sql, new
            {
                name,
                eventName,
                gdrName,
                master,
                status,
                type
            });
        }

        public async Task<int> CountAllInformationOrdersAsync()
        {
            string sql = @"SELECT COUNT(*)
                        FROM EVENTS E WHERE 1 = 1";

            var connection = await _connectionFactory.CreateConnectionAsync();
            return await connection.QueryFirstAsync<int>(sql);
        }

        private string WhereOrders(string name, string eventName, string gdrName, string master, EnumEventStatus? status, EnumTableType? type)
        {
            StringBuilder sb = new StringBuilder();

            if (!string.IsNullOrEmpty(name))
            {
                sb.Append("AND O.Name LIKE %@name%");
            }

            if (!string.IsNullOrEmpty(eventName))
            {
                sb.Append("AND E.Name LIKE %@eventName%");
            }

            if (!string.IsNullOrEmpty(gdrName))
            {
                sb.Append("AND T.GdrName LIKE %@gdrName%");
            }

            if (!string.IsNullOrEmpty(master))
            {
                sb.Append("AND T.Master LIKE %@master%");
            }

            if (status.HasValue)
            {
                sb.AppendLine("AND E.Status = @status");
            }

            if (type.HasValue)
            {
                sb.AppendLine("AND T.Type = @type");
            }

            return sb.ToString();
        }

        public async Task<IEnumerable<MdlOrder>> GetOrdersAsync(int page, int limit)
        {
            int offset = page * limit;
            string sql = "SELECT * FROM ORDERS " +
                "ORDER BY NAME ASC " +
                "LIMIT @limit OFFSET @offset;";

            var connection = await _connectionFactory.CreateConnectionAsync();
            return await connection.QueryAsync<MdlOrder>(sql, new
            {
                limit,
                offset
            });
        }

        public async Task InsertOrdersAsync(IEnumerable<MdlOrder> orders)
        {
            var connection = await _connectionFactory.CreateConnectionAsync();
            await connection.BulkInsertAsync(orders);
        }

        public async Task UpsertOrdersAsync(IEnumerable<MdlOrder> orders)
        {
            const string upsertSql = @"
        INSERT INTO ORDERS (IdEventbride, EventIdEventbride, OrderIdEventbride, TableIdEventbride, Name, Created, Quantity, Cancelled) 
        VALUES (@IdEventbride, @EventIdEventbride, @OrderIdEventbride, @TableIdEventbride, @Name, @Created, @Quantity, @Cancelled)
        ON CONFLICT(IdEventbride) DO UPDATE SET
            EventIdEventbride = excluded.EventIdEventbride,
            OrderIdEventbride = excluded.OrderIdEventbride,
            TableIdEventbride = excluded.TableIdEventbride,
            Name = excluded.Name,
            Quantity = excluded.Quantity,
            Cancelled = excluded.Cancelled";

            var connection = await _connectionFactory.CreateConnectionAsync();
            await connection.ExecuteAsync(upsertSql, orders);
        }

        public async Task DeleteAllOrdersAsync()
        {
            string sql = "DELETE FROM ORDERS;";
            var connection = await _connectionFactory.CreateConnectionAsync();

            await connection.ExecuteAsync(sql);
        }
    }
}
