using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using PharmacyApp.Common.Repositories;
using PharmacyApp.Features.Accounts.Logic;
using PharmacyApp.Features.Accounts.Views;
using PharmacyApp.Features.Products_Catalogue;
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

namespace PharmacyApp
{

    public sealed partial class MainWindow : Window
    {
        private ProductCatalogueService productService;

    public MainWindow()
        {
            ServiceWrapper.Initialize();

            InitializeComponent();
            IItemsRepository repo = new SQLItemsRepository();
            productService = new ProductCatalogueService(repo);
            Features.Accounts.Views.LoginView.UserLoggedIn += () =>
            {
                UpdateUI();
            };
            Features.Accounts.Views.RegisterView.UserRegistered += () =>
            {
                UpdateUI();
            };


            MainFrame.Navigate(typeof(Features.Products_Catalogue.HomePage));
        }


        private void OnHomeClicked(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(typeof(Features.Products_Catalogue.HomePage));
        }

        private void OnProductsClicked(object sender, RoutedEventArgs e)
        {
            User? currentuser = ServiceWrapper.UserAccountService.CurrentUser;
            MainFrame.Navigate(typeof(Features.Products_Catalogue.CatalogPage), (productService, currentuser));
        }

        private void OnCartClicked(object sender, RoutedEventArgs e)
        {
        }

        private async void OnAccountClicked(object sender, RoutedEventArgs e)
        {



            if (ServiceWrapper.UserAccountService.CurrentUser == null)
            {
                // Not logged in -> go to login page
                MainFrame.Navigate(typeof(Features.Accounts.Views.LoginView));
            }
            else
            {
                // Logged in -> show profile dialog
                MainFrame.Navigate(typeof(Features.Accounts.Views.ProfileManagementView));
            }
        }

        private void OnAdminClicked(object sender, RoutedEventArgs e)
        {

            MainFrame.Navigate(typeof(Features.Pharmacy_Management.EditPage));
        }

        private void OnPeriodTrackerClicked(object sender, RoutedEventArgs e)
        {
            if (ServiceWrapper.UserAccountService.CurrentUser == null)
            {
                MainFrame.Navigate(typeof(Features.Accounts.Views.LoginView));
            }
            else
                MainFrame.Navigate(typeof(Features.Period_Tracker.Views.PeriodTrackerPage));
        }
        private void UpdateUI()
        {
            var user = ServiceWrapper.UserAccountService.CurrentUser;

            if (user != null && user.IsAdmin)
            {
                AdminButton.Visibility = Visibility.Visible;
                AdminUsersButton.Visibility = Visibility.Visible;
            }
            else
            {
                AdminButton.Visibility = Visibility.Collapsed;
                AdminUsersButton.Visibility = Visibility.Collapsed;
            }
            if (user != null)
            {
                RegisterButton.Visibility = Visibility.Collapsed;
                LoginButton.Visibility = Visibility.Collapsed;
                AccountButton.Visibility = Visibility.Visible;
            }
            else
            {
                RegisterButton.Visibility = Visibility.Visible;
                LoginButton.Visibility = Visibility.Visible;
                AccountButton.Visibility = Visibility.Collapsed;
            }
        }

        private void OnAdminUsersClicked(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(typeof(Features.Accounts.Views.AdminAccountsManagementView));
        }

        private void OnRegisterClicked(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(typeof(Features.Accounts.Views.RegisterView));
        }

        private void OnLoginClicked(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(typeof(Features.Accounts.Views.LoginView));
        }
    }
}