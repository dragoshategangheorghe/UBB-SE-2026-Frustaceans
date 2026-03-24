using System;
using System.Collections.Generic;
using PharmacyApp.Models;

namespace PharmacyApp.Common.Repositories
{
    internal interface IItemRepository
    {
        void addItem(string name, string producer, string category,
                    float price, int nrOfPills, int quantity = 0,
                    string label = "", string description = "", string imagePath = "..\\..\\Assets\\placeholder.png",
                    float discount = 0f);
        void removeItem(int idToBeRemoved);
        Item getItem(int id);
        List<Item> getItemsByName(string name);
        void changeItemInfo(int id, Item newItem);
        bool checkItemExists(int id);
    }
}
