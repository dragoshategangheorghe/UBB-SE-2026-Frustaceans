using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyApp.Models
{
    internal class Order
    {

        public string Id { get; private set; }
        public string ClientID { get; set; }
        public DateOnly PickUpDate { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsExpired { get; set; }
        // TODO should we store the prices at the time of the order?
        public Dictionary<string, int> Items { get; private set; }


        public Order(string id, string clientId, DateOnly pickUpDate,
                     bool isCompleted = false, bool isExpired = false)
        {
            Id = id;
            ClientID = clientId;
            PickUpDate = pickUpDate;
            IsCompleted = isCompleted;
            IsExpired = isExpired;
            Items = new Dictionary<string, int>();
        }

        // TODO already existing item is rejected or updated?
        public bool addItem(string newItemId, int quantity)
        {
            if (!Items.ContainsKey(newItemId))
            {
                Items.Add(newItemId, quantity);
                return true;
            }
            return false;
        }

        public bool removeItem(string newItemId)
        {
            if (Items.ContainsKey(newItemId))
            {
                Items.Remove(newItemId);
                return true;
            }
            return false;
        }

    }
}
