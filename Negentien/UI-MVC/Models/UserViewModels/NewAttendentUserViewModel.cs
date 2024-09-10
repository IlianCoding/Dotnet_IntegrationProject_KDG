using System.ComponentModel.DataAnnotations;

namespace NT.UI.MVC.Models.UserViewModels;

public class NewAttendentUserViewModel : IValidatableObject
{
    [Required]
    [MinLength(3, ErrorMessage = "First name has to be more than 3 letters")]
    public string FirstName { get; set; }
    [Required]
    [MinLength(3, ErrorMessage = "Last name has to be more than 3 letters")]
    public string LastName { get; set; }
    [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", 
        ErrorMessage = "Please enter a email address using this format: example@example.com")]
    public string Email { get; set; }
    [RegularExpression(@"^4\d{8}$", ErrorMessage = "The phone number needs to be at least 9 characters long.")]
    public string PhoneNumber { get; set; }
    public DateOnly BirthDate { get; set; }
    [Required]
    [MinLength(3, ErrorMessage = "Name has to be more than 3 letters")]
    public string AssignedProjectName { get; set; }
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