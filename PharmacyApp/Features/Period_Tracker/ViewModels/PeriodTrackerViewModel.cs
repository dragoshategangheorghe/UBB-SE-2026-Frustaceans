using Microsoft.UI;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PharmacyApp.Common.Repositories;

using PharmacyApp.Features.Accounts.Logic;
using PharmacyApp.Models;

namespace PharmacyApp.Features.Period_Tracker.ViewModels
{
    public class PeriodTrackerUserModel
    {
        public static IUsersRepository UsersRepository { get; set; } = new SQLUsersRepository(); // initialize repo at compile-time
        public static User CurrentUser =>  UsersRepository.GetUserById(1); //TODO FOR TESTING //ServiceWrapper.UserAccountService.CurrentUser;

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

        public PeriodTrackerUserModel()
        {
        }
    }

    public class PeriodTrackerViewModel
    {
        public PeriodTrackerUserModel PeriodTrackerUser { get; set; }

        public PeriodTrackerViewModel()
        {
            PeriodTrackerUser = new PeriodTrackerUserModel();
        }
    }
}