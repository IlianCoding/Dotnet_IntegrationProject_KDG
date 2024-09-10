using System.ComponentModel.DataAnnotations;
using NT.BL.Domain.flowpck;

namespace NT.BL.Domain.sessionpck;

public class Note
{
    [Key] public long Id { get; set; }
    [Required]
    [MinLength(2)]
    public string NoteTitle { get; set; }
    [Required]
    [MinLength(2)]
    public string NoteText { get; set; }
    public string CreatedAttendantName { get; set; }
    //TODO: add RunningFlow moet aanpassen om te zien is kios of niet
    public bool IsKiosk { get; set; }
    public Step Step { get; set; }
}