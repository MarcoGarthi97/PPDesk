using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PPDesk.Abstraction.DTO.Service.PP.User;
using PPDesk.Abstraction.Helper;
using PPDesk.Service.Services.PP;
using PPDesk.Service.Storages.PP;

namespace PPDesk.ViewModels
{
    public class UserViewModel : ObservableObject, IForServiceCollectionExtension
    {
        private readonly ISrvUserService _userService;
        private IEnumerable<SrvInformationUser> _usersList = new List<SrvInformationUser>();
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
        public ObservableCollection<SrvInformationUser> Users { get; }= new ObservableCollection<SrvInformationUser>();
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

        private void BindGrid(IEnumerable<SrvInformationUser> usersTemp)
        {
            Users.Clear();

            foreach (var x in usersTemp)
            {
                Users.Add(x);
            }
        }

        public async Task<IEnumerable<SrvInformationUser>> FilterUsersAsync()
        {
            IEnumerable<SrvInformationUser> usersTemp = new List<SrvInformationUser>();

            if (_loadFast)
            {
                if (!_usersList.Any())
                {
                    _usersList = await _userService.GetAllInformationUsersAsync();
                }

                var predicate = CreatePredicate();
                usersTemp = _usersList.Where(predicate).ToList();
                _count = usersTemp.Count();
            }
            else
            {
                usersTemp = await _userService.GetInformationUsersAsync(FullNameFilter, PhoneFilter, EmailFilter, _page);
                _count = await _userService.CountUsersAsync(FullNameFilter, PhoneFilter, EmailFilter);
            }

            TotalRecordsText = $"Records: {_count}";
            PageText = $"Pagina: {_page + 1} di {(_count / 50) + 1}";

            return usersTemp;
        }

        private Func<SrvInformationUser, bool> CreatePredicate()
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
                case "EventsQuantity":
                    usersTemp = isAscending
                        ? usersTemp.OrderBy(u => u.EventsQuantity).ToList()
                        : usersTemp.OrderByDescending(u => u.EventsQuantity).ToList();
                    break;
                case "OrdersQuantity":
                    usersTemp = isAscending
                        ? usersTemp.OrderBy(u => u.OrdersQuantity).ToList()
                        : usersTemp.OrderByDescending(u => u.OrdersQuantity).ToList();
                    break;
                default:
                    return;
            }

            RecordsPagination(usersTemp);
        }

        private void RecordsPagination(IEnumerable<SrvInformationUser> usersTemp)
        {
            int skip = _page * 50;
            usersTemp = usersTemp.Skip(skip).Take(50);

            BindGrid(usersTemp);
        }
    }
}
