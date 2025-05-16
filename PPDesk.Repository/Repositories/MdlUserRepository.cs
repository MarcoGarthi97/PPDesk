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

namespace PPDesk.Repository.Repositories
{
    public interface IMdlUserRepository : IForServiceCollectionExtension
    {
        Task CreateTableUsersAsync();
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
                $"Name VARCHAR(255) NOT NULL)");
        }
    }
}
