using System;
using System.Collections.Generic;

namespace PharmacyApp.Models
{

    public class Item
    {

        public int Id { get; private set; }
        public string Name { get; set; }
        public string Producer { get; set; }
        public float Price { get; set; }
        public string Category { get; set; }
        public string ImagePath { get; set; }
        public int NumberOfPills { get; set; }
        public int Quantity { get; set; }
        public string Label { get; set; }
        public string Description { get; set; }
        public float DiscountPercentage { get; set; }
        public Dictionary<string, float> ActiveSubstances { get; private set; }
        public Dictionary<DateOnly, int> Batches { get; private set; }


        public Item(int id, string name, string producer, string category,
                    float price, int nrOfPills, int quantity = 0,
                    string label = "", string description = "", string imagePath = "..\\Data\\placeholder.png",
                    float discount = 0f)
        {
            Id = id;
            Name = name;
            Producer = producer;
            Price = price;
            NumberOfPills = nrOfPills;
            Category = category;
            ImagePath = imagePath;
            Quantity = quantity;
            Label = label;
            Description = description;
            DiscountPercentage = discount;
            ActiveSubstances = new Dictionary<string, float>();
            Batches = new Dictionary<DateOnly, int>();
        }

        public void addActiveSubstance(string newSubstanceName, float concentration)
        {
            if (ActiveSubstances.ContainsKey(newSubstanceName))
                throw new ArgumentException(newSubstanceName + "is already inside the medication");
            ActiveSubstances[newSubstanceName] = concentration;
        }

        public void changeActiveSubstanceConcentration(string newSubstanceName, float newConcentration)
        {
            if (!ActiveSubstances.ContainsKey(newSubstanceName))
                throw new ArgumentException(newSubstanceName + "is not inside the medication");
            ActiveSubstances[newSubstanceName] = newConcentration;
        }

        public void removeActiveSubstance(string substanceName)
        {
            if (!ActiveSubstances.ContainsKey(substanceName))
                throw new ArgumentException(substanceName + "is not inside the medication");
            ActiveSubstances.Remove(substanceName);
        }

        public void addNewBatch(DateOnly newExpirationDate, int nrOfPacks)
        {
            if (Batches.ContainsKey(newExpirationDate))
                throw new ArgumentException("A batch with expiration date " + newExpirationDate.ToString() + " already exists");
            Batches[newExpirationDate] = nrOfPacks;
        }

        public void changeBatchNrOfPacks(DateOnly expirationDate, int newNrOfPacks)
        {
            if (!Batches.ContainsKey(expirationDate))
                throw new ArgumentException("A batch with expiration date " + expirationDate.ToString() + " doesn't exist");
            Batches[expirationDate] = newNrOfPacks;
        }

        public void removeBatch(DateOnly expirationDate)
        {
            if (!Batches.ContainsKey(expirationDate))
                throw new ArgumentException("A batch with expiration date " + expirationDate.ToString() + " doesn't exist");
            Batches.Remove(expirationDate);
        }
    }
}
