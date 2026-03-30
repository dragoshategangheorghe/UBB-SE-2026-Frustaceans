using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using PharmacyApp.Features.Accounts.Logic;
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

    public sealed partial class RegisterView : Page
    {
        public RegisterView()
        {
            this.InitializeComponent();
            this.DataContext = new RegisterViewModel(ServiceWrapper.UserAccountService);
        }
        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            var vm = (RegisterViewModel)this.DataContext;
            vm.Password = PasswordBox.Password;
        }
        private void ConfirmPasswordBox_PasswordChanged(object sender,RoutedEventArgs e)
        {
            var vm = (RegisterViewModel)this.DataContext;
            vm.ConfirmPassword = ConfirmPasswordBox.Password;
        }
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            (this.Parent as Frame)?.Navigate(typeof(LoginView));
        }
    }
}
