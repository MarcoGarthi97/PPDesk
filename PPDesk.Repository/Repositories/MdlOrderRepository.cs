using Dapper;
using PPDesk.Abstraction.DTO.Repository.Order;
using PPDesk.Abstraction.Enum;
using PPDesk.Abstraction.Helper;
using PPDesk.Repository.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z.Dapper.Plus;

namespace PPDesk.Repository.Repositories
{
    public interface IMdlOrderRepository : IForServiceCollectionExtension
    {
        Task<int> CountAllInformationOrdersAsync();
        Task<int> CountInformationOrdersAsync(string name, string nameOrder, string gdrName, string master, EnumEventStatus? status);
        Task CreateTableOrdersAsync();
        Task DeleteAllOrdersAsync();
        Task<IEnumerable<MdlInformationOrder>> GetAllInformationOrdersAsync();
        Task<IEnumerable<MdlInformationOrder>> GetInformationOrdersAsync(string name, string nameOrder, string gdrName, string master, EnumEventStatus? status, int page, int limit);
        Task<IEnumerable<MdlOrder>> GetOrdersAsync(int page, int limit);
        Task InsertOrdersAsync(IEnumerable<MdlOrder> orders);
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
                OrderIdOrderbride BIGINT NOT NULL,
                IdOrderbride BIGINT NOT NULL,
                OrderIdOrderbride BIGINT NOT NULL,
                TableIdOrderbride BIGINT NOT NULL,
                Name NVARCHAR(255), 
                Quantity SMALLINT NOT NULL,
                Created DATETIME NOT NULL, 
                Cancelled SMALLINT NOT NULL
                )");
        }

        public async Task<IEnumerable<MdlInformationOrder>> GetInformationOrdersAsync(string name, string nameOrder, string gdrName, string master, EnumEventStatus? status, int page, int limit)
        {
            int offset = page * limit;

            string sql = @"SELECT O.Id, O.IdOrderbride, O.Name, O.Quantity, O.Created AS DateOrder, E.Name AS NameOrder, E.Status AS StatusOrder, T.GdrName AS GdrName, T.Master 
                        from ORDERS O
                        JOIN EVENTS E
                        ON O.OrderIdOrderbride = E.IdOrderbride
                        JOIN TABLES T
                        ON O.TableIdOrderbride = T.IdOrderbride WHERE 1 = 1";

            sql += WhereOrders(name, nameOrder, gdrName, master, status);

            sql += "ORDER BY Start ASC " +
                "LIMIT @limit OFFSET @offset;";

            var connection = await _connectionFactory.CreateConnectionAsync();
            return await connection.QueryAsync<MdlInformationOrder>(sql, new
            {
                offset,
                limit,
                name,
                status
            });
        }

        public async Task<IEnumerable<MdlInformationOrder>> GetAllInformationOrdersAsync()
        {
            string sql = @"SELECT O.Id, O.IdOrderbride, O.Name, O.Quantity, O.Created AS DateOrder, E.Name AS NameOrder, E.Status AS StatusOrder, T.GdrName AS GdrName, T.Master 
                        from ORDERS O
                        JOIN EVENTS E
                        ON O.OrderIdOrderbride = E.IdOrderbride
                        JOIN TABLES T
                        ON O.TableIdOrderbride = T.IdOrderbride WHERE 1 = 1"" WHERE 1 = 1";

            var connection = await _connectionFactory.CreateConnectionAsync();
            return await connection.QueryAsync<MdlInformationOrder>(sql);
        }

        public async Task<int> CountInformationOrdersAsync(string name, string nameOrder, string gdrName, string master, EnumEventStatus? status)
        {
            string sql = @"SELECT COUNT(*)
                        FROM EVENTS E WHERE 1 = 1";

            sql += WhereOrders(name, nameOrder, gdrName, master, status);

            var connection = await _connectionFactory.CreateConnectionAsync();
            return await connection.QueryFirstAsync<int>(sql, new
            {
                name,
                status
            });
        }

        public async Task<int> CountAllInformationOrdersAsync()
        {
            string sql = @"SELECT COUNT(*)
                        FROM EVENTS E WHERE 1 = 1";

            var connection = await _connectionFactory.CreateConnectionAsync();
            return await connection.QueryFirstAsync<int>(sql);
        }

        private string WhereOrders(string name, string nameOrder, string gdrName, string master, EnumEventStatus? status)
        {
            StringBuilder sb = new StringBuilder();

            if (!string.IsNullOrEmpty(name))
            {
                sb.Append("AND O.Name LIKE %@name%");
            }

            if (!string.IsNullOrEmpty(nameOrder))
            {
                sb.Append("AND E.Name LIKE %@nameOrder%");
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

        public async Task DeleteAllOrdersAsync()
        {
            string sql = "DELETE FROM ORDERS;";
            var connection = await _connectionFactory.CreateConnectionAsync();

            await connection.ExecuteAsync(sql);
        }
    }
}
