using NT.BL.Domain.flowpck;
using NT.BL.Domain.users;

namespace NT.BL.SessionMngr;

public interface ISessionManager
{
    RunningFlow GetRunningFlowById(long flowId);
    RunningFlow GetRunningFlowByIdWithCurrentFlow(long runningFlowId);
    RunningFlow GetRunningFlowByIdAndTesting(long flowId);
    IEnumerable<RunningFlow> GetAllRunningFlowByCurrentFlowId(long currentFlowId);
    IEnumerable<RunningFlow> GetAllRunningFlow();
    Dictionary<string, List<Flow>> GetDistinctCreatedAttendantNamesByNotClosedRunningFlow();
    IEnumerable<RunningFlow> GetAllRunningFlowWithNotCloseStatus();
    Session GetSessionByColorAndRunningFlow(Color color, long currentFlowId);
    Session GetSessionByStateAndRunningFlow(State state, long currentFlowId);
    RunningFlow AddRunningFlow(DateTime runningFlowTime, State state, long currentFlowId, string createdAttendant, bool isKiosk, bool isTesting); 
    RunningFlow ChangeRunningFlow(long runningFlowId, State state);
    void AddSession(long runningFlowId, Color color);
    void ChangeApplicationUserInSession(Session session, ApplicationUser applicationUser);
    void ChangeExecusionTimeForSession(long runningFlowId, DateTime endTime);
    void ChangeAllActiveSessionsOfRunningFlow(long runningFlowId);
    IEnumerable<RunningFlow> GetAllNotClosedRunningFlowById(long flowId);
    IEnumerable<RunningFlow> GetAllClosedRunningFlowById(long flowId);
    IEnumerable<RunningFlow> GetAllNotClosedRunningFlowByIdAndAttendantName(long flowId, string identityName);
    IEnumerable<RunningFlow> GetAllClosedRunningFlowByIdAndAttendantName(long flowId, string identityName);
}