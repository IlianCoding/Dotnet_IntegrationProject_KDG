using System.ComponentModel.DataAnnotations;
using NT.BL.Domain.projectpck;
using NT.BL.Domain.users;

namespace NT.BL.Domain.platformpck;

public class Platform : IValidatableObject
{
    [Key] public long Id { get; set; }
    public bool IsHead { get; set; }

    [Required]
    [MinLength(3, ErrorMessage = "Name has to be more than 3 letters")]
    public string PlatformName { get; set; }

    public DateOnly CreationDate { get; set; }
    public ICollection<OrganizationUser> OrganizationMaintainer { get; set; }
    public ICollection<Platform> SharingPlatforms { get; set; }
    public ICollection<Project> ProjectsAssigned { get; set; }
    public string LogoObjectName { get; set; }


    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (CreationDate < new DateOnly(2024, 4, 23))
        {
            yield return new ValidationResult("Date has to be before 23/04/2024.", new[] { nameof(CreationDate) });
        }
    }
}