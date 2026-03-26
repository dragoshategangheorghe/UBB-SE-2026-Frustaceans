//using PharmacyApp.Common.Repositories;
//using PharmacyApp.Models;
//using System;

namespace PharmacyApp.Common.Services
{
    internal class AdminService
    {
        /*
        IItemRepository itemRepository;
        ISubstanceRepository substanceRepository;
        public AdminService(IItemRepository itemRepo, ISubstanceRepository substanceRepo)
        {
            this.itemRepository = itemRepo;
            this.substanceRepository = substanceRepo;
        }

//        public void addItem(Item newItem)
//        {
//            try
//            {
//                validateItemAdd(newItem);
//                itemRepository.addItem(newItem);
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Error adding item: {ex.Message}");
//                //return;

//            }
//        }

//        public void removeItem(string id)
//        {
//            itemRepository.removeItem(id);
//        }

//       public void updateItem(string id, Item updatedItem)
//        {
//            itemRepository.changeItemInfo(id, updatedItem);

//            // notification functionality
//            Item prevItem = itemRepository.getItem(id);
//            if(prevItem.Quantity == 0 && updatedItem.Quantity>0)
//            {
//                sendNewStockNotification(updatedItem);
//            }
//        }

//        public void addSubstance(Substance newSubstance)
//        {
//            substanceRepository.addSubstances(newSubstance);
//        }

//        public void removeSubstance(Substance substance)
//        {
//            substanceRepository.removeSubstance(substance.Name);
//        }

//        public void updateSubstance(string name, Substance substance)
//        {
//            substanceRepository.changeSubstanceInfo(name, substance);
//        }

//        public Notification sendNewStockNotification(Item item)
//        {
//            string message = $"The item {item.Name} is back in stock with quantity {item.Quantity}," +
//                $"number of pills {item.NumberOfPills!}," +
//                $"producer {item.Producer}";
//            Notification notification = new Notification("Stock Alert", "New item back in stock!");
//            // :D i am lost

//            return notification;
//        }

//        //change return to: list of notifications 
//        public Notification sendAboutToExprNotification()
//        {

//            //getall items
//            //sort by date ascending
//            // while loop through items and check if the date is still valid, if not send notification

//            //string message = $"Item with ID {items[i].id} is about to expire!";
//            //Notification notification = new Notification("Expiration Alert", message);
//            Notification notification = new Notification("Expiration Alert", "Some items are about to expire!");

//            return notification;

//        }

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
                throw new Exception("Invalid item data. Please check the input and try again.");
            }
        }
        */
    }
}
