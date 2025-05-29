using AutoMapper;
using Microsoft.Extensions.Logging;
using PPDesk.Abstraction.DTO.Repository.Event;
using PPDesk.Abstraction.DTO.Service.Eventbrite;
using PPDesk.Abstraction.DTO.Service.PP.Event;
using PPDesk.Abstraction.DTO.UI;
using PPDesk.Abstraction.Enum;
using PPDesk.Abstraction.Helper;
using PPDesk.Repository.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPDesk.Service.Services.PP
{
    public interface ISrvEventService : IForServiceCollectionExtension
    {
        Task<int> CountAllInformationEventsAsync();
        Task<int> CountInformationEventsAsync(string name, EnumEventStatus? status);
        Task CreateTableEventsAsync();
        Task DeleteAllEvents();
        Task<IEnumerable<SrvInformationEvent>> GetAllInformationEventsAsync();
        Task<IEnumerable<ComboBoxEventUI>> GetComboBoxEventsUI();
        Task<IEnumerable<SrvEvent>> GetEventsAsync(int page, int limit = 50);
        IEnumerable<SrvEvent> GetEventsByEEvents(IEnumerable<SrvEEvent> eEvents);
        Task<IEnumerable<SrvInformationEvent>> GetInformationEventsAsync(string name, EnumEventStatus? status, int page, int limit = 50);
        Task InsertEventsAsync(IEnumerable<SrvEvent> srvEvents);
        Task UpdateEventsAsync(IEnumerable<SrvEvent> srvEvents);
    }

    public class SrvEventService : ISrvEventService
    {
        private readonly IMdlEventRepository _eventRepository;
        private readonly IMapper _mapper;

        public SrvEventService(IMdlEventRepository eventRepository, IMapper mapper)
        {
            _eventRepository = eventRepository;
            _mapper = mapper;
        }

        public async Task CreateTableEventsAsync()
        {
            await _eventRepository.CreateTableEventsAsync();
        }

        public async Task<IEnumerable<SrvEvent>> GetEventsAsync(int page, int limit = 50)
        {
            var mdlEvents = await _eventRepository.GetEventsAsync(page, limit);
            return _mapper.Map<IEnumerable<SrvEvent>>(mdlEvents);
        }

        public async Task<IEnumerable<ComboBoxEventUI>> GetComboBoxEventsUI()
        {
            var events = await GetEventsAsync(0);
            return _mapper.Map<IEnumerable<ComboBoxEventUI>>(events);
        }

        public IEnumerable<SrvEvent> GetEventsByEEvents(IEnumerable<SrvEEvent> eEvents)
        {
            return _mapper.Map<IEnumerable<SrvEvent>>(eEvents);
        }

        public async Task<IEnumerable<SrvInformationEvent>> GetInformationEventsAsync(string name, EnumEventStatus? status, int page, int limit = 50)
        {
            var mdlEvents = await _eventRepository.GetInformationEventsAsync(name, status, page, limit);
            return _mapper.Map<IEnumerable<SrvInformationEvent>>(mdlEvents);
        }

        public async Task<IEnumerable<SrvInformationEvent>> GetAllInformationEventsAsync()
        {
            var mdlEvents = await _eventRepository.GetAllInformationEventsAsync();
            return _mapper.Map<IEnumerable<SrvInformationEvent>>(mdlEvents);
        }

        public async Task<int> CountInformationEventsAsync(string name, EnumEventStatus? status)
        {
            return await _eventRepository.CountInformationEventsAsync(name, status);
        }

        public async Task<int> CountAllInformationEventsAsync()
        {
            return await _eventRepository.CountAllInformationEventsAsync();
        }

        public async Task InsertEventsAsync(IEnumerable<SrvEvent> srvEvents)
        {
            var mdlEvents = _mapper.Map<IEnumerable<MdlEvent>>(srvEvents);
            await _eventRepository.InsertEventsAsync(mdlEvents);
        }

        public async Task UpdateEventsAsync(IEnumerable<SrvEvent> srvEvents)
        {
            var mdlEvents = _mapper.Map<IEnumerable<MdlEvent>>(srvEvents);
            await _eventRepository.UpdateEventsAsync(mdlEvents);
        }

        public async Task DeleteAllEvents()
        {
            await _eventRepository.DeleteAllEventsAsync();
        }
    }
}
