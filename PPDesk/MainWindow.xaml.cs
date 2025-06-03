using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using PPDesk.Abstraction.DTO.Service.Eventbrite;
using PPDesk.Abstraction.DTO.Service.PP;
using PPDesk.Abstraction.Helper;
using PPDesk.Pages;
using PPDesk.Service.Services.Eventbrite;
using PPDesk.Service.Services.PP;
using PPDesk.Service.Services.Window;
using PPDesk.Service.Storages.Eventbride;
using PPDesk.Service.Storages.PP;
using RestSharp;
using Windows.Foundation;
using Windows.Foundation.Collections;

namespace PPDesk
{
    public sealed partial class MainWindow : Window, IForServiceCollectionExtension
    {
        private readonly ISrvMainWindowService _service;
        private readonly IServiceProvider _serviceProvider;
        private IConfiguration _configuration;
        private bool _isWindowLoaded = false;

        public MainWindow(ISrvMainWindowService service, IServiceProvider serviceProvider)
        {
            this.InitializeComponent();
            _service = service;
            _serviceProvider = serviceProvider;

            // Aspetta che la finestra sia completamente caricata
            this.Activated += MainWindow_Activated;
        }

        private async void MainWindow_Activated(object sender, WindowActivatedEventArgs args)
        {
            if (!_isWindowLoaded)
            {
                _isWindowLoaded = true;
                this.Activated -= MainWindow_Activated; // Rimuovi l'event handler

                // Ora carica le configurazioni
                var load = await LoadConfigurationsAsync();
                if (load)
                {
                    LoadDatabaseAsync();
                    var tablesPage = _serviceProvider.GetRequiredService<TablesPage>();
                    ContentFrame.Content = tablesPage;
                }
            }
        }

        private async Task<bool> LoadConfigurationsAsync()
        {
            try
            {
                var scope = _serviceProvider.CreateScope();
                _configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>()!;

                var apikey = _configuration.GetSection("App:EventbriteApiKey").Get<SrvEApiKey>();
                SrvEApiKeyStorage.SetpiKeyStorage(apikey);
                SrvETokenStorage.SetBearer(SrvEApiKeyStorage.Configuration.PrivateToken);

                var databaseConfiguration = _configuration.GetSection("App:Database").Get<SrvDatabaseConfiguration>();
                SrvAppConfigurationStorage.SetDatabaseConfigurations(databaseConfiguration);

                return true;
            }
            catch (Exception ex)
            {
                await ShowErrorAlertAsync(ex);
                return false;
            }
        }

        private async Task ShowErrorAlertAsync(Exception exception)
        {
            var dialog = new ContentDialog
            {
                Title = "Errore",
                Content = exception.Message,
                CloseButtonText = "OK"
            };

            // Retry per ottenere XamlRoot se necessario
            XamlRoot xamlRoot = null;
            for (int i = 0; i < 10; i++)
            {
                if (this.Content is FrameworkElement rootElement && rootElement.XamlRoot != null)
                {
                    xamlRoot = rootElement.XamlRoot;
                    break;
                }
                await Task.Delay(50);
            }

            if (xamlRoot != null)
            {
                dialog.XamlRoot = xamlRoot;
                try
                {
                    await dialog.ShowAsync();
                }
                catch (Exception ex)
                {
                    // Log error if needed
                    Debug.WriteLine($"Errore durante la visualizzazione dell'alert: {ex.Message}");
                }
            }
            else
            {
                // Fallback: usa un MessageBox di sistema
                Debug.WriteLine($"Errore (XamlRoot non disponibile): {exception.Message}");
                // Oppure se hai riferimenti a System.Windows.Forms:
                // System.Windows.Forms.MessageBox.Show(exception.Message, "Errore");
            }
        }

        private async void LoadDatabaseAsync()
        {
            var scope = _serviceProvider.CreateScope();
            var databaseService = scope.ServiceProvider.GetRequiredService<ISrvDatabaseService>();

            await databaseService.LoadDatabaseExists();
        }

        private void NavView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (args.InvokedItemContainer is NavigationViewItem item &&
                item.Tag is string tag)
            {
                switch (tag)
                {
                    case "tablesPage":
                        var tablesPage = _serviceProvider.GetRequiredService<TablesPage>();
                        ContentFrame.Content = tablesPage;
                        break;
                    case "usersPage":
                        var usersPage = _serviceProvider.GetRequiredService<UsersPage>();
                        ContentFrame.Content = usersPage;
                        break;
                    case "eventsPage":
                        var eventsPage = _serviceProvider.GetRequiredService<EventsPage>();
                        ContentFrame.Content = eventsPage;
                        break;
                    case "ordersPage":
                        var ordersPage = _serviceProvider.GetRequiredService<OrdersPage>();
                        ContentFrame.Content = ordersPage;
                        break;
                    case "settingsPage":
                        var settingsPage = _serviceProvider.GetRequiredService<SettingsPage>();
                        ContentFrame.Content = settingsPage;
                        break;
                }
            }
        }
    }
}