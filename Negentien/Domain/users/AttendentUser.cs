using System.ComponentModel.DataAnnotations;
using NT.BL.Domain.projectpck;

namespace NT.BL.Domain.users;

public class AttendentUser : GeneralUser
{
    [Required]
    [MinLength(3, ErrorMessage = "Name has to be more than 3 letters")]
    public string Organization { get; set; }
    public Project AssignedProject { get; set; }
}