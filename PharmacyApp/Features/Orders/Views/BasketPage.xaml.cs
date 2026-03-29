using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using PharmacyApp.Features.Orders.Logic;
using PharmacyApp.Features.Orders.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace PharmacyApp.Features.Orders.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BasketPage : Page
    {
        
        UserService userServ;
        // does it need to be a property? I don't have time
        public BasketViewModel ViewModel { get; set; }

        public BasketPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            userServ = (UserService)e.Parameter;
            ViewModel = new BasketViewModel(userServ);
            DataContext = ViewModel;

            ViewModel.BasketQuantityRemoved += HandleCheckoutButton;

            ViewModel.OnBasketQuantityRemoved();

            base.OnNavigatedTo(e);
        }

        private void NavigateToCheckout(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(CheckoutPage), userServ);
        }

        private void HandleCheckoutButton(int quantity)
        {
            if (quantity > 0)
                CheckoutButton.Visibility = Visibility.Visible;
            else
                CheckoutButton.Visibility = Visibility.Collapsed;
        }

        private void EnterPrescriptionID(object sender, RoutedEventArgs e)
        {
            PrescriptionWarning.Visibility = Visibility.Collapsed;
            string prescriptionId = PrescriptionInputBox.Text;
            
            try
            {
                ViewModel.GetPrescription(prescriptionId);
            } 
            catch (ArgumentException exception)
            {
                PrescriptionWarning.Text = exception.Message;
                PrescriptionWarning.Visibility = Visibility.Visible;
            }
        }
    }
}
