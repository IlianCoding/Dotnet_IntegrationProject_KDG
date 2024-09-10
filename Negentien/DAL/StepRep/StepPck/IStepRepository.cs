using NT.BL.Domain.flowpck;
using NT.BL.Domain.questionpck.AnswerDomPck;
using NT.BL.Domain.questionpck.QuestionDomPck;

namespace NT.DAL.StepRep.StepPck;

public interface IStepRepository
{
    Step ReadStep(long stepId);
    Step ReadStepWithQuestionOptions(long stepId);
    Step ReadStepWithQuestionOpen(long stepId);
    Step ReadStepWithStep(long stepId);
    IEnumerable<Step> ReadAllConditionalSteps();
    IEnumerable<Step> ReadStepsAttachedToTheme(Theme theme);
    ConditionalPoint UpdateConditionalPoint(ConditionalPoint conditionalPoint);
    void CreateStepToFlow(Flow flow, Step step);
    void CreateStep(Step newStep);
    void DeleteStep(long stepId);
    void UpdateStep(Step step);
    ConditionalPoint ReadConditionalPoint(long id);
    void UpdateAnswerOption(AnswerOption changedAnswerOption);
    Content ReadContent(long stepContentId);
    Step ReadStepWithStepContentThemeAndState(long stepId);
    IEnumerable<Step> ReadAllStepsWithNextStep();
    Step ReadStepWithContentAndTheme(long stepId);
    void DeleteQuestionWithOption(long id);
    void DeleteConditionalPoint(long id);
    void DeleteAnswerFromQuestionWithOption(long answerId, long questionId);
    void DeleteAnswerOption(long answerid);
    void DeleteQuestionOpen(long questionOpenId);
    ConditionalPoint ReadConditionalPointByStepid(long stepId);
    void DeleteInformationContent(long informationContentId);
    ConditionalPoint ReadConditionalPointWithConditionalStep(long cpId);
}