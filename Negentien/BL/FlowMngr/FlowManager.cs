using System.ComponentModel.DataAnnotations;
using System.Text;
using NT.BL.Domain.flowpck;
using NT.BL.Domain.projectpck;
using NT.BL.Domain.users;
using NT.BL.StepMngr;
using NT.DAL.FlowRep;
using NT.DAL.ProjectRep.ProjectPck;
using NT.DAL.SessionRep.RunningFlowPck;
using NT.DAL.SessionRep.SessionPck;
using NT.DAL.StepRep.UserAnswerPck;
using NT.DAL.User.AnonymousUser;
using NT.DAL.User.ApplicationPck;

namespace NT.BL.FlowMngr;

public class FlowManager : IFlowManager
{
    private readonly IFlowRepository _flowRepository;
    private readonly IRunningFlowRepository _runningFlowRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly ISessionRepository _sessionRepository;
    private readonly IUserAnswerRepository _userAnswerRepository;
    private readonly IAnonymousUserRepository _anonymousUserRepository;
    private readonly IApplicationUserRepository _applicationUserRepository;
    private readonly Lazy<IStepManager> _lazyStepManager;

    public FlowManager(IFlowRepository flowRepository, IProjectRepository projectManager,
        IRunningFlowRepository runningFlowRepository,
        ISessionRepository sessionRepository, IUserAnswerRepository userAnswerRepository,
        IAnonymousUserRepository anonymousUserRepository, IApplicationUserRepository applicationUserRepository, Lazy<IStepManager> lazyStepManager)
    {
        _flowRepository = flowRepository;
        _projectRepository = projectManager;
        _runningFlowRepository = runningFlowRepository;
        _sessionRepository = sessionRepository;
        _userAnswerRepository = userAnswerRepository;
        _anonymousUserRepository = anonymousUserRepository;
        _applicationUserRepository = applicationUserRepository;
        _lazyStepManager = lazyStepManager;
    }

    public Flow GetFlowWithStep(long flowId)
    {
        return _flowRepository.ReadFlowWithSteps(flowId);
    }

    public Flow GetFlowWithStepWithNextStepAndContent(long flowId)
    {
        return _flowRepository.ReadFlowWithStepWithNextStepAndContent(flowId);
    }


    public Flow GetFlowById(long id)
    {
        return _flowRepository.ReadFlowById(id);
    }

    public Flow GetFlowByStepId(long stepId)
    {
        IEnumerable<Flow> flows = _flowRepository.ReadAllFlowsWithStepsWithNextStep();
        foreach (var flow in flows)
        {
            foreach (var step in flow.Steps)
            {
                if (step.Id == stepId)
                {
                    return flow;
                }
            }
        }

        return null;
    }

    public Flow ChangeFlow(long flowId, string flowName, State state, bool isLinear)
    {
        Flow flow = _flowRepository.ReadFlowWithSteps(flowId);
        if (flow == null)
        {
            return null;
        }

        flow.FlowName = flowName;
        flow.State = state;
        flow.IsLinear = isLinear;
        _flowRepository.UpdateFlow(flow);
        return flow;
    }

    public Flow AddFlow(long projectId, State state, string flowName, bool isLinear)
    {
        Flow newFlow = new Flow()
        {
            FlowName = flowName,
            IsLinear = isLinear,
            State = state
        };

        ValidateFlow(newFlow);
        _flowRepository.CreateFlow(newFlow);
        ConnectFlowToProject(projectId, newFlow);

        return newFlow;
    }

    public Flow GetFlowByIdWithRunningFlow(long id)
    {
        return _flowRepository.ReadFlowByIdWithRunningFlow(id);
    }

    public IEnumerable<Flow> GetFlowsWithRunningFlowNotClosed()
    {
        return _flowRepository.ReadAllFlowsWithRunningFlowNotClosed();
    }

    public Flow GetFlowByIdWithStepWithContentWithRunningFlow(long flowId)
    {
        return _flowRepository.ReadAllFlowsWithStepsWithContentAndRunningFlow(flowId);
    }

    public RunningFlow GetRunningFlowById(long runningFlowId)
    {
        return _flowRepository.ReadFlowByRunningFlowId(runningFlowId);
    }

    public void RemoveFlow(long flowId)
    {
        Flow flow = _flowRepository.ReadFlowById(flowId);
        DeleteRunningFlowsAttachedToFlow(flowId);
        DeleteStepsAttachedToFlow(flowId);

        _flowRepository.DeleteFlow(flow);
    }

    public long GetProjectByFlow(long flowId)
    {
        return _flowRepository.ReadProjectIdByFlow(flowId);
    }

    public void ChangeFlowFirstStep(long flowId, Step nextStep)
    {
        var flow = _flowRepository.ReadFlowWithStepWithNextStepAndContent(flowId);
        flow.FirstStep = nextStep;
        _flowRepository.UpdateFlow(flow);
    }

