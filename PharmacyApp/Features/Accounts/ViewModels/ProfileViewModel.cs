using PharmacyApp.Common.Commands;
using PharmacyApp.Features.Accounts.Logic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PharmacyApp.Features.Accounts.ViewModels
{
    public class ProfileViewModel : INotifyPropertyChanged
    {
        private UserAccountService _userAccountService;

        private string username;
        private string phoneNumber;

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
        private string errorMessage;
        public string ErrorMessage
        {
            get => errorMessage; 
            set { 
                errorMessage = value;
                OnPropertyChanged();
            }
        }


        public ICommand SaveCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        public ICommand ChangePasswordCommand { get; set; }

        public ProfileViewModel(UserAccountService userAccountService)
        {
            _userAccountService = userAccountService;

            LoadUserData();

            SaveCommand = new RelayCommand(SaveChanges);
            CancelCommand = new RelayCommand(CancelChanges);
            ChangePasswordCommand = null;
        }

        private void LoadUserData()
        {
            var user = _userAccountService.CurrentUser;
            if (user == null) return;

            Username = user.Username;
            PhoneNumber = user.PhoneNumber;
        }

        public void SaveChanges()
        {

            _userAccountService.UpdateProfile(Username, PhoneNumber);
            System.Diagnostics.Debug.WriteLine("Profile updated successfully");
            //throws error if user input is incorrect => handled in .xaml.cs to be able to show message

        }

        public void CancelChanges()
        {
            LoadUserData();
            System.Diagnostics.Debug.WriteLine("Changes canceled");
        }

        



        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
