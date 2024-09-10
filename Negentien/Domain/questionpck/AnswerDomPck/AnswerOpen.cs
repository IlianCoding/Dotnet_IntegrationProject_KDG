using System.ComponentModel.DataAnnotations;

namespace NT.BL.Domain.questionpck.AnswerDomPck;

public class AnswerOpen : Answer
{
    [Required]
    [StringLength(200, MinimumLength = 1, ErrorMessage = "Answer should be between 1 and 200 characters long.")]
    public string Answer { get; set; }
}