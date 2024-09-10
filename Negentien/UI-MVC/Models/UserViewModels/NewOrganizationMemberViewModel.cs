using System.ComponentModel.DataAnnotations;

namespace NT.UI.MVC.Models.UserViewModels;

public class NewOrganizationMemberViewModel : IValidatableObject
{
    [Required, MinLength(3, ErrorMessage = "Your first name needs to be at least 3 characters long")]
    public string FirstName { get; set; }
    [Required, MinLength(3, ErrorMessage = "Your last name needs to be at least 3 characters long")]
    public string LastName { get; set; }
    [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", 
        ErrorMessage = "Please enter a email address using this format: example@example.com")]
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public DateOnly Birthday { get; set; }
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        List<ValidationResult> validationResults = new List<ValidationResult>();
        
        DateOnly minimumAllowedBirthday = DateOnly.FromDateTime(DateTime.Today.AddYears(-16));

        if (Birthday > minimumAllowedBirthday)
        {
            validationResults.Add(new ValidationResult(
                "The person must be at least 16 years old.",
                new[] { nameof(Birthday) }));
        }

        return validationResults;
    }
}