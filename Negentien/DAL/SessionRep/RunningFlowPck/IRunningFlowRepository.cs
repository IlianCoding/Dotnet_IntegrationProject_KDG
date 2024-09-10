using NT.BL.Domain.flowpck;

namespace NT.DAL.SessionRep.RunningFlowPck;

public interface IRunningFlowRepository
{
    RunningFlow ReadRunningFlow(long flowId);
    void CreateRunningFlow(RunningFlow runningFlow);
    RunningFlow ReadFlowByIdAndTesting(long flowId);
    IEnumerable<RunningFlow> ReadAllRunningFlowByCurrentFlowId(long currentFlowId);
    void UpdateRunningFlow(RunningFlow runningFlow);
    IEnumerable<RunningFlow> ReadAllRunningFlow();
    RunningFlow ReadRunningFlowByIdWithCurrentFlow(long runningFlowId);
    IEnumerable<RunningFlow> ReadRunningFlowsByFlowId(long flowId);
    void DeleteRunningFlowByFlowIdAndTest(long flowId);
    void DeleteRunningFlows(IEnumerable<RunningFlow> runningFlowOfFlow);
}