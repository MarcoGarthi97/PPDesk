using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using PPDesk.Pages;
using PPDesk.Service.Services.Eventbrite;
using PPDesk.Service.Services.Window;
using RestSharp;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace PPDesk
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        private readonly ISrvMainWindowService _service;
        private readonly IServiceProvider _serviceProvider;

        public MainWindow(ISrvMainWindowService service, IServiceProvider serviceProvider)
        {
            this.InitializeComponent();
            _service = service;
            _serviceProvider = serviceProvider;

            var tablesPage = _serviceProvider.GetRequiredService<TablesPage>();
            ContentFrame.Content = tablesPage;
        }


        private async void myButton_Click(object sender, RoutedEventArgs e)
        {
            myButton.Content = "Clicked";
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
