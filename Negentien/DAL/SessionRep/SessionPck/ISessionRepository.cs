using NT.BL.Domain.flowpck;
using NT.BL.Domain.users;

namespace NT.DAL.SessionRep.SessionPck;

public interface ISessionRepository
{
    Session ReadSessionById(long sessionId);
    Session ReadSessionWithColorFromRunningFlow(Color color, long runningFlowId);
    Session ReadSessionWithStateAndRunningFlow(State state, long currentFlowId);
    IEnumerable<Session> ReadSessionsOfRunningFlows(long runningFlowId);
    void CreateSession(Session session);
    void UpdateSessionStateAndColor(long sessionId, long runningFlowId);
    void UpdateSessionApplicationUser(Session session, ApplicationUser applicationUser);
    void UpdateSessionExecutionTime(long runningFlowId, DateTime endTime);
    void UpdateAllActiveSessionsOfRunningFlow(long runningFlowId);
    void DeleteSessions(IEnumerable<Session> sessionsOfRunningFlow);
}