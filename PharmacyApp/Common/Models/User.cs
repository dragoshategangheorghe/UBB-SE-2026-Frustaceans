using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyApp.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Email {  get; set; }
        public string PhoneNumber { get; set; }
        public string PasswordHash { get; set; }
        public bool IsAdmin { get; set; }
        public string Username { get; set; }
        public bool IsDisabled { get; set; }

        public bool DiscountNotifications { get; set; }
        public int LoyaltyPoints { get; set; }




    }
}
