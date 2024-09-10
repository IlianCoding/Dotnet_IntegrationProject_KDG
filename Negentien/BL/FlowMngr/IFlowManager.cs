using NT.BL.Domain.flowpck;

namespace NT.BL.FlowMngr;

public interface IFlowManager
{
    Flow GetFlowWithStep(long flowId);
    Flow GetFlowWithStepWithNextStepAndContent(long flowId);
    Flow GetFlowById(long id);
    Flow GetFlowByStepId(long stepId);
    Flow ChangeFlow(long flowId, string flowName, State state, bool isLinear);
    Flow AddFlow(long projectId, State state, string flowName, bool isLinear);
    void ChangeFlowFirstStep(long flowId, Step nextStep);
    Step GetLastStep(long flowId);
    void ChangFlowFirstStep(long flowId, Step newStep);
    IEnumerable<Flow> GetFlowsWithRunningFlowNotClosed();
    RunningFlow GetRunningFlowById(long runningFlowId);
    void RemoveFlow(long flow);
    void RemoveRunningFlowByFlowIdAndTesting(long flowId);
    long GetProjectByFlow(long flowId);
    Flow GetFlowByIdWithStepWithContentWithRunningFlow(long flowId);
}