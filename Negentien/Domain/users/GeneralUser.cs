using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace NT.BL.Domain.users;

public class GeneralUser : IdentityUser, IValidatableObject
{
    [Required]
    [MinLength(3, ErrorMessage = "First name has to be more than 3 letters")]
    public string FirstName { get; set; }
    [Required]
    [MinLength(3, ErrorMessage = "Last name has to be more than 3 letters")]
    public string LastName { get; set; }
    public DateOnly BirthDate { get; set; }
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        DateOnly today = DateOnly.FromDateTime(DateTime.Today);
        int age = today.Year - BirthDate.Year;
        
        if (today < BirthDate.AddYears(age))
        {
            age--;
        }

        if (age < 16)
        {
            yield return new ValidationResult("Person must be at least 16 years old.", new[] { nameof(BirthDate) });
        }
    }
}