using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using PharmacyApp.Models;
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

namespace PharmacyApp.Features.Orders.Views;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class OrderHistoryPage : Page
{
    OrderService userServ;
    OrderHistoryViewModel ViewModel { get; set; }

    public OrderHistoryPage()
    {
        InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        // TODO refactor later to have greater separation between 
        userServ = (OrderService)e.Parameter;
        ViewModel = new OrderHistoryViewModel(userServ);
        DataContext = ViewModel;
        base.OnNavigatedTo(e);

        ViewModel.ClickDetailButton += RedirectToDetailPage;
        ViewModel.ClickCancelButton += AskCancelOrderConfirmation;
        ViewModel.ClickResubmitButton += RedirectToResubmitPage;
    }

    private void RedirectToDetailPage(Tuple<OrderService, Order> args)
    {
        Frame.Navigate(typeof(PharmacyApp.Features.Orders.Views.ModifyIncompleteOrderPage),
                    new Tuple<OrderService, int>(args.Item1, args.Item2.Id));
    }

    private async void AskCancelOrderConfirmation(Tuple<OrderService, Order> args)
    {
        Order currOrder = args.Item2;

        ContentDialog dialog = new ContentDialog();

        dialog.XamlRoot = this.XamlRoot;
        dialog.Title = "Cancel?";
        dialog.PrimaryButtonText = "Yes";
        dialog.SecondaryButtonText = "No";
        dialog.DefaultButton = ContentDialogButton.None;
        dialog.Content = $"Do you want to cancel Order#{currOrder.Id}?";

        var result = await dialog.ShowAsync();

        if (result == ContentDialogResult.Primary)
            ViewModel.CancelOrder(currOrder);
    }

    private void RedirectToResubmitPage(Tuple<OrderService, Order> args)
    {
        Frame.Navigate(typeof(PharmacyApp.Features.Orders.Views.ResubmitOrderPage),
                    new Tuple<OrderService, int>(args.Item1, args.Item2.Id));
    }
}

// this is needed as custom logic on the view level to represent
// expired and not expired orders differently in the order history
public class OrderTemplateSelector : DataTemplateSelector
{
    public DataTemplate CompletedTemplate { get; set; }
    public DataTemplate IncompletedTemplate { get; set; }
    public DataTemplate ExpiredTemplate { get; set; }

    protected override DataTemplate SelectTemplateCore(object item)
    {
        Order currentOrder = (Order)item;

        if (currentOrder.IsCompleted)
            return CompletedTemplate;
        if (currentOrder.IsExpired)
            return ExpiredTemplate;
        return IncompletedTemplate;
    }
}