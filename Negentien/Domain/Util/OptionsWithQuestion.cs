using NT.BL.Domain.questionpck.AnswerDomPck;
using NT.BL.Domain.questionpck.QuestionDomPck;

namespace NT.BL.Domain.Util;

public class OptionsWithQuestion
{
    public QuestionWithOption QuestionWithOption { get; set; }
    public IEnumerable<AnswerOption> AnswerOptions { get; set; }
}