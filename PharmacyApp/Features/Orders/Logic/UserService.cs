using System;
using System.Collections.Generic;
using PharmacyApp.Common.Repositories;
using PharmacyApp.Models;

namespace PharmacyApp.Features.Orders.Logic
{
    public class UserService
    {
        public ISubstancesRepository SubstancesRepo { get; private set; }
        public IItemsRepository ItemsRepo { get; private set; }
        public IUsersRepository UsersRepo { get; private set; }
        public IOrdersRepository OrdersRepo { get; private set; }
        public User ActiveUser { get; private set; }

        public UserService()
        {
            SubstancesRepo = new SQLSubstancesRepository();
            ItemsRepo = new SQLItemsRepository();
            UsersRepo = new SQLUsersRepository();
            OrdersRepo = new SQLOrdersRepository();
            // TODO add the user parameter to the constructor
            ActiveUser = UsersRepo.GetUserByEmail("xyz@gmail.com");

            AddToBasket(5, 1);
            AddToBasket(7, 2);
        }

        public void AddToBasket(int itemId, int quantityToBuy)
        {
            ActiveUser.AddItemToBasket(itemId, quantityToBuy);
        }

        public void UpdateBasketItemQuantity(int itemId, int newQuantityToBuy)
        {
            ActiveUser.Basket[itemId] = newQuantityToBuy;

            if (ActiveUser.Basket[itemId] <= 0)
                ActiveUser.RemoveItemFromBasket(itemId);
        }

        public void RemoveFromBasket(int itemIdToRemove)
        {
            ActiveUser.RemoveItemFromBasket(itemIdToRemove);
        }

        public void PlaceOrderFromBasket(DateOnly chosenPickUpDate)
        {
            // for every item inside the basket, we have to verify
            // if we have enough boxes on stock that expire after
            // the chosen pick-up date

            // after validation we have to calculate the final price
            // for every item, to put it alongside the items in OrderItems

            Dictionary<int, Tuple<int, float>> itemInfoForOrder = new();

            foreach (KeyValuePair<int, int> basketEntry in ActiveUser.Basket)
            {
                Item currentItem = ItemsRepo.GetItem(basketEntry.Key);
                int currentItemQuantity = basketEntry.Value;
                int itemQuantityAtPickUpDate = currentItem.QuantityAtSpecifiedDate(chosenPickUpDate);

                if (currentItemQuantity > itemQuantityAtPickUpDate)
                    throw new ArgumentException("On " + chosenPickUpDate.ToString("yyyy.MM.dd") + ", " +
                                                "we will have only " + itemQuantityAtPickUpDate + " boxes " +
                                                "of " + currentItem.Name + " by " + currentItem.Producer + " " +
                                                "instead of " + currentItemQuantity + ".");

                float finalPrice = currentItem.Price *
                    (1 - currentItem.DiscountPercentage) *
                    (1 - ActiveUser.UserDiscounts[currentItem.Id]);

                itemInfoForOrder.Add(currentItem.Id, new Tuple<int, float>(currentItemQuantity, finalPrice));
            }

            OrdersRepo.AddOrderWithItems(ActiveUser.Id, chosenPickUpDate, itemInfoForOrder);

            // empty the basket after successfully creating the order

            ActiveUser.Basket.Clear();
        }

        public void FillBasketFromPrescription(string prescriptionId)
        {
            // implement
        }
    }
}
