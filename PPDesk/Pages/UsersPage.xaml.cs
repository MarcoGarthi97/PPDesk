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
using PPDesk.ViewModels;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace PPDesk.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class UsersPage : Page
    {
        public UsersPage(UserViewModel userViewModel)
        {
            this.InitializeComponent();
            this.DataContext = userViewModel;

            UsersCountAsync();
        }

        private async void UsersCountAsync()
        {
            var userViewModel = (UserViewModel)DataContext;
            await userViewModel.UsersCountAsync();
        }

        private async void PrevButton_Click(object sender, RoutedEventArgs e)
        {
            var userViewModel = (UserViewModel)DataContext;
            await userViewModel.PrevButton();
        }

        private async void NextButton_Click(object sender, RoutedEventArgs e)
        {
            var userViewModel = (UserViewModel)DataContext;
            await userViewModel.NextButton();
        }

        private async void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            var userViewModel = (UserViewModel)DataContext;
            await userViewModel.FilterUsersAsync();
        }

    }
}
