using NT.BL.Domain.users;

namespace NT.UI.MVC.Models.UserViewModels;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

public class NewUserViewModel : IValidatableObject
{
    [Required, MinLength(3, ErrorMessage = "Your first name needs to be at least 3 characters long")]
    public string FirstName { get; set; }
    
    [Required, MinLength(3, ErrorMessage = "Your last name needs to be at least 3 characters long")]
    public string LastName { get; set; }
    
    [Required]
    [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", 
        ErrorMessage = "Please enter an email address using this format: example@example.com")]
    public string Email { get; set; }
    
    public string PhoneNumber { get; set; }
    
    [Required]
    public DateOnly Birthday { get; set; }
    
    public bool MoreInfo { get; set; }
    
    [Required]
    public string Password { get; set; }
    public int Color { get; set; }
    
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

        if (string.IsNullOrEmpty(Password) || Password.Length < 6)
        {
            validationResults.Add(new ValidationResult(
                "The password needs to be at least 6 characters long.",
                new[] { nameof(Password) }));
        }
        else
        {
            if (!Regex.IsMatch(Password, @"[A-Z]"))
            {
                validationResults.Add(new ValidationResult(
                    "The password must contain at least one uppercase letter.",
                    new[] { nameof(Password) }));
            }
        }

        return validationResults;
    }
}