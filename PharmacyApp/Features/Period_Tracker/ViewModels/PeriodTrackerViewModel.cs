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
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyApp.Features.Period_Tracker.ViewModels
{
    public static class PeriodTrackerUser
    {
        public static IUsersRepository UsersRepository { get; set; } =
            new SQLUsersRepository(); // initialize repo at compile-time

        //public static User? CurrentUser => ServiceWrapper.UserAccountService.CurrentUser; 
        private static User? _currentUser = null;
        public static User? CurrentUser
        {
            get
            {
                if (_currentUser == null) _currentUser = UsersRepository.GetUserById(1);
                
                return _currentUser;
            }
            set
            {
                _currentUser = value;
            }
        }

        public static DateTimeOffset ToDateTimeOffset(DateOnly dateOnly, TimeZoneInfo zone)
        {
            var dateTime = dateOnly.ToDateTime(new TimeOnly(0)); // get a datetime from DateOnly
            return new DateTimeOffset(dateTime,
                zone.GetUtcOffset(dateTime)); // get a DateTimeOffset from a DateTime and a time zone
        }

        //TODO only for testing:
        public static DateTimeOffset StartPeriodDate => CurrentUser != null ?
            ToDateTimeOffset(CurrentUser.StartPeriodDate, TimeZoneInfo.Local) : new DateTimeOffset(); // acts like a get { return ... } 

        public static int CycleDays => CurrentUser?.CycleDays ?? 28; // if current user is null, get 28, otherwise get their cycle days
        public static int PeriodLasts => CurrentUser?.PeriodLasts ?? 5;
        public static int PMSOption => CurrentUser?.PMSOption ?? 0;
        public static bool HasPeriodTracker => CurrentUser != null ? UsersRepository.UserHasPeriodTracker(CurrentUser.Id) : false;


        public static void UpdatePeriodTracker(DateTimeOffset startPeriodDate, double cycleDays, double periodLasts, int pmsOption)
        {
           // update the user in RAM and also in the SQL
           // (UpdateUser(user) creates a period tracker for the user too)
           CurrentUser.SetPeriodTracker(DateOnly.FromDateTime(startPeriodDate.DateTime),(int)cycleDays, (int)periodLasts, pmsOption);
           UsersRepository.UpdateUser(CurrentUser); // updates the database
        }
    }

    public class CalendarsModel: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private Random rng;
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this,
                new PropertyChangedEventArgs(propertyName)); // the method is not called if PropertyChanged is null
        }

        public CalendarsModel()
        {
            rng = new Random();
        }

        private DateTime _startPeriodDate;
        public DateTime StartPeriodDate
        {
            get { return _startPeriodDate; }
            set
            {
                _startPeriodDate = value;
                CurrentDate = DateTime.Today; // update this one too when you first Calculate your period (current month)
                OnPropertyChanged();
            }
        }

        private DateTime _currentDate;
        public DateTime CurrentDate
        {
            get { return _currentDate; }
            set
            {
                _currentDate = value;

                OnPropertyChanged();
            }
        }

        public DateTime CurrentBeginningPeriodDate { get; set; }
        public DateTime CurrentEndPeriodDate { get; set; }
        public DateTime CurrentBeginningLowFertilityDate { get; set; }
        public DateTime CurrentEndLowFertilityDate { get; set; }
        public DateTime CurrentBeginningOvulationDate { get; set; }
        public DateTime CurrentEndOvulationDate { get; set; }
        public DateTime CurrentBeginningPMSDate { get; set; }

        public DateTime CurrentEndPMSDate { get; set; }


        internal void CalculatePeriodTracker(DateTime startPeriodDate)
        {
            StartPeriodDate = startPeriodDate.Date;
            CurrentBeginningPeriodDate = StartPeriodDate; //begin from the start at first, then update based on arrows
            CurrentDate = DateTime.Today; 

            UpdatePeriodTracker(StartPeriodDate <= CurrentDate); //if the start is lower than the current, then we go right to update
        }

        internal void UpdatePeriodTracker(bool goRight)
        {
            // now I realised I can just add cycle days to the current Date until I reach the desired month/year based on the direction
            while (CurrentBeginningPeriodDate.Month != CurrentDate.Month &&
                   CurrentBeginningPeriodDate.Year != CurrentDate.Year)
                CurrentBeginningPeriodDate = CurrentBeginningPeriodDate.AddDays(goRight ? (int)PeriodTrackerUser.CycleDays : -(int)PeriodTrackerUser.CycleDays);

            CurrentMonth = CurrentBeginningPeriodDate.ToString("MMMM");

            // now calcualte the others based on that
            CurrentEndPeriodDate = new DateTime(CurrentBeginningPeriodDate.Year, CurrentBeginningPeriodDate.Month,CurrentBeginningPeriodDate.Day).AddDays(PeriodTrackerUser.PeriodLasts);

            PeriodInterval = "Period Days: " +
                $"{CurrentBeginningPeriodDate.Day} {CurrentBeginningPeriodDate.ToString("MMMM")} {CurrentBeginningPeriodDate.Year} - " +
                $"{CurrentEndPeriodDate.Day} {CurrentEndPeriodDate.ToString("MMMM")} {CurrentEndPeriodDate.Year}";

            if (PeriodTrackerUser.PeriodLasts < 9)
            {
                CurrentBeginningLowFertilityDate = new DateTime(CurrentEndPeriodDate.Year,
                    CurrentEndPeriodDate.Month,
                    CurrentEndPeriodDate.Day).AddDays(1);
                CurrentEndLowFertilityDate = new DateTime(CurrentBeginningPeriodDate.Year,
                    CurrentBeginningPeriodDate.Month,
                    CurrentBeginningPeriodDate.Day).AddDays(8);

                LowFertilityInterval = "Low Fertility Days: " +
                    $"{CurrentBeginningLowFertilityDate.Day} {CurrentBeginningLowFertilityDate.ToString("MMMM")} {CurrentBeginningLowFertilityDate.Year} - " +
                    $"{CurrentEndLowFertilityDate.Day} {CurrentEndLowFertilityDate.ToString("MMMM")} {CurrentEndLowFertilityDate.Year}";
            }
            else
            {
                LowFertilityInterval = "Low Fertility Days: No such days";
            }


            CurrentBeginningOvulationDate = new DateTime(CurrentBeginningPeriodDate.Year, CurrentBeginningPeriodDate.Month,
                CurrentBeginningPeriodDate.Day).AddDays(11);
            CurrentEndOvulationDate = new DateTime(CurrentEndPeriodDate.Year, CurrentEndPeriodDate.Month,
                CurrentEndPeriodDate.Day).AddDays(15);

            OvulationInterval = "Ovulation Days: " +
                $"{CurrentBeginningOvulationDate.Day} {CurrentBeginningOvulationDate.ToString("MMMM")} {CurrentBeginningOvulationDate.Year} - " +
                $"{CurrentEndOvulationDate.Day} {CurrentEndOvulationDate.ToString("MMMM")} {CurrentEndOvulationDate.Year}";

            if (PeriodTrackerUser.PMSOption != 0) // PMS exists
            {
                // PMS before the NEXT period

                CurrentBeginningPMSDate = new DateTime(CurrentBeginningPeriodDate.Year, CurrentBeginningPeriodDate.Month,
                    CurrentBeginningPeriodDate.Day).AddDays(27);
                // now subtract days based on the option selected
                if (PeriodTrackerUser.PMSOption == 1)
                    CurrentBeginningPMSDate =  CurrentBeginningPMSDate.AddDays(-rng.Next(1, 4)); // 1-3
                else if (PeriodTrackerUser.PMSOption == 2)
                    CurrentBeginningPMSDate =  CurrentBeginningPMSDate.AddDays(-rng.Next(4, 8)); // 4-7
                else
                    CurrentBeginningPMSDate = CurrentBeginningPMSDate.AddDays(-rng.Next(7, 14)); // 7-14

                CurrentEndPMSDate = new DateTime(CurrentBeginningPeriodDate.Year, CurrentBeginningPeriodDate.Month,
                    CurrentBeginningPeriodDate.Day).AddDays(27); // stops right before the next period

                PmsInterval = "PMS Days: " +
                    $"{CurrentBeginningPMSDate.Day} {CurrentBeginningPMSDate.ToString("MMMM")} {CurrentBeginningPMSDate.Year} - " +
                    $"{CurrentEndPMSDate.Day} {CurrentEndPMSDate.ToString("MMMM")} {CurrentEndPMSDate.Year}";
            }
            else
            {
                PmsInterval = "PMS Days: No such days";
            }
        }

        //NOW the actual data that changes and is the viewmodel

        private string _currentMonth;

        public string CurrentMonth
        {
            get { return _currentMonth;  }
            set
            {
                _currentMonth = value;
                OnPropertyChanged();
            }
        }

        private string _periodInterval;
        public string PeriodInterval
        {
            get { return _periodInterval; }
            set
            {
                _periodInterval = value;
                OnPropertyChanged();
            }
        }

        private string _lowFertilityInterval;
        public string LowFertilityInterval
        {
            get { return _lowFertilityInterval; }
            set
            {
                _lowFertilityInterval = value;
                OnPropertyChanged();
            }
        }

        private string _ovulationInterval;
        public string OvulationInterval
        {
            get { return _ovulationInterval; }
            set
            {
                _ovulationInterval = value;
                OnPropertyChanged();
            }
        }

        private string _pmsInterval;
        public string PmsInterval
        {
            get { return _pmsInterval; }
            set
            {
                _pmsInterval = value;
                OnPropertyChanged();
            }
        }
    }

    public class PeriodTrackerViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            // call the method for this event (event handler) stored in the PropertyChanged delegate using this property
            // to notify the View that the ViewModel has updated (call down, notify up)
            PropertyChanged?.Invoke(this,
                new PropertyChangedEventArgs(propertyName)); // the method is not called if PropertyChanged is null
        }


        //DATA about the Calendars
        public CalendarsModel Calendars { get; set; }

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


        // constructor + methods
        public PeriodTrackerViewModel()
        {
            //PeriodTrackerUser = new PeriodTrackerUserModel();
            Calendars = new CalendarsModel();
            ShowCalendars();
        }

        private void ShowCalendars()
        {
            if (PeriodTrackerUser.HasPeriodTracker)
                CalculatePeriodTracker(PeriodTrackerUser.StartPeriodDate, PeriodTrackerUser.CycleDays,
                    PeriodTrackerUser.PeriodLasts, PeriodTrackerUser.PMSOption);

            CalendarsVisibility = PeriodTrackerUser.HasPeriodTracker ? "Visible" : "Collapsed";
        }

        internal void CalculatePeriodTracker(DateTimeOffset startPeriodDate, double cycleDays, double periodLasts,
            int pmsOption)
        {
            // After clicking, first of all update the user's period tracker
            PeriodTrackerUser.UpdatePeriodTracker(startPeriodDate, cycleDays, periodLasts, pmsOption);

            // After that, update the calendars' properties, which will automatically notify the UI
            Calendars.CalculatePeriodTracker(startPeriodDate.Date);

        }

    }
}