using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using PharmacyApp.Features.Accounts.Logic;
using PharmacyApp.Features.Orders.Logic;
using PharmacyApp.Features.Accounts.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace PharmacyApp.Features.Accounts.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ProfileManagementView : Page
    {
        private UserAccountService _accountService;
        public ProfileManagementViewModel ViewModel { get; }

        public ProfileManagementView()
        {
            this.InitializeComponent();

            _accountService = ServiceWrapper.UserAccountService;
            ViewModel = new ProfileManagementViewModel(_accountService);

            this.DataContext = ViewModel;
        }

        private void OnSaveClick(object sender, RoutedEventArgs e)
        {
            try
            {
                ViewModel.ErrorMessage = null;
                ViewModel.SaveChanges();
            }
            catch (Exception ex)
            {
                ViewModel.ErrorMessage = ex.Message;
            }
        }

        private void OnCancelClick(object sender, RoutedEventArgs e)
        {
            ViewModel.CancelChanges();
        }

        private async void OnChangePasswordClick(object sender, RoutedEventArgs e)
        {
            var dialog = new ChangePasswordView(_accountService);
            dialog.XamlRoot = this.XamlRoot;

            await dialog.ShowAsync();
        }

        private async void OnOrderHistoryClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(PharmacyApp.Features.Orders.Views.OrderHistoryPage), new OrderService());
        }
    }
}
