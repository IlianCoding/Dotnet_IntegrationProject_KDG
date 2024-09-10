using NT.BL.Domain.flowpck;
using NT.BL.Domain.questionpck.AnswerDomPck;
using NT.BL.Domain.questionpck.QuestionDomPck;

namespace NT.DAL.StepRep.AnswerPck;

public interface IAnswerRepository
{
    public Answer ReadAnswer(long answerId);
    public AnswerOption ReadAnswerOption(long answerId);
    public Answer CreateAnswerOpen(AnswerOpen answerOpen);
    void UpdateAnswerOptionConditionalPoint(AnswerOption answerOption);
    AnswerOption CreateAnswerOption(AnswerOption answerOption);
    AnswerOption ReadAnswerOptionWithConditionalPoint(long answerOptionId);
    IEnumerable<UserAnswer> ReadAllUserAnswersWithStep();
}