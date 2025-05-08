using Dapper;
using PPDesk.Abstraction.DTO.Repository;
using PPDesk.Abstraction.Helper;
using PPDesk.Repository.Factory;
using System.Threading.Tasks;

namespace PPDesk.Repository.Repositories
{
    public interface IMdlTableRepository : IForServiceCollectionExtension
    {
        Task CreateTableTablesAsync();
    }

    public class MdlTableRepository : BaseRepository<MdlTable>, IMdlTableRepository
    {
        public MdlTableRepository(IDatabaseConnectionFactory connectionFactory) : base(connectionFactory)
        {
        }

        public async Task CreateTableTablesAsync()
        {
            var connection = await _connectionFactory.CreateConnectionAsync();
            await connection.QueryAsync($"CREATE TABLE TABLES (" +
                $"Id IDENTITY PRIMARY KEY," +
                $"EventId INT NOT NULL," +
                $"Gdr VARCHAR(255) NOT NULL," +
                $"Status SMALLINT NOT NULL)");
        }
    }
}
