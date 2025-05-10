using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
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
        public MainWindow(ISrvMainWindowService service)
        {
            this.InitializeComponent();
            _service = service;
        }


        private async void myButton_Click(object sender, RoutedEventArgs e)
        {
            myButton.Content = "Clicked";

            await _service.Test();
        }
    }
}
