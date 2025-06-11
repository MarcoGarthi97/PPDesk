using Microsoft.Extensions.Logging;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using PPDesk.Abstraction.Helper;
using PPDesk.Service.BackgroundServices;
using PPDesk.Service.Services.PP;
using PPDesk.Service.Storages.PP;
using PPDesk.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace PPDesk.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingsPage : Page, IForServiceCollectionExtension
    {
        private readonly ILogger<SettingsPage> _logger;

        private readonly ISrvDatabaseService _databaseService;
        private readonly ISrvEUpdateLiveBackgroundService _updateLiveBackgroundService;
        private readonly ISrvEUpdateBackgroundService _updateBackgroundService;

        public SettingsPage(SettingViewModel settingViewModel, ILogger<SettingsPage> logger, ISrvDatabaseService databaseService, ISrvEUpdateLiveBackgroundService updateLiveBackgroundService, ISrvEUpdateBackgroundService updateBackgroundService)
        {
            _logger = logger;
            _databaseService = databaseService;
            this.DataContext = settingViewModel;

            this.InitializeComponent();
            LoadComponents();
            _updateLiveBackgroundService = updateLiveBackgroundService;
            _updateBackgroundService = updateBackgroundService;
        }

        private void LoadComponents()
        {
            try
            {
                var settingViewModel = (SettingViewModel)DataContext;
                settingViewModel.LoadApiKey();
                settingViewModel.LoadDatabaseConfigurations();
                settingViewModel.LoadLiveEventDefault();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }

        private async void EventbrideButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var settingViewModel = (SettingViewModel)DataContext;
                await settingViewModel.SaveApiKeyAsync();

                DialogAsync("Salvataggio", "Salvataggio avvenuto correttamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }

        private async void DialogAsync(string title, string message)
        {
            var optionsDialog = new ContentDialog
            {
                Title = title,
                Content = message,
                CloseButtonText = "Ok",
                XamlRoot = this.XamlRoot
            };

            await optionsDialog.ShowAsync();
        }

        private async void LoadDatabaseButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var optionsDialog = new ContentDialog
                {
                    Title = "Database",
                    Content = "Creare tabelle nel database",
                    PrimaryButtonText = "Ok",
                    CloseButtonText = "Annulla",
                    XamlRoot = this.XamlRoot
                };

                var result = await optionsDialog.ShowAsync();

                if (result == ContentDialogResult.Primary)
                {
                    await ShowDatabaseCreationProgressAsync();

                    await StartBackgroundServiceAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }

        private async Task StartBackgroundServiceAsync()
        {
            try
            {
                await _updateLiveBackgroundService.StartUpdateAsync();
                await _updateBackgroundService.StartUpdateAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }

        private async void LoadDataDatabaseButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var optionsDialog = new ContentDialog
                {
                    Title = "Database",
                    Content = "Caricamento dati di Eventbride sul database",
                    PrimaryButtonText = "Ok",
                    CloseButtonText = "Annulla",
                    XamlRoot = this.XamlRoot
                };

                var result = await optionsDialog.ShowAsync();

                if (result == ContentDialogResult.Primary)
                {
                    if (SrvAppConfigurationStorage.DatabaseConfiguration.DatabaseExists)
                    {
                        await ShowDataLoadingProgressAsync();
                    }
                    else
                    {
                        DialogAsync("Attenzione", "Database non ancora creato.");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }

        private async Task ShowDataLoadingProgressAsync()
        {
            var progressDialog = new ContentDialog
            {
                Title = "Caricamento Dati Database",
                CloseButtonText = null,
                XamlRoot = this.XamlRoot
            };

            var stackPanel = new StackPanel
            {
                Spacing = 15,
                Margin = new Thickness(20)
            };

            var statusText = new TextBlock
            {
                Text = "Inizializzazione caricamento dati...",
                HorizontalAlignment = HorizontalAlignment.Center,
                FontSize = 14
            };

            var progressBar = new ProgressBar
            {
                IsIndeterminate = true,
                Width = 300
            };

            stackPanel.Children.Add(statusText);
            stackPanel.Children.Add(progressBar);

            progressDialog.Content = stackPanel;

            var progress = new Progress<string>(message =>
            {
                DispatcherQueue.TryEnqueue(() =>
                {
                    statusText.Text = message;
                });
            });

            // Avvia l'operazione in background
            var loadingTask = Task.Run(async () =>
            {
                try
                {
                    await _databaseService.LoadAllDataAsync(progress);

                    DispatcherQueue.TryEnqueue(() =>
                    {
                        statusText.Text = "Caricamento dati completato!";
                        progressBar.IsIndeterminate = false;
                        progressBar.Value = 100;
                        progressDialog.CloseButtonText = "OK";
                    });
                }
                catch (Exception ex)
                {
                    DispatcherQueue.TryEnqueue(() =>
                    {
                        statusText.Text = $"Errore nel caricamento: {ex.Message}";
                        progressBar.IsIndeterminate = false;
                        progressDialog.CloseButtonText = "Chiudi";
                    });
                }
            });

            // Mostra il dialogo e aspetta che l'utente lo chiuda
            await progressDialog.ShowAsync();

            // Assicurati che l'operazione sia completata
            await loadingTask;
        }

        private async Task ShowDatabaseCreationProgressAsync()
        {
            var progressDialog = new ContentDialog
            {
                Title = "Creazione Database",
                CloseButtonText = null, // Nasconde il pulsante di chiusura durante l'operazione
                XamlRoot = this.XamlRoot
            };

            // Crea il contenuto del dialogo con ProgressBar e TextBlock
            var stackPanel = new StackPanel
            {
                Spacing = 15,
                Margin = new Thickness(20)
            };

            var statusText = new TextBlock
            {
                Text = "Inizializzazione...",
                HorizontalAlignment = HorizontalAlignment.Center,
                FontSize = 14
            };

            var progressBar = new ProgressBar
            {
                IsIndeterminate = true,
                Width = 300
            };

            stackPanel.Children.Add(statusText);
            stackPanel.Children.Add(progressBar);

            progressDialog.Content = stackPanel;

            // Progress reporter per aggiornare il testo
            var progress = new Progress<string>(message =>
            {
                DispatcherQueue.TryEnqueue(() =>
                {
                    statusText.Text = message;
                });
            });

            // Avvia l'operazione in background mentre mostra il dialogo
            var databaseTask = Task.Run(async () =>
            {
                try
                {
                    await _databaseService.CreateTablesAsync(progress);

                    // Aggiorna UI finale
                    DispatcherQueue.TryEnqueue(() =>
                    {
                        statusText.Text = "Operazione completata!";
                        progressBar.IsIndeterminate = false;
                        progressBar.Value = 100;
                        progressDialog.CloseButtonText = "OK";
                    });
                }
                catch (Exception ex)
                {
                    DispatcherQueue.TryEnqueue(() =>
                    {
                        statusText.Text = $"Errore: {ex.Message}";
                        progressBar.IsIndeterminate = false;
                        progressDialog.CloseButtonText = "Chiudi";
                    });
                }
            });

            // Mostra il dialogo e aspetta che l'utente lo chiuda
            await progressDialog.ShowAsync();

            // Assicurati che l'operazione sia completata prima di continuare
            await databaseTask;
        }

        private void MyToggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            try
            {
                ToggleSwitch toggleSwitch = sender as ToggleSwitch;
                if (toggleSwitch != null)
                {
                    SrvAppConfigurationStorage.SetEventLiveDefault(toggleSwitch.IsOn);
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }
    }

}
