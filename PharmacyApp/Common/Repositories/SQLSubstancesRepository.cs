using Microsoft.Data.SqlClient;
using PharmacyApp.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
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
            string connString = SQLUtility.GetConnectionString();
            string selectSubstanceQueryString = $"SELECT * FROM Substances WHERE name='{name}'";

            using SqlConnection conn = new SqlConnection(connString);

            SqlDataAdapter selectSubstanceAdapter = new SqlDataAdapter(selectSubstanceQueryString, conn);

            DataSet substanceDataFromDB = new DataSet();

            conn.Open();
            selectSubstanceAdapter.Fill(substanceDataFromDB, "Substances");

            if (substanceDataFromDB.Tables["Substances"].Rows.Count == 0)
                throw new ArgumentException("Substance " + name + " does NOT exist.");


            DataRow substanceDataRow = substanceDataFromDB.Tables["Substances"].Rows[0];
            return new Substance((string)substanceDataRow["name"], (float)(decimal)substanceDataRow["lethalDose"],
                (string)substanceDataRow["description"]);
        }


        public List<Substance> GetAllSubstances()
        {
            List<Substance> allSubstances = new List<Substance>();
            string connString = SQLUtility.GetConnectionString();
            string selectAllSubstancesQueryString = $"SELECT * FROM Substances";

            using SqlConnection conn = new SqlConnection(connString);
            SqlDataAdapter selectSubstancesAdapter = new SqlDataAdapter(selectAllSubstancesQueryString, conn);
            DataSet substanceDataFromDB = new DataSet();

            conn.Open();
            selectSubstancesAdapter.Fill(substanceDataFromDB, "Substances");

            foreach (DataRow substanceDataRow in substanceDataFromDB.Tables["Substances"].Rows)
            {
                Substance newSubstance = new Substance(
                    (string)        substanceDataRow["name"], 
                    (float)(decimal)substanceDataRow["lethalDose"],
                    (string)        substanceDataRow["description"]);

                allSubstances.Add(newSubstance);
            }

            return allSubstances;
        }


        public void RemoveSubstance(string name)
        {
            if (!SubstanceExists(name))
                throw new ArgumentException("Substance " + name + " does NOT exist.");

            string connString = SQLUtility.GetConnectionString();
            string deleteSubstanceCommandString = $"DELETE FROM Substances WHERE name='{name}'";
            string deleteActiveSubstancesCommandString = $"DELETE FROM ItemSubstances WHERE name='{name}'";
            using SqlConnection conn = new SqlConnection(connString);

            conn.Open();
            SqlCommand deleteActiveSubstancesCommand = new SqlCommand(deleteActiveSubstancesCommandString, conn);
            deleteActiveSubstancesCommand.ExecuteNonQuery();
            SqlCommand deleteSubstanceCommand = new SqlCommand(deleteSubstanceCommandString, conn);
            deleteSubstanceCommand.ExecuteNonQuery();
        }

        public void UpdateSubstance(Substance substance)
        {
            if (!SubstanceExists(substance.Name))
                throw new ArgumentException("Substance " + substance.Name + "does NOT exist.");

            string connString = SQLUtility.GetConnectionString();
            string updateSubstanceCommandString = $"UPDATE Substances " +
                                                  $"SET lethalDose = {substance.LethalDose}, " +
                                                  $"description = '{substance.Description}' " +
                                                  $"WHERE name = '{substance.Name}'";

            using SqlConnection conn = new SqlConnection(connString);

            SqlCommand updateSubstanceCommand = new SqlCommand(updateSubstanceCommandString, conn);
            conn.Open();
            updateSubstanceCommand.ExecuteNonQuery();
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
