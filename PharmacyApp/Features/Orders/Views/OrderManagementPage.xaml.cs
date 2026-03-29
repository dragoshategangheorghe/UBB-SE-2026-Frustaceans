using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using PharmacyApp.Features.Orders.Logic;
using PharmacyApp.Features.Orders.ViewModels;
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

namespace PharmacyApp.Features.Orders.Views;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class OrderManagementPage : Page
{

    OrderService orderService;
    public OrderManagementViewModel ViewModel { get; set; }

    public OrderManagementPage()
    {
        InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        orderService = (OrderService)e.Parameter;
        ViewModel = new(orderService);
        DataContext = ViewModel;
    }
}


public class OrderDetailTemplateSelector : DataTemplateSelector
{
    public DataTemplate IncompleteTemplate { get; set; }
    public DataTemplate ExpiredTemplate { get; set; }
    public DataTemplate CompleteTemplate { get; set; }

    protected override DataTemplate SelectTemplateCore(object item)
    {
        OrderDetail currentOrder = (OrderDetail)item;

        if (currentOrder.IsComplete)
            return CompleteTemplate;
        if (currentOrder.IsExpired)
            return ExpiredTemplate;
        return IncompleteTemplate;
    }
}