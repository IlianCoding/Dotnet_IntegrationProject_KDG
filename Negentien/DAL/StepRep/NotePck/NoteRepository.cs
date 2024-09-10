using Microsoft.EntityFrameworkCore;
using NT.BL.Domain.sessionpck;
using NT.DAL.EF;
using SQLitePCL;

namespace NT.DAL.StepRep.NotePck;

public class NoteRepository : INoteRepository
{
    private readonly PhygitalDbContext _phygitalDbContext;

    public NoteRepository(PhygitalDbContext phygitalDbContext)
    {
        _phygitalDbContext = phygitalDbContext ?? throw new ArgumentNullException(nameof(phygitalDbContext));
    }

    public void CreateNote(Note note)
    {
        _phygitalDbContext.Notes.Add(note);
        _phygitalDbContext.SaveChanges();
    }

    public Note ReadNoteById(long noteId)
    {
        return _phygitalDbContext.Notes
            .SingleOrDefault(note => note.Id == noteId);
    }

    public Note ReadNoteByIdWithStep(long noteId)
    {
        return _phygitalDbContext.Notes
            .Include(note => note.Step)
            .SingleOrDefault(note => note.Id == noteId);
    }

    public void UpdateNote(Note note)
    {
        _phygitalDbContext.Notes.Update(note);
        _phygitalDbContext.SaveChanges();
    }

    public void DeleteNote(Note note)
    {
        _phygitalDbContext.Notes.Remove(note);
        _phygitalDbContext.SaveChanges();
    }

    public IEnumerable<Note> ReadNotesWithStep()
    {
        return _phygitalDbContext.Notes
            .Include(note => note.Step);
    }

    public void DeleteNotes(IEnumerable<Note> notes)
    {
        _phygitalDbContext.Notes.RemoveRange(notes);
        _phygitalDbContext.SaveChanges();
    }

    public IEnumerable<Note> ReadAllNotesWithStep()
    {
        return _phygitalDbContext.Notes
            .Include(n => n.Step);
    }
}