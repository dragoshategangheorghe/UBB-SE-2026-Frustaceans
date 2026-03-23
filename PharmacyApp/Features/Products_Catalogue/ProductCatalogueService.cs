using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using PharmacyApp.Common.Repositories;
using PharmacyApp.Models;

namespace PharmacyApp.Features.Products_Catalogue
{
    internal class ProductCatalogueService
    {
        private readonly IItemRepository itemRepo;
        public ProductCatalogueService(IItemRepository itemRepo)
        {
            this.itemRepo = itemRepo;
        }

        private List<Item> searchItems(string productName)
        {
            var items = itemRepo.getItemsByName("");

            if (string.IsNullOrWhiteSpace(productName))
            {
                return items;
            }

            var filteredItems = items.Where(item =>
            {
                if (item.Name == null)
                    return false;
                bool nameMatches = item.Name.Contains(productName, StringComparison.OrdinalIgnoreCase);
                return nameMatches;
            });

            return filteredItems.ToList();

        }
        private List<Item> categoryFilter(List<Item> items, string category)
        {
            if (!string.IsNullOrWhiteSpace(category))
            {
                return items;
            }
            return items.Where(item => item.Category == category).ToList();
        }
        private List<Item> priceFilter(List<Item> items, float min, float max)
        {
            // price filter is going to have options of ranges, it will not let u input the exact price range
            // if we decide to let the user do this we need to check if the min and max have a value
            // note for tomorow: mb i need to check it anyway to see if a price range is selected
            if (min < 0 || max < 0 || min > max)
            {
                throw new ArgumentException(nameof(min) + " " + nameof(max) + " are not valid for a price filter");
            }
            return items.Where(item => item.Price >= min && item.Price <= max).ToList();
        }
        private List<Item> stockFilter(List<Item> items, string stockFilter)
        {
            if (stockFilter == "in_stock")
                return items.Where(item => item.Quantity > 0).ToList();
            if (stockFilter == "low_stock")
                return items.Where(item => item.Quantity > 0 && item.Quantity < 10).ToList();
            return items;
        }
        private List<Item> discountFilter(List<Item> items, bool? discounted)
        {
            if (!discounted.HasValue)
                return items;
            if (discounted == true)
                return items.Where(item => item.DiscountPercentage > 0).ToList();
            return items.Where(item => item.DiscountPercentage == 0).ToList();
        }

    }
}
