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
            this.InitializeComponent();
            this.DataContext = orderViewModel;

            OrdersCountAsync();
            LoadOrdersAsync();
            _logger = logger;
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
                    if (((ComboBox)sender).Name == "ComboBoxOrder")
                    {
                        ComboBoxOrder.SelectedItem = null;
                    }
                    else if (((ComboBox)sender).Name == "ComboBoxStatusOrder")
                    {
                        ComboBoxStatusOrder.SelectedItem = null;
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
