using PPDesk.Abstraction.Helper;
using PPDesk.Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPDesk.Service.Services.PP
{
    public interface ISrvUserService : IForServiceCollectionExtension
    {
        Task CreateTableUsersAsync();
    }

    public class SrvUserService : ISrvUserService
    {

        private readonly IMdlUserRepository _userRepository;

        public SrvUserService(IMdlUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task CreateTableUsersAsync()
        {
            await _userRepository.CreateTableUsersAsync();
        }
    }
}
