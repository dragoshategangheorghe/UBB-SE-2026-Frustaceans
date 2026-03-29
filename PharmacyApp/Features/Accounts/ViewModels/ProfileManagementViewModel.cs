using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyApp.Features.Accounts.ViewModels
{
    using PharmacyApp.Features.Accounts.Logic;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    public class ProfileManagementViewModel : INotifyPropertyChanged
    {
        private UserAccountService _userAccountService;

        private string username;
        private string phoneNumber;
        private string errorMessage;

        public ProfileManagementViewModel(UserAccountService userAccountService)
        {
            _userAccountService = userAccountService;
            LoadUserData();
        }

        public string Email => _userAccountService.CurrentUser?.Email ?? "";

        public string Username
        {
            get => username;
            set
            {
                username = value;
                OnPropertyChanged();
            }
        }

        public string PhoneNumber
        {
            get => phoneNumber;
            set
            {
                phoneNumber = value;
                OnPropertyChanged();
            }
        }

        public string ErrorMessage
        {
            get => errorMessage;
            set
            {
                errorMessage = value;
                OnPropertyChanged();
            }
        }

        public void LoadUserData()
        {
            var user = _userAccountService.CurrentUser;
            if (user == null) return;

            Username = user.Username;
            PhoneNumber = user.PhoneNumber;
        }

        public void SaveChanges()
        {
            _userAccountService.UpdateProfile(Username, PhoneNumber);
        }

        public void CancelChanges()
        {
            LoadUserData();
            ErrorMessage = null;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
