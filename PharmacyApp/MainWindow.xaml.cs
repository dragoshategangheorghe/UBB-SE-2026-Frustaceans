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
            /*
            ISubstancesRepository substancesRepo = new SQLSubstancesRepository();
            System.Diagnostics.Debug.WriteLine(substancesRepo.SubstanceExists("ibuprophen"));
            System.Diagnostics.Debug.WriteLine(substancesRepo.SubstanceExists("dggfddg"));
            Substance a = substancesRepo.GetSubstance("ibuprophen");
            try
            {
                Substance b = substancesRepo.GetSubstance("nooooooo");
            }
            catch (Exception ex) {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            substancesRepo.RemoveSubstance("meth");
            try
            {
                substancesRepo.RemoveSubstance("nooooooo");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            */
            InitializeComponent();
            MainFrame.Navigate(typeof(Features.Products_Catalogue.HomePage));
        }

        private void OnHomeClicked(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(typeof(Features.Products_Catalogue.HomePage));
        }

        private void OnProductsClicked(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(typeof(Features.Products_Catalogue.CatalogPage));
        }

        private void OnCartClicked(object sender, RoutedEventArgs e)
        {

        }

        private void OnAccountClicked(object sender, RoutedEventArgs e)
        {

        }

        private void OnCycleTrackerClicked(object sender, RoutedEventArgs e)
        {

        }

        private void OnAdminClicked(object sender, RoutedEventArgs e)
        {

        }
    }
}
