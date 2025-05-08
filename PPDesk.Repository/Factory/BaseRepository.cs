using PPDesk.Abstraction.Helper;
using System.Threading.Tasks;

namespace PPDesk.Repository.Factory
{
    public interface IRepository<T> where T : class
    {

    }

    public class BaseRepository<T> : IRepository<T> where T : class
    {
        protected readonly IDatabaseConnectionFactory _connectionFactory;

        public BaseRepository(IDatabaseConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }
    }
}
