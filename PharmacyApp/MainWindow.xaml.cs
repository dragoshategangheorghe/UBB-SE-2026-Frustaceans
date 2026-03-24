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
            IUsersRepository usersRepository = new SQLUsersRepository();

            System.Diagnostics.Debug.WriteLine(SQLUtility.GetConnectionString());
            System.Diagnostics.Debug.WriteLine(usersRepository.GetUserById(1).Email);
            System.Diagnostics.Debug.WriteLine(usersRepository.UserExists(1));
            System.Diagnostics.Debug.WriteLine(usersRepository.UserExists("dgfs@ghsg.com"));
            System.Diagnostics.Debug.WriteLine(usersRepository.UserExists("dgfgfdssfds@ghsg.com"));
            System.Diagnostics.Debug.WriteLine(usersRepository.UserExists(2));

            User user1 = usersRepository.GetUserById(1);
            System.Diagnostics.Debug.WriteLine(user1.PeriodNotes);
            System.Diagnostics.Debug.WriteLine(user1.UserDiscounts);
            System.Diagnostics.Debug.WriteLine(user1.StockAlerts);
            System.Diagnostics.Debug.WriteLine(user1.FavoriteItems);
            InitializeComponent();
        }
    }
}
