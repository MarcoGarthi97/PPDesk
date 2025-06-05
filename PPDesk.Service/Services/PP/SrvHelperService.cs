using AutoMapper;
using PPDesk.Abstraction.DTO.Repository;
using PPDesk.Abstraction.DTO.Service.Eventbrite;
using PPDesk.Abstraction.DTO.Service.PP;
using PPDesk.Abstraction.Helper;
using PPDesk.Repository.Repositories;
using PPDesk.Service.Storages.Eventbride;
using PPDesk.Service.Storages.PP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PPDesk.Service.Services.PP
{
    public interface ISrvHelperService : IForServiceCollectionExtension
    {
        Task LoadConfigurationAsync();
        Task LoadApiKeyEventbrideAsync();
        Task LoadDatabaseConfiguration();
        Task InsertHelperAsync(IEnumerable<SrvHelper> helpers);
        Task UpdateHelperAsync(IEnumerable<SrvHelper> helpers);
        Task InsertHelperAsync(SrvHelper helper);
        Task UpdateHelperAsync(SrvHelper helper);
        Task CreateTableHelpersAsync();
        Task DeleteAllHelpersAsync();
    }

    public class SrvHelperService : ISrvHelperService
    {
        private readonly IMdlHelperRepository _helperRepository;
        private readonly IMapper _mapper;

        public SrvHelperService(IMdlHelperRepository helperRepository, IMapper mapper)
        {
            _helperRepository = helperRepository;
            _mapper = mapper;
        }

        public async Task CreateTableHelpersAsync()
        {
            await _helperRepository.CreateTableHelpersAsync();
        }

        public async Task LoadConfigurationAsync()
        {
            await LoadApiKeyEventbrideAsync();
            await LoadDatabaseConfiguration();
        }

        public async Task LoadApiKeyEventbrideAsync()
        {
            var helper = await _helperRepository.GetHelperByKeyAsync("EventbrideApiKey");
            if(helper != null)
            {
                var eventbrideApiKey = JsonSerializer.Deserialize<SrvEApiKey>(helper.Json);

                SrvEApiKeyStorage.SetpiKeyStorage(eventbrideApiKey);
            }
        }

        public async Task LoadDatabaseConfiguration()
        {
            var helper = await _helperRepository.GetHelperByKeyAsync("DatabaseConfiguration");
            if (helper != null)
            {
                var databaseConfiguration = JsonSerializer.Deserialize<SrvDatabaseConfigurationBySQL>(helper.Json);

                SrvAppConfigurationStorage.SetDatabaseConfigurations(databaseConfiguration);
            }
        }

        public async Task InsertHelperAsync(SrvHelper helper)
        {
            await InsertHelperAsync(new List<SrvHelper> { helper });
        }

        public async Task InsertHelperAsync(IEnumerable<SrvHelper> helpers)
        {
            var mdlHelpers = _mapper.Map<IEnumerable<MdlHelper>>(helpers);
            await _helperRepository.InsertHelperAsync(mdlHelpers);
        }

        public async Task UpdateHelperAsync(SrvHelper helper)
        {
            await UpdateHelperAsync(new List<SrvHelper> { helper });
        }

        public async Task UpdateHelperAsync(IEnumerable<SrvHelper> helpers)
        {
            var mdlHelpers = _mapper.Map<IEnumerable<MdlHelper>>(helpers);
            await _helperRepository.UpdateHelperAsync(mdlHelpers);
        }

        public async Task DeleteAllHelpersAsync()
        {
            await _helperRepository.DeleteAllHelpersAsync();
        }
    }
}
