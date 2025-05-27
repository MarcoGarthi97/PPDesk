using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PPDesk.Abstraction.DTO.Service.PP.Event;
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
    public class EventViewModel : ObservableObject, IForServiceCollectionExtension
    {
        private readonly ISrvEventService _eventService;
        private IEnumerable<SrvInformationEvent> _eventsList = new List<SrvInformationEvent>();
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
        public string? EventName;
        public EnumEventStatus? EventStatus;
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
        public ObservableCollection<SrvInformationEvent> Events { get; } = new ObservableCollection<SrvInformationEvent>();
        public ObservableCollection<ComboBoxEventUI> ListEvents { get; } = new ObservableCollection<ComboBoxEventUI>();
        public ObservableCollection<ComboBoxStatusEventUI> ListStatusEvents { get; } = new ObservableCollection<ComboBoxStatusEventUI>();
        public IAsyncRelayCommand LoadEventsCommand { get; }
        public int _page = 0;
        public int _count = -1;

        public EventViewModel(ISrvEventService eventService)
        {
            _eventService = eventService;
            LoadEventsCommand = new AsyncRelayCommand(LoadEventsAsync);
            _loadFast = SrvAppConfigurationStorage.DatabaseConfiguration.LoadFast;

            InitializeComboBoxesAsync().Wait();
        }

        public async Task LoadEventsAsync()
        {
            var eventsTemp = await FilterEventsAsync();
            RecordsPagination(eventsTemp);
        }

        private void BindGrid(IEnumerable<SrvInformationEvent> eventsTemp)
        {
            Events.Clear();

            foreach (var x in eventsTemp)
            {
                Events.Add(x);
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

        public async Task<IEnumerable<SrvInformationEvent>> FilterEventsAsync()
        {
            IEnumerable<SrvInformationEvent> eventsTemp = new List<SrvInformationEvent>();

            if (_loadFast)
            {
                if (!_eventsList.Any())
                {
                    _eventsList = await _eventService.GetAllInformationEventsAsync();
                }

                var predicate = CreatePredicate();
                eventsTemp = _eventsList.Where(predicate).ToList();
                _count = eventsTemp.Count();
            }
            else
            {
                eventsTemp = await _eventService.GetInformationEventsAsync(EventName, EventStatus, _page);
                _count = await _eventService.CountInformationEventsAsync(EventName, EventStatus);
            }

            TotalRecordsText = $"Records: {_count}";
            PageText = $"Pagina: {_page + 1} di {(_count / 50) + 1}";

            return eventsTemp;
        }

        private Func<SrvInformationEvent, bool> CreatePredicate()
        {
            return e => (string.IsNullOrWhiteSpace(EventName) ||
                             e.Name != null && e.Name.ToLower().Contains(EventName.ToLower(), StringComparison.OrdinalIgnoreCase)) &&
            (!EventStatus.HasValue ||
                             e.Status == EventStatus);
        }

        public async Task<int> EventsCountAsync()
        {
            if (_count == -1)
            {
                _count = await _eventService.CountAllInformationEventsAsync();
            }

            return _count;
        }

        public async Task PrevButton()
        {
            if (_page > 0)
            {
                _page--;

                var eventsTemp = await FilterEventsAsync();
                RecordsPagination(eventsTemp);
            }
        }

        public async Task NextButton()
        {
            if (_count - (50 + (_page * 50)) > 0)
            {
                _page++;

                var eventsTemp = await FilterEventsAsync();
                RecordsPagination(eventsTemp);
            }
        }

        public async Task DataSortAsync(string propertyName, bool isAscending)
        {
            var eventsTemp = await FilterEventsAsync();

            switch (propertyName)
            {
                case "Name":
                    eventsTemp = isAscending
                        ? eventsTemp.OrderBy(x => x.Name).ToList()
                        : eventsTemp.OrderByDescending(x => x.Name).ToList();
                    break;
                case "Status":
                    eventsTemp = isAscending
                        ? eventsTemp.OrderBy(x => x.Status).ToList()
                        : eventsTemp.OrderByDescending(x => x.Status).ToList();
                    break;
                case "Start":
                    eventsTemp = isAscending
                        ? eventsTemp.OrderBy(x => x.Start).ToList()
                        : eventsTemp.OrderByDescending(x => x.Start).ToList();
                    break;
                case "End":
                    eventsTemp = isAscending
                        ? eventsTemp.OrderBy(x => x.End).ToList()
                        : eventsTemp.OrderByDescending(x => x.End).ToList();
                    break;
                case "TotalUsers":
                    eventsTemp = isAscending
                        ? eventsTemp.OrderBy(x => x.TotalUsers).ToList()
                        : eventsTemp.OrderByDescending(x => x.TotalUsers).ToList();
                    break;
                case "TotalTicket":
                    eventsTemp = isAscending
                        ? eventsTemp.OrderBy(x => x.TotalTicket).ToList()
                        : eventsTemp.OrderByDescending(x => x.TotalTicket).ToList();
                    break;
                default:
                    break;
            }

            RecordsPagination(eventsTemp);
        }

        private void RecordsPagination(IEnumerable<SrvInformationEvent> eventsTemp)
        {
            int skip = _page * 50;
            eventsTemp = eventsTemp.Skip(skip).Take(50);

            BindGrid(eventsTemp);
        }
    }
}
