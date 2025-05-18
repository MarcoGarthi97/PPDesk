using Dapper;
using Microsoft.Data.Sqlite;
using PPDesk.Abstraction.DTO.Repository;
using PPDesk.Abstraction.Helper;
using PPDesk.Repository.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z.BulkOperations.Internal.InformationSchema;
using Z.Dapper.Plus;

namespace PPDesk.Repository.Repositories
{
    public interface IMdlUserRepository : IForServiceCollectionExtension
    {
        Task<int> CountUsersAsync();
        Task CreateTableUsersAsync();
        Task DeleteAllUsersAsync();
        Task<IEnumerable<MdlUser>> GetAllUsersAsync();
        Task<IEnumerable<MdlUser>> GetUsersAsync(string name, string phone, string email, int page, int limit);
        Task InsertUsersAsync(IEnumerable<MdlUser> users);
    }

    public class MdlUserRepository : BaseRepository<MdlUser>, IMdlUserRepository
    {
        public MdlUserRepository(IDatabaseConnectionFactory connectionFactory) : base(connectionFactory)
        {
        }

        public async Task CreateTableUsersAsync()
        {
            var connection = await _connectionFactory.CreateConnectionAsync();
            await connection.QueryAsync($"CREATE TABLE USERS (" +
                $"Id INTEGER PRIMARY KEY AUTOINCREMENT," +
                $"FirstName VARCHAR(255)," +
                $"LastName VARCHAR(255)," +
                $"Name VARCHAR(255) NOT NULL," +
                $"CellPhone VARCHAR(25)," +
                $"Email VARCHAR(255))");
        }

        public async Task<IEnumerable<MdlUser>> GetUsersAsync(string name, string phone, string email, int page, int limit)
        {
            int offset = page * limit;
            string sql = "SELECT * FROM USERS WHERE 1 = 1";

            if (!string.IsNullOrWhiteSpace(name))
            {
                sql += "AND Name LIKE %@name%";
            }

            if (!string.IsNullOrWhiteSpace(phone))
            {
                sql += "AND CellPhone LIKE %@phone%";
            }

            if (!string.IsNullOrWhiteSpace(email))
            {
                sql += "AND Email LIKE %@email%";
            }

            sql += "ORDER BY Name ASC " +
                "LIMIT @limit OFFSET @offset;";

            var connection = await _connectionFactory.CreateConnectionAsync();
            return await connection.QueryAsync<MdlUser>(sql, new
            {
                limit,
                offset
            });
        }

        public async Task<IEnumerable<MdlUser>> GetAllUsersAsync()
        {
            string sql = "SELECT * FROM USERS ;";

            var connection = await _connectionFactory.CreateConnectionAsync();
            return await connection.QueryAsync<MdlUser>(sql);
        }

        public async Task<int> CountUsersAsync()
        {
            string sql = "SELECT Count(*) FROM USERS ;";

            var connection = await _connectionFactory.CreateConnectionAsync();
            return await connection.QuerySingleAsync<int>(sql);
        }

        public async Task InsertUsersAsync(IEnumerable<MdlUser> users)
        {
            var connection = await _connectionFactory.CreateConnectionAsync();
            await connection.BulkInsertAsync(users);
        }

        public async Task DeleteAllUsersAsync()
        {
            string sql = "DELETE FROM USERS;";
            var connection = await _connectionFactory.CreateConnectionAsync();

            await connection.ExecuteAsync(sql);
        }
    }
}
