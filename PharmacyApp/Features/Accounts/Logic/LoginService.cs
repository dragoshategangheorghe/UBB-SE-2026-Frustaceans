using PharmacyApp.Common.Repositories;
using PharmacyApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PharmacyApp.Features.Accounts.Logic
{
    public class LoginService
    {
        IUserRepository users;
        public LoginService(IUserRepository users)
        {
            this.users = users;
        }

        public bool isCorrectEmailFormat(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;
            email = email.Trim();

            //pattern: <text>@<text>.<text>
            string pattern = @"^.+@.+\..+";
            return Regex.IsMatch(email, pattern);
        }

        public User Login(string email,string password)
        {
            if (!isCorrectEmailFormat(email))
                throw new Exception("Not a valid e-mail");
            try
            {
                var user = users.GetByEmail(email);
                if (user.IsDisabled)
                    throw new Exception("Account disabled");
                if (!SecurityService.VerifyPassword(password, user.PasswordHash))
                    throw new Exception("Incorrect password");
                return user;
            }
            catch (Exception ) {
                throw new Exception("E-mail not found");
            }
        }

        

    }
}
