using NT.BL.Domain.questionpck.AnswerDomPck;

namespace NT.BL.Domain.questionpck.QuestionDomPck;

public class QuestionWithOption : Question
{
    public ICollection<AnswerOption> AnswerOptions { get; set; }
}