using NT.BL.Domain.flowpck;
using NT.BL.Domain.users;
using NT.DAL.FlowRep;
using NT.DAL.SessionRep.RunningFlowPck;
using NT.DAL.SessionRep.SessionPck;

namespace NT.BL.SessionMngr;

public class SessionManager : ISessionManager
{
    private readonly IRunningFlowRepository _runningFlowRepository;
    private readonly IFlowRepository _flowRepository;
    private readonly ISessionRepository _sessionRepository;

    public SessionManager(IRunningFlowRepository runningFlowRepository, ISessionRepository sessionRepository,
        IFlowRepository flowRepository)
    {
        _runningFlowRepository = runningFlowRepository;
        _sessionRepository = sessionRepository;
        _flowRepository = flowRepository;
    }

    #region RunningFlow

    public RunningFlow GetRunningFlowById(long flowId)
    {
        return _runningFlowRepository.ReadRunningFlow(flowId);
    }

    public RunningFlow GetRunningFlowByIdAndTesting(long flowId)
    {
        return _runningFlowRepository.ReadFlowByIdAndTesting(flowId);
    }

    public RunningFlow AddRunningFlow(DateTime runningFlowTime, State state, long currentFlowId,
        string createdAttendant, bool isKiosk, bool isTesting)
    {
        if (isTesting)
        {
            Flow flowForTesting = _flowRepository.ReadFlowById(currentFlowId);
            if (flowForTesting != null)
            {
                RunningFlow runningFlowForTesting = new RunningFlow
                {
                    RunningFlowTime = runningFlowTime,
                    State = state,
                    CurrentFlow = flowForTesting,
                    IsKiosk = isKiosk,
                    IsForTesting = isTesting,
                    CreatedAttendantName = createdAttendant,
                    Sessions = new List<Session>()
                };
                _runningFlowRepository.CreateRunningFlow(runningFlowForTesting);
                return runningFlowForTesting;
            }
        }
        else
        {
            Flow flow = _flowRepository.ReadFlowById(currentFlowId);
            if (flow != null)
            {
                RunningFlow runningFlow = new RunningFlow
                {
                    RunningFlowTime = runningFlowTime,
                    State = state,
                    CurrentFlow = flow,
                    IsKiosk = isKiosk,
                    IsForTesting = false,
                    CreatedAttendantName = createdAttendant,
                    Sessions = new List<Session>()
                };
                _runningFlowRepository.CreateRunningFlow(runningFlow);
                return runningFlow;
            }
        }
        return null;
    }

    public IEnumerable<RunningFlow> GetAllRunningFlowByCurrentFlowId(long currentFlowId)
    {
        return _runningFlowRepository.ReadAllRunningFlowByCurrentFlowId(currentFlowId);
    }

    public RunningFlow ChangeRunningFlow(long runningFlowId, State state)
    {
        RunningFlow runningFlow = _runningFlowRepository.ReadRunningFlow(runningFlowId);
        runningFlow.State = state;
        _runningFlowRepository.UpdateRunningFlow(runningFlow);
        return runningFlow;
    }

    public IEnumerable<RunningFlow> GetAllRunningFlow()
    {
        return _runningFlowRepository.ReadAllRunningFlow();
    }

    public Dictionary<string, List<Flow>> GetDistinctCreatedAttendantNamesByNotClosedRunningFlow()
    {
       Dictionary<string, List<Flow>> uniqueAttendantNamesAndFlows = new Dictionary<string, List<Flow>>();
        if (uniqueAttendantNamesAndFlows == null) throw new ArgumentNullException(nameof(uniqueAttendantNamesAndFlows));
        var runningFlows = _runningFlowRepository.ReadAllRunningFlow()
            .Where(runflow => runflow.State != State.Closed).ToList();
        var uniqueAttendantNames = runningFlows
            .Select(runflow => runflow.CreatedAttendantName)
            .Distinct().ToList();
        foreach (var attendantName in uniqueAttendantNames)
        {
            var uniqueFlow = runningFlows
                .Where(runflow => runflow.CreatedAttendantName == attendantName)
                .Select(runflow => runflow.CurrentFlow)
                .Distinct()
                .ToList();
            uniqueAttendantNamesAndFlows.Add(attendantName, uniqueFlow);
        }

        return uniqueAttendantNamesAndFlows;
    }

