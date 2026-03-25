using Microsoft.Data.SqlClient;
using PharmacyApp.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PharmacyApp.Common.Repositories
{
    public class SQLUsersRepository: IUsersRepository
    {
        public SQLUsersRepository() { }

        public void AddUser(string email, string phoneNumber, string passwordHash, string username, bool discountNotifications, bool isDisabled=false, bool isAdmin=false, int loyaltyPoints=0)
        {
            string connString = SQLUtility.GetConnectionString();
            string insertNewUserString =
                "INSERT INTO Users VALUES " +
                $"('{email}', '{phoneNumber}', '{passwordHash}', '{isDisabled}', '{isAdmin}', '{username}', '{discountNotifications}', {loyaltyPoints})";

            using SqlConnection conn = new SqlConnection(connString);

            SqlCommand insertNewUserCommand = new SqlCommand(insertNewUserString, conn);

            conn.Open();
            insertNewUserCommand.ExecuteNonQuery();
        }

        
        public List<User> GetAllUsers()
        {
            string connString = SQLUtility.GetConnectionString();

            string selectUsersString = $"SELECT * FROM Users";
            
            

            using SqlConnection conn = new SqlConnection(connString);

            SqlDataAdapter selectUsersAdapter = new SqlDataAdapter(selectUsersString, conn);

            DataSet usersDataFromDB = new DataSet(); // all the tables will be put in this DataSet

            conn.Open();

            selectUsersAdapter.Fill(usersDataFromDB, "Users"); // this uses the connection, fills the data from the select query into a table called "Users"

            if (usersDataFromDB.Tables["Users"].Rows.Count == 0)
                return new List<User>(); // empty list


            List<User> users = new List<User>(); // this list will contain all users 

            // iterate each user and find their data
            foreach (DataRow userRow in usersDataFromDB.Tables["Users"].Rows)
            {
                int userID = (int)userRow["userId"];

                //NOTE: userData, not usersData...
                DataSet userDataFromDB = new DataSet(); // all the tables relevant to THIS specific user will be put here

                // query their period tracker
                string selectPeriodTrackersString = $"SELECT * FROM PeriodTrackers WHERE userId={userID}";
                SqlDataAdapter selectPeriodTrackersAdapter = new SqlDataAdapter(selectPeriodTrackersString, conn);
                selectPeriodTrackersAdapter.Fill(userDataFromDB, "PeriodTrackers");

                DataRow userPeriodTrackerRow = userDataFromDB.Tables["PeriodTrackers"].Rows[0]; // their period tracker, only one

                User resultUser = new User((int)userRow["userId"], (string)userRow["email"],
                    (string)userRow["phoneNumber"],
                    (string)userRow["passwordHash"], (bool)userRow["isAdmin"], (bool)userRow["isDisabled"],
                    (string)userRow["username"], (bool)userRow["discountNotifications"],
                    (int)userRow["loyaltyPoints"],
                    DateOnly.FromDateTime((DateTime)userPeriodTrackerRow["startPeriodDate"]), (int)userPeriodTrackerRow["cycleDays"],
                    (int)userPeriodTrackerRow["periodLasts"], (int)userPeriodTrackerRow["PMSOption"],
                    (bool)userPeriodTrackerRow["wantsToBePregnant"]);

                // now find, using the same open connection, their notifications, discounts and period notes
                string selectUserNotificationsString = $"SELECT * FROM UserNotifications WHERE userId={userID}";
                string selectUserDiscountsString = $"SELECT * FROM UserDiscounts WHERE userId={userID}";
                string selectPeriodNotesString = $"SELECT * FROM PeriodNotes WHERE userId={userID}";

                SqlDataAdapter selectUserNotificationsAdapter = new SqlDataAdapter(selectUserNotificationsString, conn);
                SqlDataAdapter selectUserDiscountsAdapter = new SqlDataAdapter(selectUserDiscountsString, conn);
                SqlDataAdapter selectPeriodNotesAdapter = new SqlDataAdapter(selectPeriodNotesString, conn);

                selectUserNotificationsAdapter.Fill(userDataFromDB, "UserNotifications");
                selectUserDiscountsAdapter.Fill(userDataFromDB, "UserDiscounts");
                selectPeriodNotesAdapter.Fill(userDataFromDB, "PeriodNotes");

                // now  give them their notifications and discounts
                foreach (DataRow notificationsRow in userDataFromDB.Tables["UserNotifications"].Rows)
                {
                    if ((bool)notificationsRow["favouriteItem"]) // add that item to the user's favorite items if they checked it
                        resultUser.addFavoriteItem((int)notificationsRow["itemId"]);
                    if ((bool)notificationsRow["stockAlert"]) // same principle
                        resultUser.addStockAlert((int)notificationsRow["itemId"]);
                    // if both are false, the item is NOT in the database at all
                }

                foreach (DataRow discountRow in userDataFromDB.Tables["UserDiscounts"].Rows)
                {
                    // a row is only present here if the user has personal discounts on some items
                    resultUser.addUserDiscount((int)discountRow["itemId"], (float)(decimal)discountRow["itemDiscountPercentage"]);
                }

                // now add the user's period notes
                foreach (DataRow periodNoteRow in userDataFromDB.Tables["PeriodNotes"].Rows)
                {
                    resultUser.addPeriodNote((int)periodNoteRow["noteId"], (string)periodNoteRow["noteBody"], (bool)periodNoteRow["isDone"]);
                }


                users.Add(resultUser); // finally add the user to the users list
            }

            return users;
        }

        public User GetUserByEmail(string email)
        {
            string connString = SQLUtility.GetConnectionString();
            string selectUserString = $"SELECT * FROM Users WHERE email={email}";

            using SqlConnection conn = new SqlConnection(connString);

            SqlDataAdapter selectUserAdapter = new SqlDataAdapter(selectUserString, conn);

            DataSet userDataFromDB = new DataSet();

            conn.Open();
            selectUserAdapter.Fill(userDataFromDB, "Users");

            if (userDataFromDB.Tables["Users"].Rows.Count == 0)
                throw new ArgumentException("User with E-Mail " + email + " does NOT exist.");

            DataRow userRow = userDataFromDB.Tables["Items"].Rows[0];


            return GetUserById((int)userRow["userId"]); // please ignore the inefficiency of this, better than line duplications
        }

        public User GetUserById(int id)
        {
            string connString = SQLUtility.GetConnectionString();

            string selectUserString = $"SELECT * FROM Users WHERE userId={id}";
            string selectPeriodTrackerString = $"SELECT * FROM PeriodTrackers WHERE userId={id}";
            string selectUserNotificationsString = $"SELECT * FROM UserNotifications WHERE userId={id}";
            string selectUserDiscountsString = $"SELECT * FROM UserDiscounts WHERE userId={id}";
            string selectPeriodNotesString = $"SELECT * FROM PeriodNotes WHERE userId={id}";

            using SqlConnection conn = new SqlConnection(connString);

            SqlDataAdapter selectUserAdapter = new SqlDataAdapter(selectUserString, conn);
            SqlDataAdapter selectPeriodTrackerAdapter = new SqlDataAdapter(selectPeriodTrackerString, conn);
            SqlDataAdapter selectUserNotificationsAdapter = new SqlDataAdapter(selectUserNotificationsString, conn);
            SqlDataAdapter selectUserDiscountsAdapter = new SqlDataAdapter(selectUserDiscountsString, conn);
            SqlDataAdapter selectPeriodNotesAdapter = new SqlDataAdapter(selectPeriodNotesString, conn);

            DataSet userDataFromDB = new DataSet(); // all the tables will be put in this DataSet

            conn.Open();

            selectUserAdapter.Fill(userDataFromDB, "Users"); // this uses the connection, fills the data from the select query into a table called "Users"
            selectPeriodTrackerAdapter.Fill(userDataFromDB, "PeriodTrackers");
            selectUserNotificationsAdapter.Fill(userDataFromDB, "UserNotifications");
            selectUserDiscountsAdapter.Fill(userDataFromDB, "UserDiscounts");
            selectPeriodNotesAdapter.Fill(userDataFromDB, "PeriodNotes");

            if (userDataFromDB.Tables["Users"].Rows.Count == 0)
                throw new ArgumentException("User with ID " + id + " does NOT exist.");


            // create the user from the unique user and period tracker rows
            DataRow userRow = userDataFromDB.Tables["Users"].Rows[0]; // the user with this id
            DataRow userPeriodTrackerRow = userDataFromDB.Tables["PeriodTrackers"].Rows[0]; // their period tracker


            User resultUser = new User((int)userRow["userId"], (string)userRow["email"],
                (string)userRow["phoneNumber"],
                (string)userRow["passwordHash"], (bool)userRow["isAdmin"], (bool)userRow["isDisabled"],
                (string)userRow["username"], (bool)userRow["discountNotifications"],
                (int)userRow["loyaltyPoints"],
                DateOnly.FromDateTime((DateTime)userPeriodTrackerRow["startPeriodDate"]),(int)userPeriodTrackerRow["cycleDays"],
                (int)userPeriodTrackerRow["periodLasts"],(int)userPeriodTrackerRow["PMSOption"],
                (bool)userPeriodTrackerRow["wantsToBePregnant"]);

            // now  give them their notifications and discounts
            foreach (DataRow notificationsRow in userDataFromDB.Tables["UserNotifications"].Rows)
            {
                if ((bool)notificationsRow["favouriteItem"]) // add that item to the user's favorite items if they checked it
                    resultUser.addFavoriteItem((int)notificationsRow["itemId"]);
                if ((bool)notificationsRow["stockAlert"]) // same principle
                    resultUser.addStockAlert((int)notificationsRow["itemId"]); 
                // if both are false, the item is NOT in the database at all
            }

            foreach (DataRow discountRow in userDataFromDB.Tables["UserDiscounts"].Rows)
            {
                // a row is only present here if the user has personal discounts on some items
                resultUser.addUserDiscount((int)discountRow["itemId"], (float)(decimal)discountRow["itemDiscountPercentage"]);
            }

            // now add the user's period notes
            foreach (DataRow periodNoteRow in userDataFromDB.Tables["PeriodNotes"].Rows)
            {
                resultUser.addPeriodNote((int)periodNoteRow["noteId"], (string)periodNoteRow["noteBody"], (bool)periodNoteRow["isDone"]);
            }


            return resultUser;
        }

        public void UpdateUser(User user)
        {
            throw new NotImplementedException();
        }

        public bool UserExists(string email)
        {
            string connString = SQLUtility.GetConnectionString();
            string selectUserString = $"SELECT * FROM Users WHERE email='{email}'";

            using SqlConnection conn = new SqlConnection(connString);

            SqlDataAdapter selectUserAdapter = new SqlDataAdapter(selectUserString, conn);

            DataSet userDataFromDB = new DataSet();

            conn.Open();
            selectUserAdapter.Fill(userDataFromDB, "Users");

            if (userDataFromDB.Tables["Users"].Rows.Count > 0)
                return true;
            return false;
        }

        public bool UserExists(int id)
        {
            string connString = SQLUtility.GetConnectionString();
            string selectUserString = $"SELECT * FROM Users WHERE userId={id}";

            using SqlConnection conn = new SqlConnection(connString);

            SqlDataAdapter selectUserAdapter = new SqlDataAdapter(selectUserString, conn);

            DataSet userDataFromDB = new DataSet();

            conn.Open();
            selectUserAdapter.Fill(userDataFromDB, "Users");

            if (userDataFromDB.Tables["Users"].Rows.Count > 0)
                return true;
            return false;
        }
    }
}
