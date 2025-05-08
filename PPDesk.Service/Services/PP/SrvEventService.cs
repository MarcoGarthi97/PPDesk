using AutoMapper;
using PPDesk.Abstraction.Helper;
using PPDesk.Repository.Repositories;
using System.Threading.Tasks;

namespace PPDesk.Service.Services.PP
{
    public interface ISrvEventService : IForServiceCollectionExtension
    {
        Task CreateTableEventsAsync();
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
    }
}
