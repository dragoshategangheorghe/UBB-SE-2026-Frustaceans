using PharmacyApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyApp.Common.Repositories
{
    public interface IUserRepository
    {

        public User GetById(int id);
        public User GetByEmail(string email);

        public void Add(string email, string phoneNumber, string passwordHash, string username, bool discountNotifications, bool isDisabled=false, bool isAdmin=false, int loyaltyPoints=0);
        public void Update(User user);

        public void Delete(int id);

        public List<User> GetAll();


    }
}
