using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using PharmacyApp.Common.Repositories;
using PharmacyApp.Common.Services;
using PharmacyApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace PharmacyApp.Features.Pharmacy_Management;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class StatisticsPage : Page
{
    public List<Tuple<int, string, int>> TopItems { get; set; }
    public Dictionary<string, int> TopSubstances { get; set; }
    private AdminService adminService;
    private IItemsRepository itemsRepository;
    private ISubstancesRepository substancesRepository;
    public StatisticsPage()
    {
        InitializeComponent();
        itemsRepository = new SQLItemsRepository();
        substancesRepository = new SQLSubstancesRepository();
        adminService = new AdminService(itemsRepository, substancesRepository);
        TopItems = adminService.GetTop30Items();
        TopSubstances =adminService.GetTop20Substances();
        ItemsGrid.Visibility = Visibility.Visible;
        SubstancesGrid.Visibility = Visibility.Collapsed;
    }

    private void GoToEditPageClick(object sender, RoutedEventArgs e)
    {
        this.Frame.Navigate(typeof(EditPage));
    }

    private void OnItemsClick(object sender, RoutedEventArgs e)
    {
        ItemsGrid.Visibility = Visibility.Visible;
        SubstancesGrid.Visibility = Visibility.Collapsed;
    }

    private void OnSubstancesClick(object sender, RoutedEventArgs e)
    {
        ItemsGrid.Visibility = Visibility.Collapsed;
        SubstancesGrid.Visibility = Visibility.Visible;
    }
}
