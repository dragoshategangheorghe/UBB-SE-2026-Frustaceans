using PharmacyApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System;
using User = PharmacyApp.Models.User;

namespace PharmacyApp.Features.Accounts.Logic
{
    public class AdminAccountService
    {

        private List<User> users;
        public List<User> GetAllUsers()
        {
            return users;
        }

        public AdminAccountService(List<User> users)
        {
            this.users = users;
        }

        public List<User> SearchUsers(string query)
        {
            query = query.Trim();
            if (query.StartsWith("id:"))
            {
                int id;
                try
                {
                    id = int.Parse(query.Substring(3));
                    return users.Where(u => u.Id == id).ToList();
                } catch (FormatException) { }

            }

            if (query.StartsWith("username:"))
            {
                string username = query.Substring(9);
                return users.Where(u => u.Username.Contains(username)).ToList();
            }

            if (query.StartsWith("mail:"))
            {
                string mail = query.Substring(5);
                return users.Where(u => u.Email.Contains(mail)).ToList();
            }

            return users;
        }

        public void PromoteToAdmin(User client)
        {
            if (client.IsAdmin || client.IsDisabled)
            {
                return;
            }
            client.IsAdmin = true;
        }
        public void DisableAccount(User client)
        {
            if (client.IsAdmin || client.IsDisabled)
                return;
            client.IsDisabled = true;
        }

    }
}
