using AutoMapper;
using PPDesk.Abstraction.Helper;
using PPDesk.Repository.Repositories;
using System.Threading.Tasks;

namespace PPDesk.Service.Services.PP
{
    public interface ISrvTableUserService : IForServiceCollectionExtension
    {
        Task CreateTableTableUsersAsync();
    }

    public class SrvTableUserService : ISrvTableUserService
    {
        private readonly IMdlTableUserRepository _tableUserRepository;
        private readonly IMapper _mapper;

        public SrvTableUserService(IMdlTableUserRepository tableUserRepository, IMapper mapper)
        {
            _tableUserRepository = tableUserRepository;
            _mapper = mapper;
        }

        public async Task CreateTableTableUsersAsync()
        {
            await _tableUserRepository.CreateTableTableUsersAsync();
        }
    }
}
