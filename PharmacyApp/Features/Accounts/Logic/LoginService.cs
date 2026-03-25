using PharmacyApp.Common.Exceptions;
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
        IUsersRepository users;
        public LoginService(IUsersRepository users)
        {
            this.users = users;
        }

        public User Login(string email,string password)
        {
            if (!UserValidationService.isCorrectEmailFormat(email))
                throw new Exception("Not a valid e-mail");
            try
            {
                var user = users.GetUserByEmail(email);
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

        public void Register(string email, string password, string confirmPassword, string username = "", string phoneNumber = "")
        {
            if (!UserValidationService.isCorrectEmailFormat(email))
                throw new Exception("Not a valid email");
            if (!UserValidationService.isCorrectPasswordFormat(password))
                throw new Exception("Incorrect format");
            if (password != confirmPassword)
                throw new Exception("Passwords don't match");

            try
            {
                var user = users.GetUserByEmail(email);
                throw new Exception("Email already linked to an account");
            }
            catch (UserNotFoundException) { }
            
            
            var passwordHash = SecurityService.HashPassword(password);
            var discountNotificationsSetting = false;
            users.AddUser(email,phoneNumber,passwordHash,username,discountNotificationsSetting);
        }

    }
}
