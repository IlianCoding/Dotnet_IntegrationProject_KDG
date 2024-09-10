using NT.BL.Domain.flowpck;
using NT.BL.Domain.questionpck.AnswerDomPck;
using NT.BL.Domain.questionpck.QuestionDomPck;

namespace NT.DAL.StepRep.QuestionPck;

public interface IQuestionRepository
{
    QuestionWithOption CreateQuestionWithOptions(QuestionWithOption questionWithOption);
    void UpdateAnswerOptionToQuestion(long id, AnswerOption newAnswerOption);
    QuestionWithOption ReadQuestionWithOptions(long id);
    Question ReadQuestionType(long questionId);
    QuestionWithOption ReadQuestionFromAnswer(AnswerOption answer);
    QuestionOpen ReadQuestionOpen(long questionOpenId);
    QuestionOpen CreateQuestionOpen(QuestionOpen questionOpen);
    Content UpdateQuestionWithOption(QuestionWithOption questionWithOption);
    Content UpdateQuestionOpen(QuestionOpen questionOpen);
    InformationContent ReadInformation(long id);
    Content UpdateInfo(InformationContent info);
}   