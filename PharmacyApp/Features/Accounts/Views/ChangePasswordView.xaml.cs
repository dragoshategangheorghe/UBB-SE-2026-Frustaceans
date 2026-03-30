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
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ChangePasswordView : ContentDialog
    {
        private UserAccountService _accountService;
        public ChangePasswordViewModel ViewModel { get; }

        public ChangePasswordView(UserAccountService service)
        {
            this.InitializeComponent();

            _accountService = service;
            ViewModel = new ChangePasswordViewModel(service);

            this.DataContext = ViewModel;
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            try
            {
                ViewModel.OldPassword = OldPasswordBox.Password;
                ViewModel.NewPassword = NewPasswordBox.Password;
                ViewModel.ConfirmPassword = ConfirmPasswordBox.Password;

                ViewModel.ErrorMessage = null;

                ViewModel.ChangePasswordCommand.Execute(null);
            }
            catch (Exception ex)
            {
                args.Cancel = true;
                ViewModel.ErrorMessage = ex.Message;
            }
        }
    }
}
