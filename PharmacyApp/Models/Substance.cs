using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyApp.Models
{
    public class Substance
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public float lethalDose { get; set; }
        public Substance(string name, string description, float lethalDose)
        {
            Name = name;
            Description = description;
            this.lethalDose = lethalDose;
        }

    }
}
