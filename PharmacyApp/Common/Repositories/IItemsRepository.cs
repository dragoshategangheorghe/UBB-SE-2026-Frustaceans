using System;
using System.Collections.Generic;
using PharmacyApp.Models;

namespace PharmacyApp.Common.Repositories
{
    public interface IItemsRepository
    {
     
        void AddItem(string name, string producer, string category,
                    float price, int nrOfPills,
                    string label = "", string description = "", string imagePath = "..\\..\\Assets\\placeholder.png",
                    float discount = 0f);
        void RemoveItem(int idToBeRemoved);
        Item GetItem(int id);
        List<Item> GetAllItems();
        List<Item> GetItemsByName(string name);
        void UpdateItem(Item newItem);
        bool ItemExists(int id);
    }
}
