using PharmacyApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyApp.Features.Accounts.Logic
{
    public class UserAccountService
    {
        public User CurrentUser {  get; private set; }
        public UserAccountService(User user) {
            CurrentUser = user;
        }

        public void UpdateProfile(string newUsername, string newPhoneNumber)
        {
            if (string.IsNullOrEmpty(newUsername)) {
                
                    newUsername = CurrentUser.Email.Split("@")[0];
            }
            else if (!UserValidationService.isCorrectUsernameFormat(newUsername))
            {
                throw new Exception("Invalid new username");
            }

            if (!string.IsNullOrEmpty(newPhoneNumber) && !UserValidationService.isCorrectPhoneNumberFormat(newPhoneNumber))
            {
                throw new Exception("Invalid new phone number");
            }

            newPhoneNumber = string.IsNullOrEmpty(newPhoneNumber) ? CurrentUser.PhoneNumber : newPhoneNumber;

            //update the stuff in the DB and in-memory
            

        }


        public void ChangePassword(string oldPass, string newPass, string confirmPass) {
            if (!SecurityService.VerifyPassword(oldPass, CurrentUser.PasswordHash))
            {
                throw new Exception("Incorrect password");    
            }
            if (!UserValidationService.isCorrectPasswordFormat(newPass)) 
            {
                throw new Exception("New password must comply with the rules");
            }
            if (newPass != confirmPass)
            {
                throw new Exception("Passwords don't match");
            }
            var newPassHash = SecurityService.HashPassword(newPass);
            //TODO update in db and in-memory

        }






    }
}
