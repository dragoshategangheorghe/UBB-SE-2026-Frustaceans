using PharmacyApp.Common.Commands;
using PharmacyApp.Features.Accounts.Logic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;


namespace PharmacyApp.Features.Accounts.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private UserAccountService _userAccountService;
        public event Action LoginSucceded;
        private string email;
        private string password;
        private string errorMessage;

        public string Email
        {
            get => email;
            set
            {
                email = value;
                OnPropertyChanged();
            }
        }

        public string Password
        {
            get => password;
            set
            {
                password = value;
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

        public ICommand LoginCommand { get; set; }

        public LoginViewModel(UserAccountService userAccountService)
        {
            _userAccountService = userAccountService;

            LoginCommand = (ICommand)new RelayCommand(Login);
        }

        public void Login()
        {

            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "Fields cannot be empty";
                return;
            }

            System.Diagnostics.Debug.WriteLine($"Logging in with {Email}");

            try
            {

                _userAccountService.Login(Email, Password);
                
                LoginSucceded?.Invoke();
            }
            catch (Exception ex) {
                ErrorMessage = ex.Message;
            }
            
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
