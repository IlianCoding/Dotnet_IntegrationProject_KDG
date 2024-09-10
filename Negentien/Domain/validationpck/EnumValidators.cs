using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace NT.BL.Domain.validationpck;

public class EnumValidators
{
    public static ValidationResult DefinedValuesOnly(object value, ValidationContext validationContext)
    {
        if (!Enum.IsDefined(value.GetType(), value))
        {
            string memberName = validationContext.MemberName;
            string errorMessage = String.Format("The specified value of '{0}' is not defined"
                , memberName);
            return new ValidationResult(errorMessage, new string[] { memberName });
        }

        return ValidationResult.Success;
    }
}