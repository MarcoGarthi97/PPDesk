using AutoMapper;
using PPDesk.Abstraction.Helper;
using PPDesk.Repository.Repositories;
using System.Threading.Tasks;

namespace PPDesk.Service.Services.PP
{
    public interface ISrvVersionService : IForServiceCollectionExtension
    {
        Task CreateTableVersionAsync();
        Task<string> GetVersionAsync();
        Task InsertVersionAsync(string version = "1.0.0");
    }

    public class SrvVersionService : ISrvVersionService
    {
        private readonly IMdlVersionRepository _versionRepository;
        private readonly IMapper _mapper;

        public SrvVersionService(IMdlVersionRepository versionRepository, IMapper mapper)
        {
            _versionRepository = versionRepository;
            _mapper = mapper;
        }

        public async Task CreateTableVersionAsync()
        {
            await _versionRepository.CreateTableVersionAsync();
        }

        public async Task<string> GetVersionAsync()
        {
            return await _versionRepository.GetVersionAsync();
        }

        public async Task InsertVersionAsync(string version = "1.0.0")
        {
            await _versionRepository.InsertVersionAsync(version);
        }
    }
}
