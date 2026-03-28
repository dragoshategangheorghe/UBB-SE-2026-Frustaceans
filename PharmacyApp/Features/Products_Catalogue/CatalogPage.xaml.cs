using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
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

namespace PharmacyApp.Features.Products_Catalogue
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public class UIItem
    {
        public string Name { get; set; }
        public string Price { get; set; }
        public string Image { get; set; }
    }
    public sealed partial class CatalogPage : Page
    {
        private ProductCatalogueService productService;
        public CatalogPage()
        {
            InitializeComponent();
        }

        private void OnSearchClicked(object sender, RoutedEventArgs e)
        {

        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            productService = (ProductCatalogueService)e.Parameter;

            LoadProducts();
        }
        private void LoadProducts()
        {
            var items = productService.getItems(" ");
            var uiItems = items.Select(item => new UIItem
            {
                Name = item.Name,
                Price = $"{item.Price:F2} lei",
                Image = item.ImagePath
            }).ToList();

            ProductsList.ItemsSource = uiItems;
        }
        //private void LoadProducts()
        //{
        //    var items = new List<UIItem>
        //    {
        //        new UIItem { Name = "Paracetamol", Price = "5.99 lei", Image = "ms-appx:///Assets/logo.png" },
        //        new UIItem { Name = "Ibuprofen", Price = "7.49 lei", Image = "ms-appx:///Assets/logo.png" },
        //        new UIItem { Name = "Vitamin C", Price = "9.99 lei", Image = "ms-appx:///Assets/logo.png" },
        //        new UIItem { Name = "Cough Syrup", Price = "6.75 lei", Image = "ms-appx:///Assets/logo.png" },
        //        new UIItem { Name = "Aspirin", Price = "4.50 lei", Image = "ms-appx:///Assets/logo.png" },
        //        new UIItem { Name = "Magnesium", Price = "11.25 lei", Image = "ms-appx:///Assets/logo.png" }
        //    };

        //    ProductsList.ItemsSource = items;
        //}
    }
}
