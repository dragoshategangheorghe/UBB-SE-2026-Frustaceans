using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PharmacyApp.Models;

namespace PharmacyApp.Common.Repositories
{
    internal interface ISubstanceRepository
    {
        bool addSubstances(Substance newSubstance);
        bool removeSubstance(string name);
        Substance getSubstance(string name);
        bool changeSubstanceInfo(string name, Substance newSubstance);
        bool checkSubstanceExists(string name);
    }
}
