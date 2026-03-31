using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using PharmacyApp.Features.Accounts.ViewModels;
using PharmacyApp.Features.Accounts.Logic;
using PharmacyApp.Common.Repositories;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace PharmacyApp.Features.Accounts.Views
{

    public sealed partial class LoginView : Page
    {
        public static event Action UserLoggedIn;
        public LoginView()
        {
            this.InitializeComponent();
            var viewModel = new LoginViewModel(ServiceWrapper.UserAccountService);
            viewModel.LoginSucceded += OnLoginSucceeded;
            this.DataContext = viewModel;
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            var vm = (LoginViewModel)this.DataContext;
            vm.Password = PasswordBox.Password;
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            (this.Parent as Frame)?.Navigate(typeof(RegisterView));
        }
        private void OnLoginSucceeded()
        {
            UserLoggedIn?.Invoke();
            (this.Parent as Frame)?.Navigate(typeof(Features.Accounts.Views.ProfileManagementView));
        }
    }
}
