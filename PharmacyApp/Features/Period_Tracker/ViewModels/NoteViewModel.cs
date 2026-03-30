using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyApp.Features.Period_Tracker.ViewModels
{
    public class NoteViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this,
                new PropertyChangedEventArgs(propertyName));
            UpdateNote();
        }

        public int NoteId {get; set;}

        private string _noteBody;
        public string NoteBody
        {
            get { return _noteBody; }
            set
            {
                _noteBody = value;
                OnPropertyChanged();
            }
        }

        private bool _noteIsDone;
        public bool NoteIsDone
        {
            get { return _noteIsDone; }
            set
            {
                _noteIsDone = value;
                OnPropertyChanged();
            }
        }

        public NoteViewModel(int noteId, string noteBody, bool noteIsDone)
        {
            NoteId = noteId;
            NoteBody = noteBody;
            NoteIsDone = noteIsDone;
        }

        public void UpdateNote()
        {
            PeriodTrackerUser.CurrentUser.PeriodNotes[NoteId] = new Tuple<string, bool>(NoteBody, NoteIsDone);
            PeriodTrackerUser.UpdateUser();
        }


    }
}
