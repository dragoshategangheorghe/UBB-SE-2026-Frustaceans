using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyApp.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public float Price { get; set; }
        public int BoxPills { get; set; }
        public int Quantity { get; set; }
        public string Producer {  get; set; }
        public string Label { get; set; }
        public string Description { get; set; }

        public Dictionary<string, float> ActiveSubstances { get; set; }
        public Dictionary<DateOnly,int> Batches { get; set; }

        //needs IMAGE
        public float DiscountPercentage { get; set; }

        public Item(int id, string name, string category, float price, int boxPills, int quantity, string producer, string label, string description, float discountPercentage=0)
        {
            Id = id;
            Name = name;
            Category = category;
            Price = price;
            BoxPills = boxPills;
            Quantity = quantity;
            Producer = producer;
            Label = label;
            Description = description;
            DiscountPercentage = discountPercentage;
            Batches = new Dictionary<DateOnly, int> { };
            ActiveSubstances = new Dictionary<string, float> { };
        }

        //NEEDS IMPLEMENTATIONS
        public bool addActiveSubstance(Substance NewSubstance, float Concentration)
        {
            return false;
        }
        public bool removeActiveSubstance(string SubstanceName)
        {
            return false;
        }

        public bool addNewBatch(DateOnly ExpirationDate, int NumberOfPacks)
        {
            return false;
        }

        public bool removeBatch(DateOnly ExpirationDate)
        {
            return false;
        }
    }
}
