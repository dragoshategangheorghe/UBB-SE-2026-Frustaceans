
namespace PharmacyApp.Models
{
    public class PeriodNote
    {

        public int Id { get; private set; }
        public string NoteBody { get; set; }
        public bool IsDone { get; set; }


        public PeriodNote(int id, string noteBody, bool isDone)
        {
            Id = id;
            NoteBody = noteBody;
            IsDone = isDone;
        }
    }
}
