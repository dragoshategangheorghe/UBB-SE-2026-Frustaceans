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
    public sealed partial class CheckoutPage : Page
    {
        UserService userServ;
        CheckoutViewModel viewModel;

        public CheckoutPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            userServ = (UserService)e.Parameter;
            viewModel = new CheckoutViewModel(userServ);
            base.OnNavigatedTo(e);
        }

        private void SetDefaultPickUpDate(object sender, RoutedEventArgs e)
        {
            PickUpDateSelector.MinDate = new DateTimeOffset(DateTime.Now.Date);
            PickUpDateSelector.SelectedDates.Add(PickUpDateSelector.MinDate.AddDays(1));
        }

        private void PlaceOrder(object sender, RoutedEventArgs e)
        {
            DateTime selectedDate = PickUpDateSelector.SelectedDates[0].DateTime;
        }

        private void NavigateToBasket(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(BasketPage), userServ);
        }
    }
}
