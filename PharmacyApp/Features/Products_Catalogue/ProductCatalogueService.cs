using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using PharmacyApp.Common.Repositories;
using PharmacyApp.Models;
using Item = PharmacyApp.Models.Item;

namespace PharmacyApp.Features.Products_Catalogue
{
    internal class ProductCatalogueService
    {
        private readonly IItemsRepository itemRepo;
        public ProductCatalogueService(IItemsRepository itemRepo)
        {
            this.itemRepo = itemRepo;
        }
        public List<Item> getItems(
            string search,
            List<string> categories = null,
            List<(float min, float max)> priceRanges = null,
            string stockFilter = null,
            bool? discounted = null,
            List<string> substances = null,
            bool ascending = true,
            int page = 0,
            int pageSize = 10)
            { 
                var items = searchItems(search);
                items = categoryFilter(items, categories);
                items = priceFilter(items, priceRanges);
                items = StockFilter(items, stockFilter);
                items = discountFilter(items, discounted);
                items = substanceFilter(items, substances);
                items = priceSort(items, ascending);
                items = newestSort(items, ascending); //TODO change priceSort and newestSort to have only one function
                items = paginate(items, page, pageSize);
                return items;    
            }
        private List<Item> searchItems(string productName)
        {
            var items = itemRepo.GetAllItems();

            if (string.IsNullOrWhiteSpace(productName))
            {
                return items;
            }
            if (string.IsNullOrEmpty(productName))
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
        private List<Item> categoryFilter(List<Item> items, List<string> categories)
        {
            if (categories == null || !categories.Any())
                return items;
            return items.Where(item => categories.Contains(item.Category)).ToList();
        }
        private List<Item> priceFilter(List<Item> items, List<(float min, float max)> ranges)
        {
            if (ranges == null || !ranges.Any())
                return items;
            foreach (var (min, max) in ranges)
            {
                if (min < 0 || max < 0 || min > max)
                {
                    throw new ArgumentException(nameof(min) + " " + nameof(max) + " are not valid for a price filter");
                }
            }
            return items.Where(item => ranges.Any(range => 
            item.Price >= range.min && item.Price <= range.max)).ToList();
        }
        private List<Item> StockFilter(List<Item> items, string? stockFilter)
        {
            if (stockFilter == null)
                return items;
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
        private List<Item> substanceFilter(List<Item> items, List<string> substances)
        {
            if (substances == null || !substances.Any())
                return items;
            return items.Where(item => substances.All(substance => item.ActiveSubstances.ContainsKey(substance))).ToList();
        }
        private List<Item> producerFilter(List<Item> items, List<string> producers)
        {
            if (producers == null || !producers.Any())
                return items;
            return items.Where(item => producers.Contains(item.Producer)).ToList();
        }
        private List<Item> priceSort(List<Item> items, bool ascending)
        {
            if (ascending)
                return items.OrderBy(item => item.Price).ToList();
            return items.OrderByDescending(item => item.Price).ToList();
        }


        private List<Item> newestSort(List<Item> items, bool ascending)
        {
            if (ascending)
            {
                return items
                    .OrderBy(item => latestValidDate(item) ?? DateOnly.MinValue)
                    .ToList();
            }

            return items
                .OrderByDescending(item => latestValidDate(item) ?? DateOnly.MinValue)
                .ToList();
        }
        private DateOnly? latestValidDate(Item item)
        {
            DateOnly today = DateOnly.FromDateTime(DateTime.Now);

            var validDates = item.Batches.Keys
                .Where(date => date > today);

            if (!validDates.Any())
                return null;

            return validDates.Max();
        }
        private List<Item> paginate(List<Item> items, int page, int pageSize) {
            return items.Skip(page * pageSize).Take(pageSize).ToList();
        }
    }
}
