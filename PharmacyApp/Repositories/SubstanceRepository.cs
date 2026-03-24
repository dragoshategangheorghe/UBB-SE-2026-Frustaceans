using PharmacyApp.Interfaces;
using PharmacyApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyApp.Repositories
{
    internal class SubstanceRepository : ISubstanceRepository
    {
        public bool addSubstances(Substance newSubstance)
        {
            throw new NotImplementedException();
        }

        public bool changeSubstanceInfo(string name, Substance newSubstance)
        {
            throw new NotImplementedException();
        }

        public bool checkSubstanceExists(string name)
        {
            throw new NotImplementedException();
        }

        public Substance getSubstance(string name)
        {
            throw new NotImplementedException();
        }

        public bool removeSubstance(string name)
        {
            throw new NotImplementedException();
        }
    }
}
