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
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        private ProductCatalogueService productService;

        public MainWindow()
        {
            InitializeComponent();
            IItemsRepository repo = new SQLItemsRepository();
            productService = new ProductCatalogueService(repo);
            
            
            MainFrame.Navigate(typeof(Features.Products_Catalogue.HomePage));
        }

        private void OnHomeClicked(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(typeof(Features.Products_Catalogue.HomePage), productService);
        }

        private void OnProductsClicked(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(typeof(Features.Products_Catalogue.CatalogPage), productService);
        }

        private void OnCartClicked(object sender, RoutedEventArgs e)
        {

        }

        private async void OnAccountClicked(object sender, RoutedEventArgs e)
        {



            if (ServiceWrapper.UserAccountService.CurrentUser == null)
            {
                // Not logged in → go to login page
                MainFrame.Navigate(typeof(Features.Accounts.Views.LoginView));
            }
            else
            {
                // Logged in → show profile dialog
                MainFrame.Navigate(typeof(Features.Accounts.Views.ProfileManagementView));
            }
        }

        private void OnAdminClicked(object sender, RoutedEventArgs e)
        {

            MainFrame.Navigate(typeof(Features.Pharmacy_Management.EditPage));
        }

        private void OnPeriodTrackerClicked(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(typeof(Features.Period_Tracker.Views.PeriodTrackerPage));
        }
    }
}