using CommunityToolkit.WinUI.UI.Controls;
using Microsoft.Extensions.Logging;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using PPDesk.Abstraction.Helper;
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
using Windows.System;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace PPDesk.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TablesPage : Page, IForServiceCollectionExtension
    {
        private readonly ILogger<TablesPage> _logger;

        public TablesPage(TableViewModel tableViewModel, ILogger<TablesPage> logger)
        {
            _logger = logger;
            this.InitializeComponent();
            this.DataContext = tableViewModel;

            LoadComponents();
        }

        private void LoadComponents()
        {
            if (SrvAppConfigurationStorage.DatabaseConfiguration.DatabaseExists)
            {
                TablesCountAsync();
                LoadTablesAsync();
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
                var tableViewModel = (TableViewModel)DataContext;
                await tableViewModel.InitializeComboBoxesAsync();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }

        private async void LoadTablesAsync()
        {
            try
            {
                var tableViewModel = (TableViewModel)DataContext;
                await tableViewModel.LoadTablesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }

        private async void TablesCountAsync()
        {
            try
            {
                var tableViewModel = (TableViewModel)DataContext;
                await tableViewModel.TablesCountAsync();
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
                var tableViewModel = (TableViewModel)DataContext;
                await tableViewModel.PrevButton();
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
                var tableViewModel = (TableViewModel)DataContext;
                await tableViewModel.NextButton();
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
                var tableViewModel = (TableViewModel)DataContext;
                await tableViewModel.LoadTablesAsync();
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
                var tableViewModel = (TableViewModel)DataContext;
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

                    await tableViewModel.DataSortAsync(propertyName, isAscending);
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
                    LoadTablesAsync();
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
                    else if (((ComboBox)sender).Name == "ComboBoxTypeTable")
                    {
                        ComboBoxTypeTable.SelectedItem = null;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }
    }
}
