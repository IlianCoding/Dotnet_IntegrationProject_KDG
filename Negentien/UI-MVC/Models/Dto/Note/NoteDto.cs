namespace NT.UI.MVC.Models.Dto.Note;

public class NoteDto
{
    public long Id { get; set; }
    
    public string NoteTitle { get; set; }
    public string NoteText { get; set; }
    public string CreatedAttendantName { get; set; }
    
    public long StepId { get; set; }
}