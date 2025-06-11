using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PPDesk.Abstraction.DTO.Service.PP.Table;
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
using Windows.ApplicationModel.Email;
using Windows.System;

namespace PPDesk.ViewModels
{
    public class TableViewModel : ObservableObject, IForServiceCollectionExtension
    {
        private readonly ISrvTableService _tableService;
        private readonly ISrvEventService _eventService;
        private IEnumerable<SrvInformationTable> _tablesList = new List<SrvInformationTable>();
        private bool _loadFast;
        private string? _totalRecordsText;
        private string? _pageText;
        private ComboBoxEventUI? _selectedEvent;
        private ComboBoxStatusEventUI? _selectedStatusEvent;
        private ComboBoxTypeTableUI? _selectedTypeTable;
        private string _selectedNameGdr;
        private string _selectedMaster;
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
                TableType = value == null ? null : (EnumTableType)value?.Id!;
            }
        }

        public string? EventName;
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

        public EnumEventStatus? EventStatus;
        public EnumTableType? TableType;
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
        public ObservableCollection<SrvInformationTable> Tables { get; } = new ObservableCollection<SrvInformationTable>();
        public ObservableCollection<ComboBoxEventUI> ListEvents { get; } = new ObservableCollection<ComboBoxEventUI>();
        public ObservableCollection<ComboBoxStatusEventUI> ListStatusEvents { get; } = new ObservableCollection<ComboBoxStatusEventUI>();
        public ObservableCollection<ComboBoxTypeTableUI> ListTypeTables { get; } = new ObservableCollection<ComboBoxTypeTableUI>();
        public IAsyncRelayCommand LoadTablesCommand { get; }
        public int _page = 0;
        public int _count = -1;

        public TableViewModel(ISrvTableService tableService, ISrvEventService eventService)
        {
            _tableService = tableService;
            LoadTablesCommand = new AsyncRelayCommand(LoadTablesAsync);
            _loadFast = SrvAppConfigurationStorage.DatabaseConfiguration.LoadFast;
            _eventService = eventService;
        }

        public async Task LoadTablesAsync()
        {
            var tablesTemp = await FilterTablesAsync();
            RecordsPagination(tablesTemp);
        }

        private void BindGrid(IEnumerable<SrvInformationTable> tablesTemp)
        {
            Tables.Clear();

            foreach (var x in tablesTemp)
            {
                Tables.Add(x);
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

            if (SrvAppConfigurationStorage.EventLiveDefault)
            {
                SelectedStatusEvent = ListStatusEvents.FirstOrDefault(x => x.Id == 1);
            }
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

        public async Task<IEnumerable<SrvInformationTable>> FilterTablesAsync()
        {
            IEnumerable<SrvInformationTable> tablesTemp = new List<SrvInformationTable>();

            if (_loadFast)
            {
                if (!_tablesList.Any())
                {
                    _tablesList = await _tableService.GetAllInformationTablesAsync();
                }

                var predicate = CreatePredicate();
                tablesTemp = _tablesList.Where(predicate).ToList();
                _count = tablesTemp.Count();
            }
            else
            {
                tablesTemp = await _tableService.GetInformationTablesAsync(EventName, GdrName, Master, EventStatus, TableType, _page);
                _count = await _tableService.CountInformationTablesAsync(EventName, GdrName, Master, EventStatus, TableType);
            }

            TotalRecordsText = $"Records: {_count}";
            PageText = $"Pagina: {_page + 1} di {(_count / 50) + 1}";

            return tablesTemp;
        }

        private Func<SrvInformationTable, bool> CreatePredicate()
        {
            return table => (string.IsNullOrWhiteSpace(EventName) ||
                             table.EventName != null && table.EventName.ToLower().Contains(EventName.ToLower(), StringComparison.OrdinalIgnoreCase)) &&
                            (string.IsNullOrWhiteSpace(GdrName) ||
                             table.GdrName != null && table.GdrName.ToLower().Contains(GdrName.ToLower(), StringComparison.OrdinalIgnoreCase)) &&
            (string.IsNullOrWhiteSpace(Master) ||
                             table.Master != null && table.Master.ToLower().Contains(Master.ToLower(), StringComparison.OrdinalIgnoreCase)) &&
            (!EventStatus.HasValue ||
                             table.EventStatus == EventStatus) &&
            (!TableType.HasValue ||
                             table.TableType == TableType);
        }

        public async Task<int> TablesCountAsync()
        {
            if (_count == -1)
            {
                _count = await _tableService.CountAllInformationTablesAsync();
            }

            return _count;
        }

        public async Task PrevButton()
        {
            if (_page > 0)
            {
                _page--;

                var tablesTemp = await FilterTablesAsync();
                RecordsPagination(tablesTemp);
            }
        }

        public async Task NextButton()
        {
            if (_count - (50 + (_page * 50)) > 0)
            {
                _page++;

                var tablesTemp = await FilterTablesAsync();
                RecordsPagination(tablesTemp);
            }
        }

        public async Task DataSortAsync(string propertyName, bool isAscending)
        {
            var tablesTemp = await FilterTablesAsync();

            switch (propertyName)
            {
                case "EventName":
                    tablesTemp = isAscending
                        ? tablesTemp.OrderBy(x => x.EventName).ToList()
                        : tablesTemp.OrderByDescending(x => x.EventName).ToList();
                    break;
                case "EventStatus":
                    tablesTemp = isAscending
                        ? tablesTemp.OrderBy(x => x.EventStatus).ToList()
                        : tablesTemp.OrderByDescending(x => x.EventStatus).ToList();
                    break;
                case "GdrName":
                    tablesTemp = isAscending
                        ? tablesTemp.OrderBy(x => x.GdrName).ToList()
                        : tablesTemp.OrderByDescending(x => x.GdrName).ToList();
                    break;
                case "Capacity":
                    tablesTemp = isAscending
                        ? tablesTemp.OrderBy(x => x.Capacity).ToList()
                        : tablesTemp.OrderByDescending(x => x.Capacity).ToList();
                    break;
                case "QuantitySold":
                    tablesTemp = isAscending
                        ? tablesTemp.OrderBy(x => x.QuantitySold).ToList()
                        : tablesTemp.OrderByDescending(x => x.QuantitySold).ToList();
                    break;
                case "StartDate":
                    tablesTemp = isAscending
                        ? tablesTemp.OrderBy(x => x.StartDate).ToList()
                        : tablesTemp.OrderByDescending(x => x.StartDate).ToList();
                    break;
                case "EndDate":
                    tablesTemp = isAscending
                        ? tablesTemp.OrderBy(x => x.EndDate).ToList()
                        : tablesTemp.OrderByDescending(x => x.EndDate).ToList();
                    break;
                case "Master":
                    tablesTemp = isAscending
                        ? tablesTemp.OrderBy(x => x.Master).ToList()
                        : tablesTemp.OrderByDescending(x => x.Master).ToList();
                    break;
                case "TableType":
                    tablesTemp = isAscending
                        ? tablesTemp.OrderBy(x => x.TableType).ToList()
                        : tablesTemp.OrderByDescending(x => x.TableType).ToList();
                    break;
                default:
                    break;
            }

            RecordsPagination(tablesTemp);
        }

        private void RecordsPagination(IEnumerable<SrvInformationTable> tablesTemp)
        {
            int skip = _page * 50;
            tablesTemp = tablesTemp.Skip(skip).Take(50);

            BindGrid(tablesTemp);
        }

        public async Task UpdateInformationTable(SrvInformationTable table)
        {
            await _tableService.UpdateInformationTableAsync(table);
        }
    }
}
