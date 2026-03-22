using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyApp.Models
{

    internal class Item
    {

        public string Id { get; private set; }
        public string Name { get; set; }
        public string Producer { get; set; }
        public float Price { get; set; }
        public int NumberOfPills { get; set; }
        public int Quantity { get; set; }
        public string Label { get; set; }
        public string Description { get; set; }
        public float DiscountPercentage { get; set; }
        public Dictionary<string, float> ActiveSubstances { get; private set; }
        public Dictionary<DateOnly, int> Batches { get; private set; }

        public Item(string id, string name, string producer,
                    float price, int nrOfPills, int quantity = 0,
                    string label = "", string description = "",
                    float discount = 0f)
        {
            Id = id;
            Name = name;
            Producer = producer;
            Price = price;
            NumberOfPills = nrOfPills;
            Quantity = quantity;
            Label = label;
            Description = description;
            DiscountPercentage = discount;
            ActiveSubstances = new Dictionary<string, float>();
            Batches = new Dictionary<DateOnly, int>();
        }

        // TODO should we really use booleans instead of exceptions for error handling?
        // TODO talk about whether putting an already existing substance into the dictionary
        public bool addActiveSubstance(string newSubstanceName, float concentration)
        {
            if (!ActiveSubstances.ContainsKey(newSubstanceName))
            {
                ActiveSubstances[newSubstanceName] = concentration;
                return true;
            }
            return false;
        }

        public bool removeActiveSubstance(string substanceName)
        {
            if (ActiveSubstances.ContainsKey(substanceName))
            {
                ActiveSubstances.Remove(substanceName);
                return true;
            }
            return false;
        }

        // TODO talk about putting a batch with already existing date
        public bool addNewBatch(DateOnly newExpirationDate, int nrOfPacks)
        {
            if (!Batches.ContainsKey(newExpirationDate))
            {
                Batches[newExpirationDate] = nrOfPacks;
                return true;
            }
            return false;
        }

        public bool removeBatch(DateOnly expirationDate)
        {
            if (Batches.ContainsKey(expirationDate))
            {
                Batches.Remove(expirationDate);
                return true;
            }
            return false;
        }
    }
}
