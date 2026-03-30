using Microsoft.UI;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using PharmacyApp.Common.Repositories;
using PharmacyApp.Features.Accounts.Logic;
using PharmacyApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyApp.Features.Period_Tracker.ViewModels
{


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
        public CalendarsViewModel Calendars { get; set; }
        public ObservableCollection<NoteViewModel> Notes { get; }

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
            Calendars = new CalendarsViewModel();
            Notes = new ObservableCollection<NoteViewModel>();
            CreateNotes();
            ShowCalendars();
        }

        private void CreateNotes()
        {
            foreach (KeyValuePair<int, Tuple<string, bool>> periodNote in PeriodTrackerUser.CurrentUser.PeriodNotes)
            {
                NoteViewModel periodNoteVM = new NoteViewModel(periodNote.Key, periodNote.Value.Item1, periodNote.Value.Item2);
                Notes.Add(periodNoteVM);
            }
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

        internal void UpdatePeriodTracker(bool goRight)
        {
            Calendars.CurrentDate = Calendars.CurrentDate.AddMonths(goRight ? 1 : -1); // go in this month's direction
            Calendars.UpdatePeriodTracker(goRight);
        }

    }
}