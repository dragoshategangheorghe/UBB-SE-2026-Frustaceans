using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PharmacyApp.Models;

namespace PharmacyApp.Common.Repositories
{
    internal interface ISubstancesRepository
    {
        void AddSubstances(Substance newSubstance);
        void RemoveSubstance(string name);
        Substance GetSubstance(string name);
        void UpdateOrder(string name, Substance newSubstance);
        bool SubstanceExists(string name);
    }
}
