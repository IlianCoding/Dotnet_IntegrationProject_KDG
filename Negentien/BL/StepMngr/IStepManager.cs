using Microsoft.AspNetCore.Http;
using NT.BL.Domain.flowpck;
using NT.BL.Domain.questionpck.QuestionDomPck;
using NT.BL.Domain.questionpck.AnswerDomPck;

namespace NT.BL.StepMngr;

public interface IStepManager
{
    UserAnswer AddUserAnswer(long answerId, long sessionId, bool answerOpen);
    void AddUserOpenAnswer(Session session, string answerString, long questionId);
    UserAnswer GetUserAnswer(long answerId, Session session);
    Step GetStep(long stepId);
    Step GetStepWithStep(long stepId);
    Step AddStep(Theme theme, Content content, State state, bool isConditioneel, Step nextStep);
    void AddStepToFlow(long stepFlowId, long stepId);
    void ChangeStep(long id, string stepDtoName, Theme stepDtoTheme, bool stepDtoIsConditioneel, Content stepDtoContent);
    Step AddStep(Theme theme, Content content, State state, bool isConditioneel, Step nextStep, string name);
    void RemoveStepAndRelocate(long stepId);
    void RemoveStep(long stepId, long flowId);
    void DeactivateStep(long stepId);
    void ActivateStep(long stepId);
    ConditionalPoint AddConditionalPoint(string name, Step step);
    ConditionalPoint GetConditionalPoint(long id);
    void ChangeStepIsConditioneel(long id,bool stepDtoIsConditioneel);
    QuestionWithOption AddQuestionWithOptions(string questionString, ICollection<string> options, string questionType);
    Content AddQuestionOpen(string questionText);
    Content AddInformation(string newInformationTitle, string newInformationTextInformation, string objectName, string contentType);
    Content GetContent(long stepContentId);
    void ChangeAnswerOptionCp(long id, ConditionalPoint conditionalPoint);
    void ChangeStepNextStep(long stepId, Step nextStep);
    Step GetStepWithStepContentThemeAndState(long stepId);
    AnswerOption GetAnswerOptionWithConditionalPoint(long answerId);
    void RemoveConditionalPoint(long conditionalPointId);
    ConditionalPoint GetConditionalPointWithConditionalStep(long cpId);
    Content ChangeQuestionWithOptions(long id, string questionText);
    Content ChangeQuestionOpen(long id, string questionText);
    Content ChangeInformation(long id, string updateInformationContentType, string updateInformationObjectName, string updateInformationTextInformation, string updateInformationTitle);
    ConditionalPoint ChangeConditionalPoint(long id, string conditionalPointName, Step step);
}