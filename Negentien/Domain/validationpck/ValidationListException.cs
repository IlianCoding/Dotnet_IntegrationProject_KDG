using System.ComponentModel.DataAnnotations;

namespace NT.BL.Domain.validationpck;

public class ValidationListException : Exception
{
    public List<ValidationResult> ValidationResults { get; set; }
    
    public ValidationListException(List<ValidationResult> validationResults)
    {
        ValidationResults = validationResults;
    }
}