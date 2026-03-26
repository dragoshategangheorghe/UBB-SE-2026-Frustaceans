using PharmacyApp.Common.Repositories;
using PharmacyApp.Models;
using System;

namespace PharmacyApp.Common.Services
{
    internal class AdminService
    {
        IItemsRepository itemRepository;
        ISubstancesRepository substanceRepository;

        public AdminService()
        {
            this.itemRepository = new SQLItemsRepository();
            this.substanceRepository = new SQLSubstancesRepository();
        }
        public AdminService(IItemsRepository itemRepo, ISubstancesRepository substanceRepo)
        {
            this.itemRepository = itemRepo;
            this.substanceRepository = substanceRepo;
        }

        public void AddItem(Item newItem)
        {
            try
            {
                validateItemAdd(newItem);
                itemRepository.AddItem(newItem.Name, newItem.Producer, newItem.Category,
                                       newItem.Price, newItem.NumberOfPills,
                                       newItem.Label, newItem.Description, newItem.ImagePath,
                                       newItem.DiscountPercentage);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding item: {ex.Message}");
                //return;

            }
        }

        public void RemoveItem(int id)
        {
            itemRepository.RemoveItem(id);
        }

       public void UpdateItem(int id, Item updatedItem)
        {
            if (!itemRepository.ItemExists(id)) { 
                throw new ArgumentException("Item with the specified ID does not exist.");
            }

            // notification functionality
            Item prevItem = itemRepository.GetItem(id);
            if(prevItem.Quantity == 0 && updatedItem.Quantity>0)
            {
                sendNewStockNotification(updatedItem);
            }

            itemRepository.UpdateItem(updatedItem);
        }

        public void AddSubstance(Substance newSubstance)
        {
            substanceRepository.AddSubstance(newSubstance.Name, newSubstance.LethalDose, newSubstance.Description);
        }

        public void RemoveSubstance(Substance substance)
        {
            // TODO: add a if substance exists check
            substanceRepository.RemoveSubstance(substance.Name);
        }

        public void UpdateSubstance(string name, Substance substance)
        {
            //?????????????????????
            //substanceRepository.UpdateSubstance(name, substance);
        }

        public Notification sendNewStockNotification(Item item)
        {
            string message = $"The item {item.Name} is back in stock with quantity {item.Quantity}," +
                $"number of pills {item.NumberOfPills!}," +
                $"producer {item.Producer}";
            Notification notification = new Notification("Stock Alert", "New item back in stock!");
            // :D i am lost

            return notification;
        }

        //change return to: list of notifications 
        public Notification sendAboutToExprNotification()
        {

            //getall items
            //sort by date ascending
            // while loop through items and check if the date is still valid, if not send notification

            //string message = $"Item with ID {items[i].id} is about to expire!";
            //Notification notification = new Notification("Expiration Alert", message);
            Notification notification = new Notification("Expiration Alert", "Some items are about to expire!");

            return notification;

        }

        public void validateItemAdd(Item item)
        {
            if (item.Name == "" ||
                item.Producer == "" ||
                item.Price <= 0 ||
                item.NumberOfPills <= 0 ||
                item.Quantity < 0 ||
                item.DiscountPercentage < 0 ||
                item.ActiveSubstances.Count == 0)
            {
                throw new ArgumentException("Invalid item data. Please check the input and try again.");
            }
        }
        
    }
}
