using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using PharmacyApp.Common.Repositories;
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
    public sealed partial class ResubmitOrderPage : Page
    {

        OrderService orderService;
        ResubmitOrderViewModel viewModel;

        public ResubmitOrderPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var extractedArgs = (Tuple<OrderService, int>)(e.Parameter);

            orderService = extractedArgs.Item1;
            int orderID = extractedArgs.Item2;
            viewModel = new(orderService, orderID);
            DataContext = viewModel;

            base.OnNavigatedTo(e);
        }

        private void SetDefaultPickUpDate(object sender, RoutedEventArgs e)
        {
            PickUpDateSelector.MinDate = new System.DateTimeOffset(DateTime.Now.Date.AddDays(1));
            PickUpDateSelector.SelectedDates.Add(PickUpDateSelector.MinDate);
        }

        private void CheckUnselectedDate(CalendarView sender, CalendarViewSelectedDatesChangedEventArgs e)
        {
            if (PickUpDateSelector.SelectedDates.Count == 0)
                PickUpDateSelector.SelectedDates.Add(PickUpDateSelector.MinDate);
        }

        private async void ResubmitOrder(object sender, RoutedEventArgs e)
        {
            DateOnly selectedDate = DateOnly.FromDateTime(PickUpDateSelector.SelectedDates[0].Date);
            int orderIDToResubmit = viewModel.shownOrderID;

            // TODO not get the function directly from the user service
            // maybe get it through the view model? but na, no time
            try
            {
                orderService.ResubmitExpiredOrder(orderIDToResubmit, selectedDate);

                ContentDialog confirmationMessage = new ContentDialog();

                confirmationMessage.XamlRoot = this.XamlRoot;
                confirmationMessage.Title = "Success";
                confirmationMessage.Content = "A new order has been created identical to the previously selected expired order";
                confirmationMessage.CloseButtonText = "Ok";

                // TODO rewrite the parameter, so that it's connected nicely
                Frame.Navigate(typeof(PharmacyApp.Features.Orders.Views.OrderHistoryPage), orderService);
                var result = await confirmationMessage.ShowAsync();
            }
            catch (ArgumentException exception)
            {
                ContentDialog causeOfErrorDialog = new ContentDialog();

                causeOfErrorDialog.XamlRoot = this.XamlRoot;
                causeOfErrorDialog.Title = "Error";
                causeOfErrorDialog.Content = exception.Message;
                causeOfErrorDialog.CloseButtonText = "Ok";

                var result = await causeOfErrorDialog.ShowAsync();
            }
        }

        private void NavigateToOrderHistory(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(OrderHistoryPage), orderService);
        }
    }
}
