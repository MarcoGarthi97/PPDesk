using Dapper;
using PPDesk.Abstraction.DTO.Repository;
using PPDesk.Abstraction.Helper;
using PPDesk.Repository.Factory;
using System.Threading.Tasks;

namespace PPDesk.Repository.Repositories
{
    public interface IMdlEventRepository : IForServiceCollectionExtension
    {
        Task CreateTableEventsAsync();
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
                $"Id IDENTITY PRIMARY KEY," +
                $"Name VARCHAR(255) NOT NULL)");
        }
    }
}
