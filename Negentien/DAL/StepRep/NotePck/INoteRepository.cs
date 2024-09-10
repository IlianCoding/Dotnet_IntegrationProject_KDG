using NT.BL.Domain.sessionpck;

namespace NT.DAL.StepRep.NotePck;

public interface INoteRepository
{
    void CreateNote(Note note);
    Note ReadNoteById(long noteId);
    Note ReadNoteByIdWithStep(long noteId);
    void UpdateNote(Note note);
    void DeleteNote(Note note);
    IEnumerable<Note> ReadNotesWithStep();
    void DeleteNotes(IEnumerable<Note> notes);
    IEnumerable<Note> ReadAllNotesWithStep();
}