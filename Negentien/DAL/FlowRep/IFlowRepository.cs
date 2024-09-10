using NT.BL.Domain.flowpck;
using NT.BL.Domain.questionpck.AnswerDomPck;
using NT.BL.Domain.questionpck.QuestionDomPck;
using NT.BL.Domain.Util;

namespace NT.DAL.FlowRep;

public interface IFlowRepository
{
    IEnumerable<Flow> ReadAllFlowsWithStepsWithNextStep();
    Flow ReadFlowWithSteps(long flowId);
    Flow ReadFlowById(long id);
    IEnumerable<Flow> ReadAllFlowsWithSteps();
    void UpdateFlow(Flow flow);
    void CreateFlow(Flow flow);
    Flow ReadFlowWithStepWithNextStepAndContent(long flowId);
    Flow ReadFlowByIdWithRunningFlow(long id);
    IEnumerable<Flow> ReadAllFlowsWithRunningFlowNotClosed();
    void DeleteStepFromFlow(long stepId, long flowId);
    Flow ReadAllFlowsWithStepsWithContentAndRunningFlow(long flowId);
    void DeleteFlow(Flow flow);
   long ReadProjectIdByFlow(long flowId);
   RunningFlow ReadFlowByRunningFlowId(long runningFlowId);
   IEnumerable<AnswerOption> ReadAllUserAnswerOptionsUnderFlow(long flowId);
   IEnumerable<OptionsWithQuestion> ReadAllSingleAndMultipleChoiceQuestions(long flowId);
   IEnumerable<StatisticalSession> GetStatisticalSessions(long flowId);
   IEnumerable<StatisticalQuestion> GetQuestionsOfFlow(long flowId);
}