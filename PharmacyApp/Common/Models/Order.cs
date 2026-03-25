using System;
using System.Collections.Generic;

namespace PharmacyApp.Models
{
    public class Order
    {

        public int Id { get; private set; }
        public int ClientID { get; set; }
        public DateOnly PickUpDate { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsExpired { get; set; }
        public Dictionary<int, int> Items { get; private set; }


        public Order(int id, int clientId, DateOnly pickUpDate,
                     bool isCompleted = false, bool isExpired = false)
        {
            Id = id;
            ClientID = clientId;
            PickUpDate = pickUpDate;
            IsCompleted = isCompleted;
            IsExpired = isExpired;
            Items = new Dictionary<int, int>();
        }

        public void addItem(int newItemId, int quantity)
        {
            if (Items.ContainsKey(newItemId))
                throw new ArgumentException("Item #" + newItemId + " already exists in order");
            Items[newItemId] = quantity;
        }

        public void changeItemQuantity(int itemId, int newQuantity)
        {
            if (!Items.ContainsKey(itemId)) 
                throw new ArgumentException("Item #" + itemId + " doesn't exist");
            Items[itemId] = newQuantity;
        }

        public void removeItem(int itemId)
        {
            if (!Items.ContainsKey(itemId))
                throw new ArgumentException("Item #" + itemId + " doesn't exist");
            Items.Remove(itemId);
        }

    }
}
