using AutoMapper;
using PPDesk.Abstraction.Helper;
using PPDesk.Repository.Repositories;
using System.Threading.Tasks;

namespace PPDesk.Service.Services.PP
{
    public interface ISrvTableService : IForServiceCollectionExtension
    {
        Task CreateTableTablesAsync();
    }

    public class SrvTableService : ISrvTableService
    {
        private readonly IMdlTableRepository _tableRepository;
        private readonly IMapper _mapper;

        public SrvTableService(IMdlTableRepository tableRepository, IMapper mapper)
        {
            _tableRepository = tableRepository;
            _mapper = mapper;
        }

        public async Task CreateTableTablesAsync()
        {
            await _tableRepository.CreateTableTablesAsync();
        }
    }
}
