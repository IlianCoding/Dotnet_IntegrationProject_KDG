using System.ComponentModel.DataAnnotations;
using NT.BL.Domain.flowpck;
using NT.BL.Domain.validationpck;

namespace NT.BL.Domain.questionpck.QuestionDomPck;

public abstract class Question : Content
{
    [Required]
    [CustomValidation(typeof(EnumValidators), "DefinedValuesOnly")]
    public QuestionType QuestionType { get; set; }
    [Required]
    [StringLength(1000, MinimumLength = 3, ErrorMessage = "Question should be between 3 and 1000 characters long.")]
    public string QuestionText { get; set; }

}