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
using PPDesk.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
            this.InitializeComponent();
            this.DataContext = tableViewModel;

            TablesCountAsync();
            LoadTablesAsync();
            _logger = logger;
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
