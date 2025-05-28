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
using Z.BulkOperations.Internal.InformationSchema;

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
        private ComboBoxTypeTableUI? _selectedTypeTable;
        private string _selectedNameGdr;
        private string _selectedMaster;
        private string _selectedName;
        public ComboBoxEventUI? SelectedEvent
        {
            get => _selectedEvent;
            set
            {
                SetProperty(ref _selectedEvent, value);
                EventName = value?.Name;
            }
        }

        public ComboBoxStatusEventUI? SelectedStatusEvent
        {
            get => _selectedStatusEvent;
            set
            {
                SetProperty(ref _selectedStatusEvent, value);
                EventStatus = value == null ? null : (EnumEventStatus)value?.Id!;
            }
        }
        public ComboBoxTypeTableUI? SelectedTypeStable
        {
            get => _selectedTypeTable;
            set
            {
                SetProperty(ref _selectedTypeTable, value);
                TypeTable = value == null ? null : (EnumTableType)value?.Id!;
            }
        }
        public string? GdrName
        {
            get => _selectedNameGdr;
            set
            {
                SetProperty(ref _selectedNameGdr!, value);
            }
        }
        public string? Master
        {
            get => _selectedMaster;
            set
            {
                SetProperty(ref _selectedMaster!, value);
            }
        }
        public string? Name
        {
            get => _selectedName;
            set
            {
                SetProperty(ref _selectedName!, value);
            }
        }
        public string? EventName;
        public EnumEventStatus? EventStatus;
        public EnumTableType? TypeTable;
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
        public ObservableCollection<ComboBoxTypeTableUI> ListTypeTables { get; } = new ObservableCollection<ComboBoxTypeTableUI>();
        public IAsyncRelayCommand LoadOrdersCommand { get; }
        public int _page = 0;
        public int _count = -1;

        public OrderViewModel(ISrvOrderService orderService, ISrvEventService eventService)
        {
            _orderService = orderService;
            _eventService = eventService;

            LoadOrdersCommand = new AsyncRelayCommand(LoadOrdersAsync);
            _loadFast = SrvAppConfigurationStorage.DatabaseConfiguration.LoadFast;
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

        public async Task InitializeComboBoxesAsync()
        {
            await InitializeComboBoxEventsAsync();
            InitializeComboBoxStatusEvent();
            InitializeComboBoxTypeTable();
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

        private void InitializeComboBoxTypeTable()
        {
            var typeTables = new List<ComboBoxTypeTableUI>
            {
                new ComboBoxTypeTableUI(0, "Sessione Gdr"),
                new ComboBoxTypeTableUI(1, "Multi Tavolo")
            };

            ListTypeTables.Clear();
            typeTables.ForEach(x => ListTypeTables.Add(x));
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
                ordersTemp = await _orderService.GetInformationOrdersAsync(Name, EventName, GdrName, Master, EventStatus, TypeTable, _page);
                _count = await _orderService.CountInformationOrdersAsync(Name, EventName, GdrName, Master, EventStatus, TypeTable);
            }

            TotalRecordsText = $"Records: {_count}";
            PageText = $"Pagina: {_page + 1} di {(_count / 50) + 1}";

            return ordersTemp;
        }

        private Func<SrvInformationOrder, bool> CreatePredicate()
        {
            return e => (string.IsNullOrWhiteSpace(Name) ||
                             e.Name != null && e.Name.ToLower().Contains(Name.ToLower(), StringComparison.OrdinalIgnoreCase)) &&
                        (string.IsNullOrWhiteSpace(EventName) ||
                             e.EventName != null && e.EventName.ToLower().Contains(EventName.ToLower(), StringComparison.OrdinalIgnoreCase)) &&
                        (string.IsNullOrWhiteSpace(GdrName) ||
                             e.GdrName != null && e.GdrName.ToLower().Contains(GdrName.ToLower(), StringComparison.OrdinalIgnoreCase)) &&
                        (string.IsNullOrWhiteSpace(Master) ||
                             e.Master != null && e.Master.ToLower().Contains(Master.ToLower(), StringComparison.OrdinalIgnoreCase)) &&
            (!EventStatus.HasValue ||
                             e.StatusEvent == EventStatus) &&
            (!TypeTable.HasValue ||
                             e.TypeTable == TypeTable);
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
                case "DateOrder":
                    ordersTemp = isAscending
                        ? ordersTemp.OrderBy(x => x.DateOrder).ToList()
                        : ordersTemp.OrderByDescending(x => x.DateOrder).ToList();
                    break;
                case "Quantity":
                    ordersTemp = isAscending
                        ? ordersTemp.OrderBy(x => x.Quantity).ToList()
                        : ordersTemp.OrderByDescending(x => x.Quantity).ToList();
                    break;
                case "EventName":
                    ordersTemp = isAscending
                        ? ordersTemp.OrderBy(x => x.EventName).ToList()
                        : ordersTemp.OrderByDescending(x => x.EventName).ToList();
                    break;
                case "StatusEvent":
                    ordersTemp = isAscending
                        ? ordersTemp.OrderBy(x => x.StatusEvent).ToList()
                        : ordersTemp.OrderByDescending(x => x.StatusEvent).ToList();
                    break;
                case "GdrName":
                    ordersTemp = isAscending
                        ? ordersTemp.OrderBy(x => x.GdrName).ToList()
                        : ordersTemp.OrderByDescending(x => x.GdrName).ToList();
                    break;
                case "Master":
                    ordersTemp = isAscending
                        ? ordersTemp.OrderBy(x => x.Master).ToList()
                        : ordersTemp.OrderByDescending(x => x.Master).ToList();
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
