using Microsoft.Extensions.Hosting;
using PPDesk.Abstraction.Helper;
using PPDesk.Service.BackgroundServices;
using PPDesk.Service.Storages.PP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PPDesk.Service.Services.PP
{
    public interface ISrvBackgroundServiceOrchestratorService : IForServiceCollectionExtension
    {
        Task StartAllBackgrounServicesAsync();
        Task StartSrvEUpdateLiveBackgroundServiceAsync();
    }

    public class SrvBackgroundServiceOrchestratorService : ISrvBackgroundServiceOrchestratorService
    {
        private readonly IHostedService _hostedService;

        public SrvBackgroundServiceOrchestratorService(IHostedService hostedService)
        {
            _hostedService = hostedService;
        }

        public async Task StartAllBackgrounServicesAsync()
        {
            await StartSrvEUpdateLiveBackgroundServiceAsync();
        }

        public async Task StartSrvEUpdateLiveBackgroundServiceAsync()
        {
            try
            {
                if (_hostedService is SrvEUpdateLiveBackgroundService backgroundService && SrvAppConfigurationStorage.LiveBackgroundServiceConfiguration != null)
                {
                    await backgroundService.StartAsync(CancellationToken.None);
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
