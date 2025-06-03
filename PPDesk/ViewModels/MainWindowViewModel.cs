using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;
using PPDesk.Abstraction.DTO.Service.Eventbrite;
using PPDesk.Abstraction.DTO.Service.PP;
using PPDesk.Abstraction.Helper;
using PPDesk.Service.Services.PP;
using PPDesk.Service.Storages.Eventbride;
using PPDesk.Service.Storages.PP;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPDesk.ViewModels
{
    public class MainWindowViewModel : ObservableObject, IForServiceCollectionExtension
    {
        private readonly IServiceProvider _serviceProvider;

        private IConfiguration _configuration;

        public MainWindowViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<bool> LoadConfigurations()
        {
            try
            {
                var scope = _serviceProvider.CreateScope();
                _configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>()!;

                LoadAppsettings();
                LoadDatabaseAsync();

                return true;
            }
            catch (Exception ex)
            {
                ShowErrorAlertAsync(ex);

                return false;
            }
        }

        private void LoadAppsettings()
        {
            var apikey = _configuration.GetSection("App:EventbriteApiKey").Get<SrvEApiKey>();
            SrvEApiKeyStorage.SetpiKeyStorage(apikey);
            SrvETokenStorage.SetBearer(SrvEApiKeyStorage.Configuration.PrivateToken);

            var databaseConfiguration = _configuration.GetSection("App:Database").Get<SrvDatabaseConfiguration>();
            SrvAppConfigurationStorage.SetDatabaseConfigurations(databaseConfiguration);
        }

        private async void LoadDatabaseAsync()
        {
            var scope = _serviceProvider.CreateScope();
            var databaseService = scope.ServiceProvider.GetRequiredService<ISrvDatabaseService>();

            await databaseService.LoadDatabaseExists();
        }

        private async void ShowErrorAlertAsync(Exception exception)
        {
            var dialog = new ContentDialog
            {
                Title = "Errore",
                Content = exception.Message,
                CloseButtonText = "OK",
                
            };

            try
            {
                await dialog.ShowAsync();
                //_logger.LogWarning("Database non trovato - alert mostrato all'utente");
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "Errore durante la visualizzazione dell'alert");
            }
        }
    }
}
