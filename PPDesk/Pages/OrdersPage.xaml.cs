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
using PPDesk.Abstraction.Helper;
using CommunityToolkit.WinUI.UI.Controls;
using Microsoft.Extensions.Logging;
using PPDesk.Abstraction.DTO.UI;
using PPDesk.ViewModels;
using Windows.System;
using PPDesk.Service.Storages.PP;
using PPDesk.Abstraction.DTO.Service.PP.Order;
using Microsoft.UI;
using PPDesk.Abstraction.DTO.Service.PP.Table;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace PPDesk.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class OrdersPage : Page, IForServiceCollectionExtension
    {
        private readonly ILogger<OrdersPage> _logger;

        public OrdersPage(OrderViewModel orderViewModel, ILogger<OrdersPage> logger)
        {
            _logger = logger;
            this.InitializeComponent();
            this.DataContext = orderViewModel;

            LoadComponents();
        }

        private void LoadComponents()
        {
            if (SrvAppConfigurationStorage.DatabaseConfiguration.DatabaseExists)
            {
                OrdersCountAsync();
                LoadOrdersAsync();
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
                var orderViewModel = (OrderViewModel)DataContext;
                await orderViewModel.InitializeComboBoxesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }

        private async void LoadOrdersAsync()
        {
            try
            {
                var orderViewModel = (OrderViewModel)DataContext;
                await orderViewModel.LoadOrdersAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }

        private async void OrdersCountAsync()
        {
            try
            {
                var orderViewModel = (OrderViewModel)DataContext;
                await orderViewModel.OrdersCountAsync();
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
                var orderViewModel = (OrderViewModel)DataContext;
                await orderViewModel.PrevButton();
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
                var orderViewModel = (OrderViewModel)DataContext;
                await orderViewModel.NextButton();
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
                var orderViewModel = (OrderViewModel)DataContext;
                await orderViewModel.LoadOrdersAsync();
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
                var orderViewModel = (OrderViewModel)DataContext;
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

                    await orderViewModel.DataSortAsync(propertyName, isAscending);
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
                    LoadOrdersAsync();
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

        private async void RowButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var button = sender as Button;

                var order = (SrvInformationOrder)button!.Tag;
                order.UserPresence = order.UserPresence == true ? false : true;

                var orderViewModel = (OrderViewModel)DataContext;
                await orderViewModel.CheckPresenceAsync(order);

                LoadOrdersAsync();

                MessageBoxAsync("Presenza", "La presenza è stata impostata correttamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }

        private void DataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            try
            {
                if (e.Row.DataContext is SrvInformationOrder order)
                {
                    _logger.LogInformation($"LoadingRow per table {order.Id}, AllUsersPresence: {order.UserPresence}");

                    if (order.UserPresence)
                    {
                        e.Row.Background = new SolidColorBrush(Colors.Green);
                        _logger.LogInformation($"Riga {order.Id} colorata di verde");
                    }
                    else
                    {
                        e.Row.Background = new SolidColorBrush(Colors.Transparent);
                    }
                }
                else
                {
                    _logger.LogWarning("DataContext non è di tipo SrvInformationTable");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore in LoadingRow");
            }
        }

        private async void MessageBoxAsync(string title, string message)
        {
            var optionsDialog = new ContentDialog
            {
                Title = title,
                Content = message,
                PrimaryButtonText = "Ok",
                XamlRoot = this.XamlRoot
            };

            await optionsDialog.ShowAsync();
        }
    }
}
