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
using PPDesk.Service.Storages.PP;

namespace PPDesk.ViewModels
{
    public class UserViewModel : ObservableObject
    {
        private readonly ISrvUserService _userService;
        private IEnumerable<SrvUser> _usersList = new List<SrvUser>();
        private bool _loadFast;
        private string? _totalRecordsText;
        private string? _pageText;

        public string? FullNameFilter { get; set; }
        public string? PhoneFilter { get; set; }
        public string? EmailFilter { get; set; }
        public string? TotalRecordsText
        {
            get => _totalRecordsText;
            set => SetProperty(ref _totalRecordsText, value);
        }
        public string? PageText
        {
            get => _pageText;
            set => SetProperty(ref _pageText, value);
        }
        public ObservableCollection<SrvUser> Users { get; }= new ObservableCollection<SrvUser>();
        public IAsyncRelayCommand LoadUsersCommand { get; }
        public int _page = 0;
        public int _count = -1;

        public UserViewModel(ISrvUserService userService)
        {
            _userService = userService;
            LoadUsersCommand = new AsyncRelayCommand(LoadUsersAsync);
            _loadFast = SrvAppConfigurationStorage.DatabaseConfiguration.LoadFast;
        }

        public async Task LoadUsersAsync()
        {
            var usersTemp = await FilterUsersAsync();
            RecordsPagination(usersTemp);
        }

        private void BindGrid(IEnumerable<SrvUser> usersTemp)
        {
            Users.Clear();

            foreach (var x in usersTemp)
            {
                Users.Add(x);
            }
        }

        public async Task<IEnumerable<SrvUser>> FilterUsersAsync()
        {
            IEnumerable<SrvUser> usersTemp = new List<SrvUser>();

            if (_loadFast)
            {
                if (!_usersList.Any())
                {
                    _usersList = await _userService.GetAllUsersAsync();
                }

                var predicate = CreatePredicate();
                usersTemp = _usersList.Where(predicate).ToList();
                _count = usersTemp.Count();
            }
            else
            {
                usersTemp = await _userService.GetUsersAsync(FullNameFilter, PhoneFilter, EmailFilter, _page);
                _count = await _userService.CountUsersAsync(FullNameFilter, PhoneFilter, EmailFilter);
            }

            TotalRecordsText = $"Records: {_count}";
            PageText = $"Pagina: {_page + 1} di {(_count / 50) + 1}";

            return usersTemp;
        }

        private Func<SrvUser, bool> CreatePredicate()
        {
            return user => (string.IsNullOrWhiteSpace(FullNameFilter) ||
                             user.Name != null && user.Name.ToLower().Contains(FullNameFilter.ToLower(), StringComparison.OrdinalIgnoreCase)) &&
                            (string.IsNullOrWhiteSpace(PhoneFilter) ||
                             user.CellPhone != null && user.CellPhone.ToLower().Contains(PhoneFilter.ToLower(), StringComparison.OrdinalIgnoreCase)) &&
                            (string.IsNullOrWhiteSpace(EmailFilter) ||
                             user.Email != null && user.Email.ToLower().Contains(EmailFilter.ToLower(), StringComparison.OrdinalIgnoreCase));
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

                var usersTemp = await FilterUsersAsync();
                RecordsPagination(usersTemp);
            }
        }

        public async Task NextButton()
        {
            if(_count - (50 + (_page * 50)) > 0)
            {
                _page++;

                var usersTemp = await FilterUsersAsync();
                RecordsPagination(usersTemp);
            }
        }

        public async Task DataSortAsync(string propertyName, bool isAscending)
        {
            var usersTemp = await FilterUsersAsync();

            switch (propertyName)
            {
                case "FirstName":
                    usersTemp = isAscending
                        ? usersTemp.OrderBy(u => u.FirstName).ToList()
                        : usersTemp.OrderByDescending(u => u.FirstName).ToList();
                    break;
                case "LastName":
                    usersTemp = isAscending
                        ? usersTemp.OrderBy(u => u.LastName).ToList()
                        : usersTemp.OrderByDescending(u => u.LastName).ToList();
                    break;
                case "CellPhone":
                    usersTemp = isAscending
                        ? usersTemp.OrderBy(u => u.CellPhone).ToList()
                        : usersTemp.OrderByDescending(u => u.CellPhone).ToList();
                    break;
                case "Email":
                    usersTemp = isAscending
                        ? usersTemp.OrderBy(u => u.Email).ToList()
                        : usersTemp.OrderByDescending(u => u.Email).ToList();
                    break;
                default:
                    return;
            }

            RecordsPagination(usersTemp);
        }

        private void RecordsPagination(IEnumerable<SrvUser> usersTemp)
        {
            int skip = _page * 50;
            usersTemp = usersTemp.Skip(skip).Take(50);

            BindGrid(usersTemp);
        }
    }
}
