using Microsoft.UI;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using PharmacyApp.Common.Repositories;
using PharmacyApp.Features.Accounts.Logic;
using PharmacyApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyApp.Features.Period_Tracker.ViewModels
{
    public class PeriodTrackerUserModel
    {
        public static IUsersRepository UsersRepository { get; set; } =
            new SQLUsersRepository(); // initialize repo at compile-time

        public static User CurrentUser => ServiceWrapper.UserAccountService.CurrentUser; //TODO FOR TESTING //ServiceWrapper.UserAccountService.CurrentUser;

        public static DateTimeOffset ToDateTimeOffset(DateOnly dateOnly, TimeZoneInfo zone)
        {
            var dateTime = dateOnly.ToDateTime(new TimeOnly(0)); // get a datetime from DateOnly
            return new DateTimeOffset(dateTime,
                zone.GetUtcOffset(dateTime)); // get a DateTimeOffset from a DateTime and a time zone
        }

        //TODO only for testing:
        public DateTimeOffset StartPeriodDate =>
            ToDateTimeOffset(CurrentUser.StartPeriodDate, TimeZoneInfo.Local); // acts like a get { return ... } 

        public int CycleDays => CurrentUser.CycleDays;
        public int PeriodLasts => CurrentUser.PeriodLasts;
        public int PMSOption => CurrentUser.PMSOption;

        public bool HasPeriodTracker => UsersRepository.UserHasPeriodTracker(CurrentUser.Id);

        public PeriodTrackerUserModel()
        {
        }

        internal void UpdatePeriodTracker(DateTimeOffset startPeriodDate, double cycleDays, double periodLasts, int pmsOption)
        {
           // update the user in RAM and also in the SQL
           // (UpdateUser(user) creates a period tracker for the user too)
           CurrentUser.SetPeriodTracker(DateOnly.FromDateTime(startPeriodDate.DateTime),(int)cycleDays, (int)periodLasts, pmsOption);
           UsersRepository.UpdateUser(CurrentUser); // updates the database
        }
    }

    public class PeriodTrackerViewModel : INotifyPropertyChanged
    {
        public PeriodTrackerUserModel PeriodTrackerUser { get; set; }



        private string _calendarsVisibility;

        [DefaultValue("Collapsed")] // set its default value to "Collapsed"
        public string CalendarsVisibility
        {
            get { return _calendarsVisibility; }
            set
            {
                _calendarsVisibility = value;
                OnPropertyChanged();
            }
        }





        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            // call the method for this event (event handler) stored in the PropertyChanged delegate using this property
            // to notify the View that the ViewModel has updated (call down, notify up)
            PropertyChanged?.Invoke(this,
                new PropertyChangedEventArgs(propertyName)); // the method is not called if PropertyChanged is null
        }

        public PeriodTrackerViewModel()
        {
            PeriodTrackerUser = new PeriodTrackerUserModel();
            ShowCalendars();
        }

        private void ShowCalendars()
        {
            CalendarsVisibility = PeriodTrackerUser.HasPeriodTracker ? "Visible" : "Collapsed";
        }

        internal void UpdatePeriodTracker(DateTimeOffset startPeriodDate, double cycleDays, double periodLasts,
            int pmsOption)
        {
            // After clicking, first of all update the user's period tracker
            PeriodTrackerUser.UpdatePeriodTracker(startPeriodDate, cycleDays, periodLasts, pmsOption);

            // After that, update the calendars



        }
    }
}