    public Step GetLastStep(long flowId)
    {
        Flow flow = _flowRepository.ReadFlowWithSteps(flowId);
        foreach (var step in flow.Steps)
        {
            if (!step.IsConditioneel && step.NextStep == null)
            {
                return step;
            }
        }

        return null;
    }

    public void ChangFlowFirstStep(long flowId, Step newStep)
    {
        Flow flow = _flowRepository.ReadFlowById(flowId);
        flow.FirstStep = newStep;
        _flowRepository.UpdateFlow(flow);
    }
    public void RemoveFlow(Flow flow)
    {
        DeleteRunningFlowsAttachedToFlow(flow.Id);
        DeleteStepsAttachedToFlow(flow.Id);
       
        _flowRepository.DeleteFlow(flow);
    }

    public void RemoveRunningFlowByFlowIdAndTesting(long flowId)
    {
        RemoveSessionsAttachedToTestingRunningFlow(_runningFlowRepository.ReadFlowByIdAndTesting(flowId).Id);
        _runningFlowRepository.DeleteRunningFlowByFlowIdAndTest(flowId);
    }

    private void RemoveSessionsAttachedToTestingRunningFlow(long runningFlowId)
    {
        var sessionsOfRunningFlow = _sessionRepository.ReadSessionsOfRunningFlows(runningFlowId).ToList();

        RemoveUserAnswersAttachedToSessions(sessionsOfRunningFlow);
        _sessionRepository.DeleteSessions(sessionsOfRunningFlow);
    }

    private void DeleteRunningFlowsAttachedToFlow(long flowId)
    {
        var runningFlowOfFlow = _runningFlowRepository.ReadRunningFlowsByFlowId(flowId).ToList();

        RemoveSessionsAttachedToRunningFlows(runningFlowOfFlow);
        _runningFlowRepository.DeleteRunningFlows(runningFlowOfFlow);
    }

    private void RemoveSessionsAttachedToRunningFlows(IEnumerable<RunningFlow> runningFlowOfFlow)
    {
        foreach (RunningFlow runningFlow in runningFlowOfFlow)
        {
            var sessionsOfRunningFlow = _sessionRepository.ReadSessionsOfRunningFlows(runningFlow.Id).ToList();
            IEnumerable<ApplicationUser> applicationUsers = _applicationUserRepository.ReadAllApplicationUserWithCommentWithSession().ToList();
            foreach (Session session in sessionsOfRunningFlow)
            {
                foreach (ApplicationUser applicationUser in applicationUsers)
                {
                    if (applicationUser.Sessions.SingleOrDefault(s => s.Id == session.Id) != null)
                    {
                        applicationUser.Sessions.Remove(session);
                        _applicationUserRepository.UpdateApplicationUser(applicationUser);
                    }
                }
            }
            RemoveUserAnswersAttachedToSessions(sessionsOfRunningFlow);
            RemoveAnomymousUserAttachedToSessions(sessionsOfRunningFlow);
            _sessionRepository.DeleteSessions(sessionsOfRunningFlow);
        }
    }

    private void RemoveAnomymousUserAttachedToSessions(List<Session> sessionsOfRunningFlow)
    {
        IEnumerable<AnonymousUser> anonymousUsers = _anonymousUserRepository.ReadAllAnonymousUserWithSession().ToList();
        foreach (Session session in sessionsOfRunningFlow)
        {
            _anonymousUserRepository.DeleteAnonymousUsers(anonymousUsers.Where(an => an.Session.Id == session.Id));
        }
    }

    private void RemoveUserAnswersAttachedToSessions(IEnumerable<Session> sessionsOfRunningFlow)
    {
        foreach (Session session in sessionsOfRunningFlow)
        {
            _userAnswerRepository.DeleteUserAnswers(session.UserAnswers);
        }
    }

    private void DeleteStepsAttachedToFlow(long flowId)
    {
        Flow flow = _flowRepository.ReadFlowWithSteps(flowId);
        flow.FirstStep = null;
        _flowRepository.UpdateFlow(flow);
        _flowRepository.ReadFlowWithStepWithNextStepAndContent(flow.Id);
        foreach (var step in flow.Steps)
        {
            _lazyStepManager.Value.RemoveStep(step.Id, flowId);
        }
    }

    private void ConnectFlowToProject(long projectId, Flow flow)
    {
        Project project = _projectRepository.ReadProjectWithFlows(projectId);
        if (project == null)
        {
            return;
        }

        project.Flows.Add(flow);

        _projectRepository.Update(project);
    }

    private void ValidateFlow(Flow flow)
    {
        List<ValidationResult> errors = new List<ValidationResult>();
        ValidationContext vc = new ValidationContext(flow);
        bool valid = Validator.TryValidateObject(flow, vc, errors, true);

        if (!valid)
        {
            StringBuilder builder = new StringBuilder();
            foreach (ValidationResult vResult in errors)
            {
                builder.Append(vResult.ErrorMessage + " at members: ");
                foreach (string member in vResult.MemberNames)
                {
                    builder.Append(member + ", ");
                }

                builder.Append('\n');
            }

            throw new ValidationException(builder.ToString());
        }
    }
}