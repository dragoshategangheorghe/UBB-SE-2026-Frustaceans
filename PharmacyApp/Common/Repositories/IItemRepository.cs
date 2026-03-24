using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PharmacyApp.Models;

namespace PharmacyApp.Common.Repositories
{
    internal interface IItemRepository
    {
        bool addItem(Item newItem);
        bool removeItem(string idToBeRemoved);
        Item getItem(string id);
        List<Item> getItemsByName(string name);
        bool changeItemInfo(string id, Item newItem);
        bool checkItemExists(string id);
    }
}
