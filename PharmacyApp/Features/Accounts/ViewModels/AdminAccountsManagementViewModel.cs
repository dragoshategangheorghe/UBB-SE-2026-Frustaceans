using PharmacyApp.Common.Services;
using PharmacyApp.Features.Accounts.Logic;
using PharmacyApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyApp.Features.Accounts.ViewModels
{
    public class AdminAccountsManagementViewModel : INotifyPropertyChanged
    {
        private UserAccountService _userService;

        private string searchQuery;
        private string errorMessage;

        public ObservableCollection<UserItemViewModel> Users { get; set; }

        public AdminAccountsManagementViewModel(UserAccountService userService)
        {
            _userService = userService;
            Users = new ObservableCollection<UserItemViewModel>();

            LoadUsers();
        }

        public string SearchQuery
        {
            get => searchQuery;
            set
            {
                searchQuery = value;
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

        public void LoadUsers()
        {
            try
            {
                var users = _userService.SearchUsers("");
                UpdateUsers(users);
                ErrorMessage = null;
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }

        public void Search()
        {
            var result = _userService.SearchUsers(SearchQuery ?? "");
            UpdateUsers(result);
        }

        public void Promote(UserItemViewModel userItem)
        {
            _userService.PromoteToAdmin(userItem.User);
            Refresh();
        }

        public void Disable(UserItemViewModel userItem)
        {
            _userService.DisableAccount(userItem.User);
            Refresh();
        }

        private void Refresh()
        {
            Search();
        }

        private void UpdateUsers(List<User> users)
        {
            Users.Clear();
            foreach (var user in users)
                Users.Add(new UserItemViewModel(user));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
