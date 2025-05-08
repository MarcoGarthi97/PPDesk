using Dapper;
using PPDesk.Abstraction.DTO.Repository;
using PPDesk.Abstraction.Helper;
using PPDesk.Repository.Factory;
using System.Threading.Tasks;

namespace PPDesk.Repository.Repositories
{
    public interface IMdlTableUserRepository : IForServiceCollectionExtension
    {
        Task CreateTableTableUsersAsync();
    }

    public class MdlTableUserRepository : BaseRepository<MdlTableUser>, IMdlTableUserRepository
    {
        public MdlTableUserRepository(IDatabaseConnectionFactory connectionFactory) : base(connectionFactory)
        {
        }

        public async Task CreateTableTableUsersAsync()
        {
            var connection = await _connectionFactory.CreateConnectionAsync();
            await connection.QueryAsync($"CREATE TABLE TABLEUSERS (" +
                $"Id IDENTITY PRIMARY KEY," +
                $"TableId INT NOT NULL," +
                $"UserId INT NOT NULL," +
                $"TypeUser SMALLINT NOT NULL)");
        }
    }
}
