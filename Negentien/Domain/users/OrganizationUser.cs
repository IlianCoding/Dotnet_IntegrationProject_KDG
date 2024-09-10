using System.ComponentModel.DataAnnotations;
using NT.BL.Domain.projectpck;

namespace NT.BL.Domain.users;

public class OrganizationUser : GeneralUser
{
    [Required]
    [MinLength(3, ErrorMessage = "Name has to be more than 3 letters")]
    public string Organization { get; set; }
    
    /* This forces the user upon first login to change their password */
    public bool RequireConfirmedPassword { get; set; } = true;
    public ICollection<Project> OwnedProjects { get; set; }
}