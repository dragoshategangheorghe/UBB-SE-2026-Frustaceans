using PharmacyApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyApp.Common.Repositories
{
    public interface IUsersRepository
    {
        User GetUserById(int id);
        User GetUserByEmail(string email);

        void AddUser(User user);
        void UpdateUser(User user);

        void DeleteUser(int id);

        List<User> GetAllUsers();
    }
}