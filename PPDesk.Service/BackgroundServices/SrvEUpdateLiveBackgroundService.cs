using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PPDesk.Abstraction.Helper;
using PPDesk.Service.Services.PP;
using PPDesk.Service.Storages.PP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PPDesk.Service.BackgroundServices
{
    public interface ISrvEUpdateLiveBackgroundService : IForServiceCollectionExtension
    {
        Task StartUpdateAsync();
        void StopManualUpdate();
    }

    public class SrvEUpdateLiveBackgroundService : BackgroundService, ISrvEUpdateLiveBackgroundService
    {
        private readonly ISrvDatabaseService _databaseService;
        private readonly ILogger<SrvEUpdateLiveBackgroundService> _logger;
        private TimeSpan _updateInterval = SrvAppConfigurationStorage.LiveBackgroundServiceConfiguration != null ? TimeSpan.FromMinutes(SrvAppConfigurationStorage.LiveBackgroundServiceConfiguration.MinutesInterval) : TimeSpan.FromMinutes(10);
        private CancellationTokenSource _manualCancellationTokenSource;
        private Task _manualTask;

        public SrvEUpdateLiveBackgroundService(ISrvDatabaseService databaseService, ILogger<SrvEUpdateLiveBackgroundService> logger)
        {
            _databaseService = databaseService;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                _logger.LogInformation("Start ExecuteAsync");

                using var timer = new PeriodicTimer(_updateInterval);
                while(!stoppingToken.IsCancellationRequested && await timer.WaitForNextTickAsync(stoppingToken))
                {
                    await UpdateLiveDataAsync();
                }

                _logger.LogInformation("End ExecuteAsync");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }

        private async Task UpdateLiveDataAsync()
        {
            try
            {
                await _databaseService.UpdateLiveDataAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopping SrvEUpdateLiveBackgroundService");
            await base.StopAsync(cancellationToken);
        }

        public async Task StartUpdateAsync()
        {
            if (_manualTask == null || _manualTask.IsCompleted)
            {
                _manualCancellationTokenSource = new CancellationTokenSource();
                _manualTask = ExecuteAsync(_manualCancellationTokenSource.Token);
                _logger.LogInformation("Manual background service started");
            }
        }

        public void StopManualUpdate()
        {
            if (_manualCancellationTokenSource != null)
            {
                _manualCancellationTokenSource.Cancel();
                _manualCancellationTokenSource.Dispose();
                _manualCancellationTokenSource = null;
                _logger.LogInformation("Manual background service stopped");
            }
        }
    }
}
