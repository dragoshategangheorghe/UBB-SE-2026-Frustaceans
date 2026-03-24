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
        public bool isCorrectPasswordFormat(string password)
        {
            if (string.IsNullOrWhiteSpace(password)) return false;

            //password should have:
            //-min 8 chars, at least one capital letter, at least one small letter,at least one digit, at least one of {!,@,#,%,^,*}, no other char types   
            string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#%^*])[A-Za-z\d!@#%^*]{8,}$";
            return Regex.IsMatch(password,pattern);
        }

        public bool isCorrectPhoneNumberFormat(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber)) return false;
            phoneNumber = phoneNumber.Trim();

            string pattern = @"^[0-9]+$";
            return Regex.IsMatch(phoneNumber,pattern);
        }

        public bool isCorrectUsernameFormat(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return false;
            username = username.Trim();

            string pattern = @"^[A-Za-z_]+$";
            return Regex.IsMatch(username,pattern);
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

        public void Register(string email, string password, string confirmPassword, string username = "", string phoneNumber = "")
        {
            if (!isCorrectEmailFormat(email))
                throw new Exception("Not a valid email");
            if (!isCorrectPasswordFormat(password))
                throw new Exception("Incorrect format");
            if (password != confirmPassword)
                throw new Exception("Passwords don't match");

            try
            {
                var user = users.GetByEmail(email);
                throw new Exception("Email already linked to an account");
            }
            catch (UserNotFoundException) { }
            var passwordHash = SecurityService.HashPassword(password);
            var discountNotificationsSetting = false;
            users.Add(email,phoneNumber,passwordHash,username,discountNotificationsSetting);
        }

    }
}
