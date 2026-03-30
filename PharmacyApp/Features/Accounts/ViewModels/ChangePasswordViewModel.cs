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
    public class ChangePasswordViewModel : INotifyPropertyChanged
    {
        private readonly UserAccountService _userAccountService;

        private string oldPassword;
        private string newPassword;
        private string confirmPassword;
        private string errorMessage;

        public ICommand ChangePasswordCommand;
        public ChangePasswordViewModel(UserAccountService service)
        {
            _userAccountService = service;
            ChangePasswordCommand=new RelayCommand(ChangePassword);
        }

        public string OldPassword
        {
            get => oldPassword;
            set
            {
                oldPassword = value;
                OnPropertyChanged();
            }
        }

        public string NewPassword
        {
            get => newPassword;
            set
            {
                newPassword = value;
                OnPropertyChanged();
            }
        }

        public string ConfirmPassword
        {
            get => confirmPassword;
            set
            {
                confirmPassword = value;
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

        
        public void ChangePassword()
        {
            _userAccountService.ChangePassword(
                OldPassword,
                NewPassword,
                ConfirmPassword
            );
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
