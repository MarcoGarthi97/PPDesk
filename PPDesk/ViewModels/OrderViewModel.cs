using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PPDesk.Abstraction.DTO.Service.PP.Order;
using PPDesk.Abstraction.DTO.UI;
using PPDesk.Abstraction.Enum;
using PPDesk.Abstraction.Helper;
using PPDesk.Service.Services.PP;
using PPDesk.Service.Storages.PP;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPDesk.ViewModels
{
    public class OrderViewModel : ObservableObject, IForServiceCollectionExtension
    {
        private readonly ISrvOrderService _orderService;
        private readonly ISrvEventService _eventService;
        private IEnumerable<SrvInformationOrder> _ordersList = new List<SrvInformationOrder>();
        private bool _loadFast;
        private string? _totalRecordsText;
        private string? _pageText;
        private ComboBoxEventUI? _selectedEvent;
        private ComboBoxStatusEventUI? _selectedStatusEvent;
        public ComboBoxEventUI? SelectedEvent
        {
            get => _selectedEvent;
            set
            {
                SetProperty(ref _selectedEvent, value);
                OrderName = value?.Name;
            }
        }

        public ComboBoxStatusEventUI? SelectedStatusEvent
        {
            get => _selectedStatusEvent;
            set
            {
                SetProperty(ref _selectedStatusEvent, value);
                OrderStatus = value == null ? null : (EnumEventStatus)value?.Id!;
            }
        }
        public string? OrderName;
        public EnumEventStatus? OrderStatus;
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
        public ObservableCollection<SrvInformationOrder> Orders { get; } = new ObservableCollection<SrvInformationOrder>();
        public ObservableCollection<ComboBoxEventUI> ListEvents { get; } = new ObservableCollection<ComboBoxEventUI>();
        public ObservableCollection<ComboBoxStatusEventUI> ListStatusEvents { get; } = new ObservableCollection<ComboBoxStatusEventUI>();
        public IAsyncRelayCommand LoadOrdersCommand { get; }
        public int _page = 0;
        public int _count = -1;

        public OrderViewModel(ISrvOrderService orderService, ISrvEventService eventService)
        {
            _orderService = orderService;
            _eventService = eventService;

            LoadOrdersCommand = new AsyncRelayCommand(LoadOrdersAsync);
            _loadFast = SrvAppConfigurationStorage.DatabaseConfiguration.LoadFast;

            InitializeComboBoxesAsync().Wait();
        }

        public async Task LoadOrdersAsync()
        {
            var ordersTemp = await FilterOrdersAsync();
            RecordsPagination(ordersTemp);
        }

        private void BindGrid(IEnumerable<SrvInformationOrder> ordersTemp)
        {
            Orders.Clear();

            foreach (var x in ordersTemp)
            {
                Orders.Add(x);
            }
        }

        private async Task InitializeComboBoxesAsync()
        {
            await InitializeComboBoxEventsAsync();
            InitializeComboBoxStatusEvent();
        }

        private async Task InitializeComboBoxEventsAsync()
        {
            var events = await _eventService.GetComboBoxEventsUI();

            ListEvents.Clear();
            foreach (var e in events)
            {
                ListEvents.Add(e);
            }
        }

        private void InitializeComboBoxStatusEvent()
        {
            var statusEvents = new List<ComboBoxStatusEventUI> {
                new ComboBoxStatusEventUI(0, "Bozza"),
                new ComboBoxStatusEventUI(1, "Live"),
                new ComboBoxStatusEventUI(2, "Iniziato"),
                new ComboBoxStatusEventUI(3, "Concluso"),
                new ComboBoxStatusEventUI(4, "Completato"),
                new ComboBoxStatusEventUI(5, "Cancellato")
            };

            ListStatusEvents.Clear();
            statusEvents.ForEach(x => ListStatusEvents.Add(x));
        }

        public async Task<IEnumerable<SrvInformationOrder>> FilterOrdersAsync()
        {
            IEnumerable<SrvInformationOrder> ordersTemp = new List<SrvInformationOrder>();

            if (_loadFast)
            {
                if (!_ordersList.Any())
                {
                    _ordersList = await _orderService.GetAllInformationOrdersAsync();
                }

                var predicate = CreatePredicate();
                ordersTemp = _ordersList.Where(predicate).ToList();
                _count = ordersTemp.Count();
            }
            else
            {
                ordersTemp = await _orderService.GetInformationOrdersAsync(OrderName, OrderStatus, _page);
                _count = await _orderService.CountInformationOrdersAsync(OrderName, OrderStatus);
            }

            TotalRecordsText = $"Records: {_count}";
            PageText = $"Pagina: {_page + 1} di {(_count / 50) + 1}";

            return ordersTemp;
        }

        private Func<SrvInformationOrder, bool> CreatePredicate()
        {
            return e => (string.IsNullOrWhiteSpace(OrderName) ||
                             e.Name != null && e.Name.ToLower().Contains(OrderName.ToLower(), StringComparison.OrdinalIgnoreCase)) &&
            (!OrderStatus.HasValue ||
                             e.Status == OrderStatus);
        }

        public async Task<int> OrdersCountAsync()
        {
            if (_count == -1)
            {
                _count = await _orderService.CountAllInformationOrdersAsync();
            }

            return _count;
        }

        public async Task PrevButton()
        {
            if (_page > 0)
            {
                _page--;

                var ordersTemp = await FilterOrdersAsync();
                RecordsPagination(ordersTemp);
            }
        }

        public async Task NextButton()
        {
            if (_count - (50 + (_page * 50)) > 0)
            {
                _page++;

                var ordersTemp = await FilterOrdersAsync();
                RecordsPagination(ordersTemp);
            }
        }

        public async Task DataSortAsync(string propertyName, bool isAscending)
        {
            var ordersTemp = await FilterOrdersAsync();

            switch (propertyName)
            {
                case "Name":
                    ordersTemp = isAscending
                        ? ordersTemp.OrderBy(x => x.Name).ToList()
                        : ordersTemp.OrderByDescending(x => x.Name).ToList();
                    break;
                case "Status":
                    ordersTemp = isAscending
                        ? ordersTemp.OrderBy(x => x.Status).ToList()
                        : ordersTemp.OrderByDescending(x => x.Status).ToList();
                    break;
                case "Start":
                    ordersTemp = isAscending
                        ? ordersTemp.OrderBy(x => x.Start).ToList()
                        : ordersTemp.OrderByDescending(x => x.Start).ToList();
                    break;
                case "End":
                    ordersTemp = isAscending
                        ? ordersTemp.OrderBy(x => x.End).ToList()
                        : ordersTemp.OrderByDescending(x => x.End).ToList();
                    break;
                case "TotalUsers":
                    ordersTemp = isAscending
                        ? ordersTemp.OrderBy(x => x.TotalUsers).ToList()
                        : ordersTemp.OrderByDescending(x => x.TotalUsers).ToList();
                    break;
                case "TotalTicket":
                    ordersTemp = isAscending
                        ? ordersTemp.OrderBy(x => x.TotalTicket).ToList()
                        : ordersTemp.OrderByDescending(x => x.TotalTicket).ToList();
                    break;
                default:
                    break;
            }

            RecordsPagination(ordersTemp);
        }

        private void RecordsPagination(IEnumerable<SrvInformationOrder> ordersTemp)
        {
            int skip = _page * 50;
            ordersTemp = ordersTemp.Skip(skip).Take(50);

            BindGrid(ordersTemp);
        }
    }
}
