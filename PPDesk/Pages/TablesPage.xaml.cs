using CommunityToolkit.WinUI.UI.Controls;
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
        public TablesPage(TableViewModel tableViewModel)
        {
            this.InitializeComponent();
            this.DataContext = tableViewModel;

            TablesCountAsync();
            LoadTablesAsync();
        }

        private async void LoadTablesAsync()
        {
            var tableViewModel = (TableViewModel)DataContext;
            await tableViewModel.LoadTablesAsync();
        }

        private async void TablesCountAsync()
        {
            var tableViewModel = (TableViewModel)DataContext;
            await tableViewModel.TablesCountAsync();
        }

        private async void PrevButton_Click(object sender, RoutedEventArgs e)
        {
            var tableViewModel = (TableViewModel)DataContext;
            await tableViewModel.PrevButton();
        }

        private async void NextButton_Click(object sender, RoutedEventArgs e)
        {
            var tableViewModel = (TableViewModel)DataContext;
            await tableViewModel.NextButton();
        }

        private async void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            var tableViewModel = (TableViewModel)DataContext;
            await tableViewModel.LoadTablesAsync();
        }

        private async void DataGrid_Sorting(object sender, DataGridColumnEventArgs e)
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

        private void OnPageKeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                LoadTablesAsync();
            }
        }
    }
}
