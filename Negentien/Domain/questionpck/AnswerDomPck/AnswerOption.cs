using System.ComponentModel.DataAnnotations;
using NT.BL.Domain.flowpck;

namespace NT.BL.Domain.questionpck.AnswerDomPck;

public class AnswerOption : Answer
{
    public ConditionalPoint ConditionalPoint { get; set; }

    [Required]
    [StringLength(200, MinimumLength = 1, ErrorMessage = "Answer should be between 1 and 200 characters long.")]
    public string TextAnswer { get; set; }
}