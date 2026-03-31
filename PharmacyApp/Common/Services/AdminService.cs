using PharmacyApp.Common.Repositories;
using PharmacyApp.Models;
using System;
using System.Collections.Generic;

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
                itemRepository.AddItemWithQuantity(newItem.Name, newItem.Producer, newItem.Category,
                                       newItem.Price, newItem.NumberOfPills, newItem.Quantity, newItem.ActiveSubstances, newItem.Batches,
                                       newItem.Label, newItem.Description, newItem.ImagePath,
                                       newItem.DiscountPercentage);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding item: {ex.Message}");
                return;

            }
        }

        public void AddItemWithQuantity(Item newItem)
        {
            try
            {
                validateItemAdd(newItem);
                itemRepository.AddItemWithQuantity(newItem.Name, newItem.Producer, newItem.Category,
                                       newItem.Price, newItem.NumberOfPills,
                                       newItem.Quantity, newItem.ActiveSubstances, newItem.Batches,
                                       newItem.Label, newItem.Description, newItem.ImagePath,
                                       newItem.DiscountPercentage);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding item: {ex.Message}");
                return;
            }
        }

        public void RemoveItem(int id)
        {
            itemRepository.RemoveItem(id);
        }

        public void UpdateItem(int id, Item updatedItem)
        {
            if (!itemRepository.ItemExists(id))
            {
                throw new ArgumentException("Item with the specified ID does not exist.");
            }

            // notification functionality
            Item prevItem = itemRepository.GetItem(id);
            if (prevItem.Quantity == 0 && updatedItem.Quantity > 0)
            {
                sendNewStockNotification(updatedItem);
            }
            updatedItem.Id = id;
            itemRepository.UpdateItem(updatedItem);
        }

        public void AddSubstance(Substance newSubstance)
        {
            substanceRepository.AddSubstance(newSubstance.Name, newSubstance.LethalDose, newSubstance.Description);
        }

        public void RemoveSubstance(Substance substance)
        {
            substanceRepository.RemoveSubstance(substance.Name);
        }

        public void UpdateSubstance(string name, Substance substance)
        {

            substanceRepository.UpdateSubstance(substance);
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

        public List<Item> GetExpiredItems()
        {
            List<Item> expiredItems = new List<Item>();
            List<Item> allItems = itemRepository.GetAllItems();
            DateOnly currentDate = DateOnly.FromDateTime(DateTime.Now);
            foreach (Item item in allItems)
            {
                foreach (var batch in item.Batches)
                {
                    if (batch.Key < currentDate)
                    {
                        expiredItems.Add(item);
                        break; // No need to check other batches for this item
                    }
                }
            }
            sendAboutToExprNotification();
            return expiredItems;
        }

        //change return to: list of notifications 
        public Notification sendAboutToExprNotification()
        {

            //List<Item> expiredItems = GetExpiredItems();
            //if (expiredItems.Count > 0) { 
            //    // send notification for each expired item
            //    foreach (Item item in expiredItems)
            //    {
            //        string message = $"Item with ID {item.Id} is about to expire!";
            //        Notification notification = new Notification("Expiration Alert", message);
            //        // send notification to the user (e.g., display it in the UI, send an email, etc.)
            //    }
            //}

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

        public List<Tuple<int, string, int>> GetTop30Items()
        {
            return itemRepository.GetTop30Items();
        }

        public Dictionary<string, int> GetTop20Substances()
        {
            return substanceRepository.GetTop20Substances();

        }
    }
}
