using Microsoft.Data.SqlClient;
using PharmacyApp.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyApp.Common.Repositories
{
    internal class SQLSubstancesRepository : ISubstancesRepository
    {
        public void AddSubstance(string name, float lethalDose, string description)
        {
            if (SubstanceExists(name))
                throw new ArgumentException("Substance " + name + " exists already.");

            string connString = SQLUtility.GetConnectionString();
            string insertSubstanceCommandString =
                $"INSERT INTO Substances VALUES ('{name}', {lethalDose}, '{description}')";

            using SqlConnection conn = new SqlConnection(connString);

            SqlCommand insertSubstanceCommand = new SqlCommand(insertSubstanceCommandString, conn);

            conn.Open();
            insertSubstanceCommand.ExecuteNonQuery();
        }

        public Substance GetSubstance(string name)
        {
            throw new NotImplementedException();
        }

        public void RemoveSubstance(string name)
        {
            throw new NotImplementedException();
        }

        public bool SubstanceExists(string name)
        {
            string connString = SQLUtility.GetConnectionString();
            string selectSubstanceQueryString = $"SELECT * FROM Substances WHERE name='{name}'";

            using SqlConnection conn = new SqlConnection(connString);

            SqlDataAdapter selectSubstanceAdapter = new SqlDataAdapter(selectSubstanceQueryString, conn);

            DataSet substanceDataFromDB = new DataSet();

            conn.Open();
            selectSubstanceAdapter.Fill(substanceDataFromDB, "Substances");

            if (substanceDataFromDB.Tables["Substances"].Rows.Count > 0)
                return true;
            return false;
        }

    }
}
