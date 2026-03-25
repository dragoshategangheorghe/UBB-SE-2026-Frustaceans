using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyApp.Common.Repositories
{
    internal static class SQLUtility
    {
        // NOTE this only works on MY computer
        // I'm sry :( but if we need to work with a local server,
        // then when you need to work on your feature and test smth,
        // change the string inside this function for yourself
        public static string GetConnectionString()
        {
            return "Data Source=" + Environment.MachineName + "\\SQLEXPRESS;Initial Catalog=Pharmacy;Integrated Security=true;TrustServerCertificate=true;";
        }
    }
}
