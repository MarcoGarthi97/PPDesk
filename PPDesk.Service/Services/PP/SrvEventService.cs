using AutoMapper;
using Microsoft.Extensions.Logging;
using PPDesk.Abstraction.DTO.Repository;
using PPDesk.Abstraction.DTO.Service.Eventbrite;
using PPDesk.Abstraction.DTO.Service.PP;
using PPDesk.Abstraction.Helper;
using PPDesk.Repository.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPDesk.Service.Services.PP
{
    public interface ISrvEventService : IForServiceCollectionExtension
    {
        Task CreateTableEventsAsync();
        Task DeleteAllEvents();
        Task<IEnumerable<SrvEvent>> GetEventsAsync(int page, int limit = 50);
        IEnumerable<SrvEvent> GetEventsByEEvents(IEnumerable<SrvEEvent> eEvents);
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

        public IEnumerable<SrvEvent> GetEventsByEEvents(IEnumerable<SrvEEvent> eEvents)
        {
            return _mapper.Map<IEnumerable<SrvEvent>>(eEvents);
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