    public IEnumerable<RunningFlow> GetAllRunningFlowWithNotCloseStatus()
    {
        return _runningFlowRepository.ReadAllRunningFlow().Where(rf => rf.State != State.Closed);
    }

    public RunningFlow GetRunningFlowByIdWithCurrentFlow(long runningFlowId)
    {
        
        return _runningFlowRepository.ReadRunningFlowByIdWithCurrentFlow(runningFlowId);
    }

    #endregion

    #region Session

    public Session GetSessionByColorAndRunningFlow(Color color, long currentFlowId)
    {
        return _sessionRepository.ReadSessionWithColorFromRunningFlow(color, currentFlowId);
    }

    public Session GetSessionByStateAndRunningFlow(State state, long currentFlowId)
    {
        return _sessionRepository.ReadSessionWithStateAndRunningFlow(state, currentFlowId);
    }
    
    public void AddSession(long runningFlowId, Color color)
    {
        Session session = _sessionRepository.ReadSessionWithColorFromRunningFlow(color, runningFlowId);

        if (session != null)
        {
            _sessionRepository.UpdateSessionStateAndColor(session.Id, session.Flow.Id);
        }

        Session newSession = new Session()
        {
            StartTime = DateTime.Now.ToUniversalTime(),
            Flow = _runningFlowRepository.ReadRunningFlow(runningFlowId),
            EndUser = new AnonymousUser(),
            Color = color,
            State = State.Open,
            UserAnswers = new List<UserAnswer>()
        };
        _sessionRepository.CreateSession(newSession);
    }

    public void ChangeApplicationUserInSession(Session session, ApplicationUser applicationUser)
    {
        _sessionRepository.UpdateSessionApplicationUser(session, applicationUser);
    }

    public void ChangeExecusionTimeForSession(long runningFlowId, DateTime endTime)
    {
        _sessionRepository.UpdateSessionExecutionTime(runningFlowId, endTime);

    }

    public void ChangeAllActiveSessionsOfRunningFlow(long runningFlowId)
    {
        _sessionRepository.UpdateAllActiveSessionsOfRunningFlow(runningFlowId);
    }

    #endregion
    

    public IEnumerable<RunningFlow> GetAllNotClosedRunningFlowById(long flowId)
    {
        return _runningFlowRepository.ReadAllRunningFlow()
            .Where(runningFlow => runningFlow.State != State.Closed && runningFlow.CurrentFlow.Id == flowId)
            .ToList();
    }

    public IEnumerable<RunningFlow> GetAllClosedRunningFlowById(long flowId)
    {
        return _runningFlowRepository.ReadAllRunningFlow()
            .Where(runningFlow => runningFlow.State == State.Closed && runningFlow.CurrentFlow.Id == flowId)
            .ToList();
    }

    public IEnumerable<RunningFlow> GetAllNotClosedRunningFlowByIdAndAttendantName(long flowId, string identityName)
    {
        return _runningFlowRepository.ReadAllRunningFlow()
            .Where(runningFlow => runningFlow.State != State.Closed && runningFlow.CurrentFlow.Id == flowId && runningFlow.CreatedAttendantName == identityName)
            .ToList();
    }

    public IEnumerable<RunningFlow> GetAllClosedRunningFlowByIdAndAttendantName(long flowId, string identityName)
    {
        return _runningFlowRepository.ReadAllRunningFlow()
            .Where(runningFlow => runningFlow.State == State.Closed && runningFlow.CurrentFlow.Id == flowId && runningFlow.CreatedAttendantName == identityName)
            .ToList();
    }
}