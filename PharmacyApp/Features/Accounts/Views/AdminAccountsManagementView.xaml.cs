using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using PharmacyApp.Features.Accounts.Logic;
using PharmacyApp.Features.Accounts.ViewModels;
using PharmacyApp.Models;
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
    public sealed partial class AdminAccountsManagementView : Page
    {
        private UserAccountService _accountService;
        public AdminAccountsManagementViewModel ViewModel { get; }

        public AdminAccountsManagementView()
        {
            this.InitializeComponent();

            _accountService = ServiceWrapper.UserAccountService;
            ViewModel = new AdminAccountsManagementViewModel(_accountService);

            this.DataContext = ViewModel;
        }

        private void OnSearchClick(object sender, RoutedEventArgs e)
        {
            try
            {
                ViewModel.ErrorMessage = null;
                ViewModel.Search();
            }
            catch (Exception ex)
            {
                ViewModel.ErrorMessage = ex.Message;
            }
        }

        private async void OnPromoteClick(object sender, Microsoft.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            if (sender is CheckBox cb && cb.DataContext is UserItemViewModel userItem)
            {
                var dialog = new ContentDialog
                {
                    Title = "Warning",
                    Content = "This action cannot be undone. Proceed?",
                    PrimaryButtonText = "Proceed",
                    CloseButtonText = "Cancel",
                    XamlRoot = this.XamlRoot
                };

                var result = await dialog.ShowAsync();

                if (result == ContentDialogResult.Primary)
                {
                    try
                    {
                        ViewModel.ErrorMessage = null;
                        ViewModel.Promote(userItem);
                    }
                    catch (Exception ex)
                    {
                        ViewModel.ErrorMessage = ex.Message;
                    }
                }
                else
                {
                    cb.IsChecked = false;
                }
            }
        }

        private async void OnDisableClick(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is UserItemViewModel userItem)
            {
                var dialog = new ContentDialog
                {
                    Title = "Confirm",
                    Content = "Disable this account?",
                    PrimaryButtonText = "Yes",
                    CloseButtonText = "No",
                    XamlRoot = this.XamlRoot
                };

                var result = await dialog.ShowAsync();

                if (result == ContentDialogResult.Primary)
                {
                    try
                    {
                        ViewModel.ErrorMessage = null;
                        ViewModel.Disable(userItem);
                    }
                    catch (Exception ex)
                    {
                        ViewModel.ErrorMessage = ex.Message;
                    }
                }
            }
        }

    }
}
