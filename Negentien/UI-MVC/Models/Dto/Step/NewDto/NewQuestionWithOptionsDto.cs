using NT.BL.Domain.questionpck.AnswerDomPck;
using NT.BL.Domain.questionpck.QuestionDomPck;

namespace NT.UI.MVC.Models.Dto;

public class NewQuestionWithOptionsDto
{
    public string QuestionText { get; set; }
    public ICollection<string> AnswerOptions { get; set; }
    public string QuestionType { get; set; }
}