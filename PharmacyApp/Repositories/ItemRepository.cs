using PharmacyApp.Interfaces;
using PharmacyApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyApp.Repositories
{
    internal class ItemRepository : IItemRepository
    {
        public bool addItem(Item newItem)
        {
            throw new NotImplementedException();
        }

        public bool changeItemInfo(string id, Item newItem)
        {
            throw new NotImplementedException();
        }

        public bool checkItemExists(string id)
        {
            throw new NotImplementedException();
        }

        public Item getItem(string id)
        {
            throw new NotImplementedException();
        }

        public List<Item> getItemsByName(string name)
        {
            throw new NotImplementedException();
        }

        public bool removeItem(string idToBeRemoved)
        {
            throw new NotImplementedException();
        }
    }
}
