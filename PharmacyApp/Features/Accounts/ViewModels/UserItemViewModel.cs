using PharmacyApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyApp.Features.Accounts.ViewModels
{
    public class UserItemViewModel
    {
        private User _user;

        public UserItemViewModel(User user)
        {
            _user = user;
        }

        public User User => _user;

        public string Email => _user.Email;
        public string Username => string.IsNullOrEmpty(_user.Username) ? "(no username)" : _user.Username;
        public string PhoneNumber => string.IsNullOrEmpty(_user.PhoneNumber) ? "(no phone)" : _user.PhoneNumber;

        public bool IsAdmin => _user.IsAdmin;
        public bool IsDisabled => _user.IsDisabled;

        public double Opacity => IsDisabled ? 0.7 : 1.0;

        public string Background
        {
            get
            {
                if (IsDisabled) return "#E8F5E9";
                if (IsAdmin) return "#FFF8E1";
                return "#F4F8F6";             
            }
        }

        public bool ShowPromote => !IsAdmin && !IsDisabled;

        public bool ShowDisable => !IsAdmin && !IsDisabled;

        public bool ShowDisabledLabel => IsDisabled;
    }
}
