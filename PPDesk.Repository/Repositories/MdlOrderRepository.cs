using Dapper;
using PPDesk.Abstraction.DTO.Repository;
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
        Task CreateTableOrdersAsync();
        Task DeleteAllOrdersAsync();
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
                EventIdEventbride BIGINT NOT NULL,
                IdEventbride BIGINT NOT NULL,
                OrderIdEventbride BIGINT NOT NULL,
                TableIdEventbride BIGINT NOT NULL,
                Name NVARCHAR(255), 
                Quantity SMALLINT NOT NULL,
                Created DATETIME NOT NULL, 
                Cancelled SMALLINT NOT NULL
                )");
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
