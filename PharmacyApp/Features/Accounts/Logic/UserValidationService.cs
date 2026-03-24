using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PharmacyApp.Features.Accounts.Logic
{
    public class UserValidationService
    {
        public static bool isCorrectEmailFormat(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;
            email = email.Trim();

            //pattern: <text>@<text>.<text>
            string pattern = @"^.+@.+\..+";
            return Regex.IsMatch(email, pattern);
        }
        public static bool isCorrectPasswordFormat(string password)
        {
            if (string.IsNullOrWhiteSpace(password)) return false;

            //password should have:
            //-min 8 chars, at least one capital letter, at least one small letter,at least one digit, at least one of {!,@,#,%,^,*}, no other char types   
            string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#%^*])[A-Za-z\d!@#%^*]{8,}$";
            return Regex.IsMatch(password, pattern);
        }

        public static bool isCorrectPhoneNumberFormat(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber)) return false;
            phoneNumber = phoneNumber.Trim();

            string pattern = @"^[0-9]+$";
            return Regex.IsMatch(phoneNumber, pattern);
        }

        public static bool isCorrectUsernameFormat(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return false;
            username = username.Trim();

            string pattern = @"^[A-Za-z_]+$";
            return Regex.IsMatch(username, pattern);
        }

    }
}
