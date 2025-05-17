using AutoMapper;
using PPDesk.Abstraction.DTO.Repository;
using PPDesk.Abstraction.DTO.Service.Eventbrite.Order;
using PPDesk.Abstraction.DTO.Service.PP;
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
        Task DeleteAllUsers();
        Task<IEnumerable<SrvUser>> GetUsersAsync(int page, int limit = 50);
        IEnumerable<SrvUser> GetUsersByEOrders(IEnumerable<SrvEOrder> eOrders);
        Task InsertUsersAsync(IEnumerable<SrvUser> srvUsers);
    }

    public class SrvUserService : ISrvUserService
    {
        private readonly IMdlUserRepository _userRepository;
        private readonly IMapper _mapper;

        public SrvUserService(IMdlUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task CreateTableUsersAsync()
        {
            await _userRepository.CreateTableUsersAsync();
        }

        public IEnumerable<SrvUser> GetUsersByEOrders(IEnumerable<SrvEOrder> eOrders)
        {
            IEnumerable<SrvEProfile> eProfiles = eOrders.SelectMany(x => x.Attendees.Select(y => y.Profile));
            IEnumerable<SrvUser> users = _mapper.Map<IEnumerable<SrvUser>>(eProfiles);

            return users.DistinctBy(x => x.Name);
        }

        public async Task<IEnumerable<SrvUser>> GetUsersAsync(int page, int limit = 50)
        {
            var mdlUsers = await _userRepository.GetUsersAsync(page, limit);
            return _mapper.Map<IEnumerable<SrvUser>>(mdlUsers);
        }

        public async Task InsertUsersAsync(IEnumerable<SrvUser> srvUsers)
        {
            var mdlUsers = _mapper.Map<IEnumerable<MdlUser>>(srvUsers);
            await _userRepository.InsertUsersAsync(mdlUsers);
        }

        public async Task DeleteAllUsers()
        {
            await _userRepository.DeleteAllUsersAsync();
        }
    }
}
