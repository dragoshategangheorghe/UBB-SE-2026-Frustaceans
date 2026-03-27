using System;
using System.Collections.Generic;
using PharmacyApp.Common.Repositories;
using PharmacyApp.Models;

namespace PharmacyApp.Features.Orders.Logic
{
    public class UserService
    {
        public SQLSubstancesRepository SubstancesRepo { get; private set; }
        public SQLItemsRepository ItemsRepo { get; private set; }
        public SQLUsersRepository UsersRepo { get; private set; }
        public SQLOrdersRepository OrdersRepo { get; private set; }
        public User ActiveUser { get; private set; }

        public UserService()
        {
            SubstancesRepo = new();
            ItemsRepo = new();
            UsersRepo = new();
            OrdersRepo = new();
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

    }
}
