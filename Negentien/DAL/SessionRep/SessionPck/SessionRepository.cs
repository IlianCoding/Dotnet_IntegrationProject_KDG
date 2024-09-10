using System.Collections;
using Microsoft.EntityFrameworkCore;
using NT.BL.Domain.flowpck;
using NT.BL.Domain.users;
using NT.DAL.EF;

namespace NT.DAL.SessionRep.SessionPck;

public class SessionRepository : ISessionRepository
{
    private readonly PhygitalDbContext _phygitalDbContext;

    public SessionRepository(PhygitalDbContext phygitalDbContext)
    {
        _phygitalDbContext = phygitalDbContext ?? throw new ArgumentNullException(nameof(phygitalDbContext));
    }

    public Session ReadSessionById(long sessionId)
    {
        return _phygitalDbContext.Sessions
            .Include(incl => incl.UserAnswers)
            .SingleOrDefault(ses => ses.Id == sessionId);
    }

    public Session ReadSessionWithColorFromRunningFlow(Color color, long runningFlowId)
    {
        return _phygitalDbContext.Sessions
            .Include(incl => incl.Flow)
            .SingleOrDefault(col => col.Color == color && col.Flow.Id == runningFlowId);
    }

    public Session ReadSessionWithStateAndRunningFlow(State state, long currentFlowId)
    {
        return _phygitalDbContext.Sessions
            .Include(incl => incl.Flow)
            .SingleOrDefault(sta => sta.State == state && sta.Flow.Id == currentFlowId);
    }
    
    public IEnumerable<Session> ReadSessionsOfRunningFlows(long runningFlowId)
    {
        return _phygitalDbContext.Sessions
            .Include(s => s.UserAnswers)
            .Where(s => s.Flow.Id == runningFlowId);
    }

    public void CreateSession(Session session)
    {
        _phygitalDbContext.Sessions.Add(session);
        _phygitalDbContext.SaveChanges();
    }

    public void UpdateSessionStateAndColor(long sessionId, long runningFlowId)
    {
        var sessionForUpdating = _phygitalDbContext.Sessions
            .SingleOrDefault(ses => ses.Id == sessionId && ses.Flow.Id == runningFlowId);
        
        if (sessionForUpdating != null)
        {
            sessionForUpdating.State = State.Closed;
            sessionForUpdating.Color = Color.None;
            _phygitalDbContext.SaveChanges();
        }
    }
    
    public void UpdateSessionApplicationUser(Session session, ApplicationUser applicationUserFor)
    {
        Session changeableSession = _phygitalDbContext.Sessions
            .SingleOrDefault(srch => srch.Id == session.Id);
        
        if (changeableSession != null)
        {
            changeableSession.ApplicationUser = applicationUserFor;
        }

        _phygitalDbContext.SaveChanges();
    }

    public void UpdateSessionExecutionTime(long runningFlowId, DateTime endTime)
    {
        Session updateableSession = _phygitalDbContext.Sessions
            .SingleOrDefault(cl => cl.Flow.Id == runningFlowId && cl.State == State.Open);
        if (updateableSession != null)
        {
            TimeSpan duration = endTime - updateableSession.StartTime;
            TimeOnly totalTime = new TimeOnly(duration.Hours, duration.Minutes, duration.Seconds);

            updateableSession.ExecusionTime = totalTime;
            updateableSession.EndTime = endTime;
        }

        _phygitalDbContext.SaveChanges();
    }

    public void UpdateAllActiveSessionsOfRunningFlow(long runningFlowId)
    {
        IEnumerable<Session> sessions = _phygitalDbContext.Sessions
            .Where(sta => sta.Flow.Id == runningFlowId && sta.State == State.Open).ToList();
        DateTime endTime = DateTime.Now;

        if (sessions.Any())
        {
            foreach (Session session in sessions)
            {
                TimeSpan duration = endTime - session.StartTime;
                TimeOnly totalTime = new TimeOnly(duration.Hours, duration.Minutes, duration.Seconds);

                session.EndTime = endTime;
                session.ExecusionTime = totalTime;
                session.Color = Color.None;
                session.State = State.Closed;
            }
        }
        _phygitalDbContext.SaveChanges();
    }
    
    public void DeleteSessions(IEnumerable<Session> sessionsOfRunningFlow)
    {
        _phygitalDbContext.Sessions.RemoveRange(sessionsOfRunningFlow);
    }
}