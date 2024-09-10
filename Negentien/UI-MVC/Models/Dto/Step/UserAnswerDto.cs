using NT.BL.Domain.questionpck.AnswerDomPck;

namespace NT.UI.MVC.Models.Dto.Step;

public class UserAnswerDto
{
    public long AnswerId { get; set; }
    public BL.Domain.flowpck.Session Session { get; set; }
    public AnswerOption AnswerOption { get; set; }
    public AnswerOpen AnswerOpen { get; set; }
    public long StepId { get; set; }
}