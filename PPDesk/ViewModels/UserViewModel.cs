using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PPDesk.Abstraction.DTO.Service.PP;
using PPDesk.Service.Services.PP;

namespace PPDesk.ViewModels
{
    public class UserViewModel : ObservableObject
    {
        private readonly ISrvUserService _userService;
        private IEnumerable<SrvUser> _usersList = new List<SrvUser>();

        public ObservableCollection<SrvUser> Users { get; }= new ObservableCollection<SrvUser>();
        public IAsyncRelayCommand LoadUsersCommand { get; }
        public int _page = 0;
        public int _count = -1;

        public UserViewModel(ISrvUserService userService)
        {
            _userService = userService;
            LoadUsersCommand = new AsyncRelayCommand(LoadUsersAsync);
        }

        private async Task LoadUsersAsync()
        {
            await FilterUsersAsync("", "", "");
        }

        private void BindGrid(IEnumerable<SrvUser> usersTemp)
        {
            Users.Clear();

            foreach (var u in usersTemp)
            {
                Users.Add(u);
            }
        }

        public async Task FilterUsersAsync(string name, string phone, string email)
        {
            IEnumerable<SrvUser> usersTemp = new List<SrvUser>();

            if (_count < 1000)
            {
                if (!_usersList.Any())
                {
                    _usersList = await _userService.GetAllUsersAsync();
                }

                usersTemp = _usersList.Where(x => x.Name.Contains(name)).ToList();

                usersTemp = usersTemp.Take(50);
            }
            else
            {
                usersTemp = await _userService.GetUsersAsync(name, phone, email, _page);
            }

            BindGrid(usersTemp);
        }

        public async Task<int> UsersCountAsync()
        {
            if(_count == -1)
            {
                _count = await _userService.CountUsersAsync();
            }

            return _count;
        }

        public async Task PrevButton()
        {
            if(_page > 0)
            {
                _page--;

                await GetRecordAsync();
            }
        }

        public async Task NextButton()
        {
            if(_count - (_page * 50)> 0)
            {
                _page++;

                await GetRecordAsync();
            }
        }

        private async Task GetRecordAsync()
        {
            if (_count < 1000)
            {
                RecordPagination();
            }
            else
            {
                await LoadUsersAsync();
            }
        }

        private void RecordPagination()
        {
            int skip = _page * 50;
            var usersTemp = _usersList.Skip(skip).Take(50);

            BindGrid(usersTemp);
        }
    }
}
