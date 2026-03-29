using PharmacyApp.Common.Services;
using PharmacyApp.Features.Orders.Logic;
using PharmacyApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyApp.Features.Orders.ViewModels
{

    public class ItemDetail
    {
        public string ItemThumbnailImagePath { get; set; }
        public string ItemDescription { get; set; }
        public string ItemQuantityString { get; set; }
        public string ItemFinalPriceString { get; set; }

        public ItemDetail(string imagePath, string description, int quantity, float finalPrice)
        {
            ItemThumbnailImagePath = imagePath;
            ItemDescription = description;
            ItemQuantityString = "Quantity: " + quantity;
            ItemFinalPriceString = finalPrice.ToString("0.00") + " RON";
        }
    }


    internal class NonEditDetailViewModel
    {

        OrderService orderService;

        public List<ItemDetail> OrderItems { get; private set; }
        public string TotalPriceString { get; private set; }
        public string StatusString { get; private set; }
        public DateOnly PickUpDate { get; private set; }
        public string PickUpDateString { get { return PickUpDate.ToString("yyyy.MM.dd"); } }

        public NonEditDetailViewModel(OrderService orderServ, int orderID)
        {
            orderService = orderServ;
            OrderItems = new();

            Order shownOrder = orderService.OrdersRepo.GetOrder(orderID);
            float totalPrice = 0f;

            foreach (var currentOrderEntry in shownOrder.ItemQuantitiesWithFinalPrice)
            {
                int itemID = currentOrderEntry.Key;
                int itemQuantity = currentOrderEntry.Value.Item1;
                float itemTotalPrice = currentOrderEntry.Value.Item2;

                Item currentItem = orderServ.ItemsRepo.GetItem(itemID);

                // TODO figure out, why does the image in XAML take FORWARD slashes
                // instead of BACKWARD slashes, like everything else in Windows
                int startingIndexOfImagePathSubstring = currentItem.ImagePath.IndexOf("\\Assets");
                string backwardSlashedImagePath = currentItem.ImagePath.Substring(startingIndexOfImagePathSubstring);
                string alteredImagePath = backwardSlashedImagePath.Replace("\\", "/");

                string itemDescription = currentItem.Name + " - " + currentItem.Producer;

                OrderItems.Add(
                    new ItemDetail(alteredImagePath, itemDescription,
                                    itemQuantity, itemTotalPrice)
                );

                totalPrice += itemTotalPrice;
            }

            TotalPriceString = totalPrice.ToString("0.00") + " RON";

            if (!shownOrder.IsExpired && !shownOrder.IsCompleted)
                StatusString = "Incomplete";
            else if (shownOrder.IsExpired)
                StatusString = "Expired";
            else
                StatusString = "Complete";

            PickUpDate = shownOrder.PickUpDate;
        }
    }
}
