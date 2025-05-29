using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using CommunityToolkit.WinUI.UI.Controls;
using PPDesk.ViewModels;
using Windows.System;
using PPDesk.Abstraction.Helper;
using Microsoft.Extensions.Logging;
using PPDesk.Service.Storages.PP;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace PPDesk.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EventsPage : Page, IForServiceCollectionExtension
    {
        private readonly ILogger<EventsPage> _logger;

        public EventsPage(EventViewModel eventViewModel, ILogger<EventsPage> logger)
        {
            _logger = logger;
            this.InitializeComponent();
            this.DataContext = eventViewModel;

            LoadComponents();
        }

        private void LoadComponents()
        {
            if (SrvAppConfigurationStorage.DatabaseConfiguration.DatabaseExists)
            {
                EventsCountAsync();
                LoadEventsAsync();
                InitializeComboBoxesAsync();
            }
            else
            {
                this.Loaded += TablesPage_Loaded;
            }
        }

        private void TablesPage_Loaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= TablesPage_Loaded; 
            ShowDatabaseNotFoundAlertAsync();
        }

        private async void ShowDatabaseNotFoundAlertAsync()
        {
            var dialog = new ContentDialog
            {
                Title = "Database non trovato",
                Content = "Il database non esiste. Andare nelle impostazioni per crearlo",
                CloseButtonText = "OK",
                XamlRoot = this.XamlRoot
            };

            try
            {
                await dialog.ShowAsync();
                _logger.LogWarning("Database non trovato - alert mostrato all'utente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore durante la visualizzazione dell'alert");
            }
        }

        private async void InitializeComboBoxesAsync()
        {
            try
            {
                var eventViewModel = (EventViewModel)DataContext;
                await eventViewModel.InitializeComboBoxesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }
        private async void LoadEventsAsync()
        {
            try
            {
                var eventViewModel = (EventViewModel)DataContext;
                await eventViewModel.LoadEventsAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }

        private async void EventsCountAsync()
        {
            try
            {
                var eventViewModel = (EventViewModel)DataContext;
                await eventViewModel.EventsCountAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }

        private async void PrevButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var eventViewModel = (EventViewModel)DataContext;
                await eventViewModel.PrevButton();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }

        private async void NextButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var eventViewModel = (EventViewModel)DataContext;
                await eventViewModel.NextButton();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }

        private async void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var eventViewModel = (EventViewModel)DataContext;
                await eventViewModel.LoadEventsAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }

        private async void DataGrid_Sorting(object sender, DataGridColumnEventArgs e)
        {
            try
            {
                var eventViewModel = (EventViewModel)DataContext;
                string propertyName = e.Column.Tag?.ToString();

                if (!string.IsNullOrEmpty(propertyName))
                {
                    bool isAscending = e.Column.SortDirection != DataGridSortDirection.Ascending;
                    e.Column.SortDirection = isAscending ? DataGridSortDirection.Ascending : DataGridSortDirection.Descending;

                    foreach (var column in ((DataGrid)sender).Columns)
                    {
                        if (column != e.Column)
                        {
                            column.SortDirection = null;
                        }
                    }

                    await eventViewModel.DataSortAsync(propertyName, isAscending);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }

        private void OnPageKeyDown(object sender, KeyRoutedEventArgs e)
        {
            try
            {
                if (e.Key == VirtualKey.Enter)
                {
                    LoadEventsAsync();
                }
                else if (e.Key == VirtualKey.Back && sender is ComboBox comboBox)
                {
                    if (((ComboBox)sender).Name == "ComboBoxEvent")
                    {
                        ComboBoxEvent.SelectedItem = null;
                    }
                    else if (((ComboBox)sender).Name == "ComboBoxStatusEvent")
                    {
                        ComboBoxStatusEvent.SelectedItem = null;
                    }
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }
    }
}
