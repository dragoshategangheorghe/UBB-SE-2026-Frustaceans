using PharmacyApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyApp.Interfaces
{
    public interface IUserRepository
    {

        public User GetById(int id);
        public User GetByEmail(string email);

        public void Add(User user);
        public void Update(User user);

        public void Delete(int id);

        public List<User> GetAll();


    }
}
