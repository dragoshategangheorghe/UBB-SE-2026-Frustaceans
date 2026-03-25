using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using PharmacyApp.Common.Repositories;
using PharmacyApp.Models;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace PharmacyApp
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            /* TEST CODE!!!
            IUsersRepository usersRepository = new SQLUsersRepository();

            System.Diagnostics.Debug.WriteLine(SQLUtility.GetConnectionString());
            System.Diagnostics.Debug.WriteLine(usersRepository.GetUserById(1).Email);
            System.Diagnostics.Debug.WriteLine(usersRepository.UserExists(1));
            System.Diagnostics.Debug.WriteLine(usersRepository.UserExists("dgfs@ghsg.com"));
            System.Diagnostics.Debug.WriteLine(usersRepository.UserExists("dgfgfdssfds@ghsg.com"));
            System.Diagnostics.Debug.WriteLine(usersRepository.UserExists(2));

            User user1 = usersRepository.GetUserById(1);
            List<User> users = usersRepository.GetAllUsers();


            User newUser = user1;

            newUser.Email = "newmail@gdsdd.com";
            newUser.PhoneNumber = "0727063545";
            newUser.PasswordHash = "newpass";
;           //newUser.RemovePeriodNote(2);
            //newUser.RemoveUserDiscount(1); // remove the 30% discount
            newUser.CycleDays = 25;
            newUser.PMSOption = 4;
            newUser.StartPeriodDate = new DateOnly(2026, 3, 25);
            //newUser.RemoveStockAlert(1);
            //newUser.RemoveFavoriteItem(2);

            usersRepository.UpdateUser(newUser);
            */



            InitializeComponent();
            MainFrame.Navigate(typeof(Features.Products_Catalogue.HomePage));
        }

        private void OnHomeClicked(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(typeof(Features.Products_Catalogue.HomePage));
        }

        private void OnProductsClicked(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(typeof(Features.Products_Catalogue.CatalogPage));
        }

        private void OnCartClicked(object sender, RoutedEventArgs e)
        {

        }

        private void OnAccountClicked(object sender, RoutedEventArgs e)
        {

        }

        private void OnCycleTrackerClicked(object sender, RoutedEventArgs e)
        {

        }

        private void OnAdminClicked(object sender, RoutedEventArgs e)
        {

        }
    }
}
