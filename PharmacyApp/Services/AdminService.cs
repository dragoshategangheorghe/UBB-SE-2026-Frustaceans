using PharmacyApp.Interfaces;
using PharmacyApp.Models;
using PharmacyApp.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyApp.Services
{
    internal class AdminService
    {
        ItemRepository itemRepository;
        SubstanceRepository substanceRepository;
        public AdminService(ItemRepository itemRepo, SubstanceRepository substanceRepo)
        {
            this.itemRepository = itemRepo;
            this.substanceRepository = substanceRepo;
        }

        public void addItem(Item newItem)
        {
            try
            {
                validateItemAdd(newItem);
                itemRepository.addItem(newItem);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding item: {ex.Message}");
                //return;

            }
        }

        public void removeItem(string id)
        {
            itemRepository.removeItem(id);
        }

       public void updateItem(string id, Item item)
        {
            itemRepository.changeItemInfo(id, item);
        }

        public void addSubstance(Substance newSubstance)
        {
            substanceRepository.addSubstances(newSubstance);
        }

        public void removeSubstance(Substance substance)
        {
            substanceRepository.removeSubstance(substance.Name);
        }

        public void updateSubstance(string name, Substance substance)
        {
            substanceRepository.changeSubstanceInfo(name, substance);
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
                throw new Exception("Invalid item data. Please check the input and try again.");
            }
        }
    }
}
