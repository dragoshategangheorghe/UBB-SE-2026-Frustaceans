using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PharmacyApp.Models;
using PharmacyApp.Common.Repositories;
using PharmacyApp.Features.Orders.Logic;
using Microsoft.UI.Xaml;

namespace PharmacyApp.Features.Orders.ViewModels
{

    public class BasketItem
    {
        float finalPrice;
        float finalPriceDiscounted;

        public string ItemThumbnailImagePath { get; private set; }
        public string ItemDescriptionInBasket { get; private set; }
        public int ItemQuantityInBasket { get; set; }
        public float ItemActiveDiscount { get; private set; }
        public float ItemActiveUserDiscount { get; private set; }
        public float InitialPricePerBox { get; private set; }
        public float FinalPriceBeforeDiscount 
        { 
            get
            {
                finalPrice = InitialPricePerBox * ItemQuantityInBasket;
                return finalPrice;
            }
            set { }
        }
        public float FinalPriceAfterDiscount
        { 
            get
            {
                finalPriceDiscounted = finalPrice * (1 - ItemActiveDiscount) * (1 - ItemActiveUserDiscount);
                decimal temp = Math.Truncate((decimal)finalPriceDiscounted * 100) / 100;
                finalPriceDiscounted = (float)temp;
                return finalPriceDiscounted;
            }
            set { }
        }

        public BasketItem(string imagePath, string description, int quantity,
                          float activeDiscount, float userDiscount, float initialPrice)
        {
            ItemThumbnailImagePath = imagePath;
            ItemDescriptionInBasket = description;
            ItemQuantityInBasket = quantity;
            ItemActiveDiscount = activeDiscount;
            ItemActiveUserDiscount = userDiscount;
            InitialPricePerBox = initialPrice;

            finalPrice = InitialPricePerBox * ItemQuantityInBasket;

            // TODO rewrite this (and in the getter as well)
            finalPriceDiscounted = finalPrice * (1 - ItemActiveDiscount) * (1 - ItemActiveUserDiscount);
            decimal temp = Math.Truncate((decimal)finalPriceDiscounted * 100) / 100;
            finalPriceDiscounted = (float)temp;

        }
    }

    public class BasketViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged = delegate { };

        public List<BasketItem> BasketItems { get; }

        public BasketViewModel(UserService userService)
        {
            Dictionary<int,int> itemsInBasket = userService.ActiveUser.Basket;
            BasketItems = new();

            foreach (KeyValuePair<int,int> item in itemsInBasket)
            {
                Item currentItem = userService.ItemsRepo.GetItem(item.Key);

                float userDiscount;
                if (userService.ActiveUser.UserDiscounts.ContainsKey(currentItem.Id))
                    userDiscount = userService.ActiveUser.UserDiscounts[currentItem.Id];
                else
                    userDiscount = 0f;

                // TODO figure out, why does the image in XAML take FORWARD slashes
                // instead of BACKWARD slashes, like everything else in Windows
                int startingIndexOfImagePathSubstring = currentItem.ImagePath.IndexOf("\\Assets");
                string backwardSlashedImagePath = currentItem.ImagePath.Substring(startingIndexOfImagePathSubstring);
                string alteredImagePath = backwardSlashedImagePath.Replace("\\", "/");

                BasketItem basketItem = new(
                    alteredImagePath,
                    currentItem.Name,
                    item.Value,
                    currentItem.DiscountPercentage,
                    userDiscount,
                    currentItem.Price);
                BasketItems.Add(basketItem);
            }
        }

    }
}
