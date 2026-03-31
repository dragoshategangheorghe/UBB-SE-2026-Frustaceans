using PharmacyApp.Common.Repositories;
using PharmacyApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyApp.Features.Period_Tracker.ViewModels
{
    public class CalendarsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private Random rng;
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this,
                new PropertyChangedEventArgs(propertyName)); // the method is not called if PropertyChanged is null
        }

        public CalendarsViewModel()
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
            CurrentBeginningPeriodDate = StartPeriodDate.AddDays(-PeriodTrackerUser.CycleDays); //begin from the start at first, then update based on arrows
            CurrentDate = DateTime.Today;

            UpdatePeriodTracker(true); //if the start is lower than the current, then we go right to update
        }

        internal void UpdatePeriodTracker(bool goRight)
        {
            // now I realised I can just add cycle days to the current Date until I reach the desired month/year based on the direction
            // NOTE for the above comment: not really.. so i give up, dates are harder than olympiad problems
            // (EndDate - StartDate).TotalDays - I could use this

            LiterallyTodayString = DateTime.Today.ToString("d");
            // go in that direction
            CurrentBeginningPeriodDate = CurrentBeginningPeriodDate.AddDays(goRight ? (int)PeriodTrackerUser.CycleDays : -(int)PeriodTrackerUser.CycleDays);

            CurrentMonth = CurrentBeginningPeriodDate.ToString("" +
                                                               "MMMM", CultureInfo.InvariantCulture);

            // now calcualte the others based on that
            CurrentEndPeriodDate = new DateTime(CurrentBeginningPeriodDate.Year, CurrentBeginningPeriodDate.Month, CurrentBeginningPeriodDate.Day).AddDays(PeriodTrackerUser.PeriodLasts);

            PeriodInterval = "Period Days: " +
                $"{CurrentBeginningPeriodDate.Day} {CurrentBeginningPeriodDate.ToString("MMMM", CultureInfo.InvariantCulture)} {CurrentBeginningPeriodDate.Year} - " +
                $"{CurrentEndPeriodDate.Day} {CurrentEndPeriodDate.ToString("MMMM", CultureInfo.InvariantCulture)} {CurrentEndPeriodDate.Year}";

            NextPeriodDateString = CurrentBeginningPeriodDate.AddDays(PeriodTrackerUser.CycleDays).ToString("d");
            NextPeriodDistanceString = "In " +
                $"{double.Ceiling((CurrentBeginningPeriodDate.AddDays(PeriodTrackerUser.CycleDays) - CurrentBeginningPeriodDate).TotalDays)} days";

            if (PeriodTrackerUser.PeriodLasts < 9)
            {
                CurrentBeginningLowFertilityDate = new DateTime(CurrentEndPeriodDate.Year,
                    CurrentEndPeriodDate.Month,
                    CurrentEndPeriodDate.Day).AddDays(1);
                CurrentEndLowFertilityDate = new DateTime(CurrentBeginningPeriodDate.Year,
                    CurrentBeginningPeriodDate.Month,
                    CurrentBeginningPeriodDate.Day).AddDays(8);

                LowFertilityInterval = "Low Fertility Days: " +
                    $"{CurrentBeginningLowFertilityDate.Day} {CurrentBeginningLowFertilityDate.ToString("MMMM", CultureInfo.InvariantCulture)} {CurrentBeginningLowFertilityDate.Year} - " +
                    $"{CurrentEndLowFertilityDate.Day} {CurrentEndLowFertilityDate.ToString("MMMM", CultureInfo.InvariantCulture)} {CurrentEndLowFertilityDate.Year}";
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
                $"{CurrentBeginningOvulationDate.Day} {CurrentBeginningOvulationDate.ToString("MMMM", CultureInfo.InvariantCulture)} {CurrentBeginningOvulationDate.Year} - " +
                $"{CurrentEndOvulationDate.Day} {CurrentEndOvulationDate.ToString("MMMM", CultureInfo.InvariantCulture)} {CurrentEndOvulationDate.Year}";



            // calculate string based on if the user is past or post ovulation
            CurrentOvulationDateString = CurrentBeginningOvulationDate.ToString("d");
            if (DateTime.Today >= CurrentBeginningOvulationDate && DateTime.Today <= CurrentEndOvulationDate)
                PastOvulationString = "Now";
            if (DateTime.Today > CurrentEndOvulationDate)
                PastOvulationString = "Passed";
            if (DateTime.Today < CurrentBeginningOvulationDate)
                PastOvulationString = "Passed";



            if (PeriodTrackerUser.PMSOption != 0) // PMS exists
            {
                // PMS before the NEXT period

                CurrentBeginningPMSDate = new DateTime(CurrentBeginningPeriodDate.Year, CurrentBeginningPeriodDate.Month,
                    CurrentBeginningPeriodDate.Day).AddDays(PeriodTrackerUser.CycleDays - 1);
                // now subtract days based on the option selected
                if (PeriodTrackerUser.PMSOption == 1)
                    CurrentBeginningPMSDate = CurrentBeginningPMSDate.AddDays(-rng.Next(1, 4)); // 1-3
                else if (PeriodTrackerUser.PMSOption == 2)
                    CurrentBeginningPMSDate = CurrentBeginningPMSDate.AddDays(-rng.Next(4, 8)); // 4-7
                else
                    CurrentBeginningPMSDate = CurrentBeginningPMSDate.AddDays(-rng.Next(7, 14)); // 7-14

                CurrentEndPMSDate = new DateTime(CurrentBeginningPeriodDate.Year, CurrentBeginningPeriodDate.Month,
                    CurrentBeginningPeriodDate.Day).AddDays(PeriodTrackerUser.CycleDays); // stops right before the next period

                PmsInterval = "PMS Days: " +
                    $"{CurrentBeginningPMSDate.Day} {CurrentBeginningPMSDate.ToString("MMMM", CultureInfo.InvariantCulture)} {CurrentBeginningPMSDate.Year} - " +
                    $"{CurrentEndPMSDate.Day} {CurrentEndPMSDate.ToString("MMMM", CultureInfo.InvariantCulture)} {CurrentEndPMSDate.Year}";
            }
            else
            {
                PmsInterval = "PMS Days: No such days";
            }

            ConstructCurrentPhaseString();
        }

        private void ConstructCurrentPhaseString()
        {
            if (DateTime.Today >= CurrentBeginningPeriodDate && DateTime.Today <= CurrentEndPeriodDate)
            {
                CurrentPhaseString = "Menstrual Phase";
                GiveUserWellnessDiscounts();
            }
            else if (DateTime.Today > CurrentEndPeriodDate && DateTime.Today < CurrentBeginningOvulationDate)
                CurrentPhaseString = "Follicular Phase";
            else if (DateTime.Today >= CurrentBeginningOvulationDate && DateTime.Today <= CurrentEndOvulationDate)
                CurrentPhaseString = "Ovulation Phase";
            else if (DateTime.Today > CurrentEndOvulationDate && DateTime.Today < CurrentBeginningPeriodDate.AddDays(PeriodTrackerUser.CycleDays))
                CurrentPhaseString = "Luteal Phase"; // before next period
            else
            {
                CurrentPhaseString = "Not calculated for this month";
            }
        }

        private void GiveUserWellnessDiscounts()
        {
            IItemsRepository itemsRepository = new SQLItemsRepository();
            List<Item> items = itemsRepository.GetAllItems();
            List<String> categories = new List<string>();
            categories.Add("wellness");
            items = items.Where(item => categories.Contains(item.Category)).ToList();
            foreach (Item item in items)
            {
                PeriodTrackerUser.CurrentUser.UserDiscounts[item.Id]=  20.0f; // give for each item a 20% discount
            }
            PeriodTrackerUser.UpdateUser();
        }

        //NOW the actual data that changes and is the viewmodel

        private string _currentMonth;

        public string CurrentMonth
        {
            get { return _currentMonth; }
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


        private string _pastOvulationString;
        public string PastOvulationString
        {
            get { return _pastOvulationString; }
            set
            {
                _pastOvulationString = value;
                OnPropertyChanged();
            }
        }

        private string _nextPeriodDateString;
        public string NextPeriodDateString
        {
            get { return _nextPeriodDateString; }
            set
            {
                _nextPeriodDateString = value;
                OnPropertyChanged();
            }
        }

        private string _currentPhaseString;
        public string CurrentPhaseString
        {
            get { return _currentPhaseString; }
            set
            {
                _currentPhaseString = value;
                OnPropertyChanged();
            }
        }
        private string _literallyTodayString;
        public string LiterallyTodayString
        {
            get
            {
                return _literallyTodayString;
            }
            set
            {
                _literallyTodayString = value;
                OnPropertyChanged();
            }
        }

        private string _nextPeriodDistanceString;

        public string NextPeriodDistanceString
        {
            get
            {
                return _nextPeriodDistanceString;
            }
            set
            {
                _nextPeriodDistanceString = value;
                OnPropertyChanged();
            }
        }

        private string _currentOvulationDateString;
        public string CurrentOvulationDateString
        {
            get
            {
                return _currentOvulationDateString;
            }
            set
            {
                _currentOvulationDateString = value;
                OnPropertyChanged();
            }
        }

    }
}
