using PharmacyApp.Common.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Notifications;
using PharmacyApp.Models;

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

        public static void UpdateUser()
        {
            UsersRepository.UpdateUser(CurrentUser); // updates the database
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

        public static int MaxNoteId
        {
            get
            {
                if (CurrentUser == null) return -1;
                if (CurrentUser.PeriodNotes.Count == 0) return 0; // create their first note
                int maxNoteId = -1;
                foreach (int key in CurrentUser.PeriodNotes.Keys)
                {
                    if (key > maxNoteId)
                        maxNoteId = key;
                }

                return maxNoteId;
            }
        }
        public static void UpdatePeriodTracker(DateTimeOffset startPeriodDate, double cycleDays, double periodLasts, int pmsOption)
        {
            // update the user in RAM and also in the SQL
            // (UpdateUser(user) creates a period tracker for the user too)
            CurrentUser.SetPeriodTracker(DateOnly.FromDateTime(startPeriodDate.DateTime), (int)cycleDays, (int)periodLasts, pmsOption);
            UpdateUser();
        }
    }
}
