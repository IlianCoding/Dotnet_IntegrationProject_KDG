using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NT.BL.Domain.flowpck;
using NT.BL.Domain.questionpck.AnswerDomPck;
using NT.BL.Domain.questionpck.QuestionDomPck;
using NT.BL.Domain.users;
using NT.BL.FlowMngr;
using NT.BL.ProjectMngr;
using NT.BL.SessionMngr;
using NT.BL.StepMngr;
using NT.UI.MVC.Models;

namespace NT.UI.MVC.Controllers.FlowPck;

public class StepController : Controller
{
    private readonly IStepManager _stepManager;
    private readonly IFlowManager _flowManager;
    private readonly ISessionManager _sessionManager;
    private readonly IProjectManager _projectManager;

    public StepController(IStepManager stepManager, IFlowManager flowManager, IProjectManager projectManager,
        ISessionManager sessionManager)
    {
        _stepManager = stepManager;
        _flowManager = flowManager;
        _projectManager = projectManager;
        _sessionManager = sessionManager;
    }

    [HttpGet]
    public IActionResult ThankYouPage([FromQuery] long runningFlowId)
    {
        RunningFlow runningFlow = _sessionManager.GetRunningFlowByIdWithCurrentFlow(runningFlowId);
        if (runningFlow == null)
        {
            return NoContent();
        }

        ViewBag.Flow = runningFlow.CurrentFlow;
        return View(runningFlow);
    }

    [HttpGet]
    public IActionResult StepDetail(long stepId)
    {
        if (User.IsInRole(CustomIdentityConstants.Organization))
        {
            Step step = _stepManager.GetStepWithStepContentThemeAndState(stepId);
            Flow flow = _flowManager.GetFlowByStepId(stepId);
            ICollection<Step> conditionalSteps = new List<Step>();
            foreach (var flowStep in flow.Steps)
            {
                if (flowStep.IsConditioneel && flowStep.NextStep ==null)
                {
                    conditionalSteps.Add(flowStep);
                }
            }
            
            ViewBag.CS = conditionalSteps.IsNullOrEmpty();
            ViewBag.Flow = flow;
            return View(step);
        }

        return Forbid();
    }

    [HttpGet]
    public IActionResult UpdatePage(long stepId)
    {
        Step step = _stepManager.GetStepWithStepContentThemeAndState(stepId);
        ViewBag.Project = _projectManager.GetProjectByStepId(stepId);
        return View(step);
    }
    
    [HttpGet]
    public IActionResult NextStep([FromQuery] long runningFlowId, [FromQuery] long stepId, long? answerOptionId)
    {
        RunningFlow runningFlow = _sessionManager.GetRunningFlowById(runningFlowId);
        if (runningFlow == null)
        {
            return NoContent();
        }

        Step step = _stepManager.GetStepWithStepContentThemeAndState(stepId);
        Flow flow = _flowManager.GetFlowByStepId(stepId);
        Step nextStep= null;
        switch (step.NextStep)
        {
            case null when flow.IsLinear:
                DateTime endTime = DateTime.Now;
                _sessionManager.ChangeExecusionTimeForSession(runningFlowId, endTime);
                return RedirectToAction("ThankYouPage", "Step", new
                {
                    runningFlowId = runningFlow.Id
                });

            case null when !flow.IsLinear:
                return RedirectToAction("FirstStep", "Step", new
                {
                    runningFlowId = runningFlow.Id,
                    flowId = flow.Id
                });
                //nextStep = _stepManager.GetStepWithContentAndTheme(flow.FirstStep.Id);
                break;
            default:
            {
                step = _stepManager.GetStepWithStepContentThemeAndState(stepId);
                if (step.Content is QuestionWithOption && answerOptionId != null){
                    AnswerOption answer = _stepManager.GetAnswerOptionWithConditionalPoint(answerOptionId.Value);
            
                    if (answer.ConditionalPoint != null)
                    {            
                        nextStep = answer.ConditionalPoint.ConditionalStep;
                    }
                    else
                    {
                        nextStep = _stepManager.GetStepWithStepContentThemeAndState(step.NextStep.Id);
                    }
                }
                else
                {
                    nextStep = _stepManager.GetStepWithStepContentThemeAndState(step.NextStep.Id);
                }

                break;
            }
        }

        if (nextStep.StepState == State.Closed)
        {
           return RedirectToAction("NextStep", "Step", new
            {
                runningFlowId = runningFlow.Id,
                stepId=nextStep.Id
            });
        }
        ViewBag.Step = step;

        SurveyViewModel surveyViewModel = new SurveyViewModel()
        {
            IsKiosk = runningFlow.IsKiosk,
            Step = nextStep,
            IsLinear = flow.IsLinear,
            RunningFlowId = runningFlow.Id
        };
        
        return View(surveyViewModel);
    }

    [HttpGet]
    public IActionResult FirstStep([FromQuery] long runningFlowId, [FromQuery] long flowId)
    {
        RunningFlow runningFlow = _sessionManager.GetRunningFlowByIdWithCurrentFlow(runningFlowId);
        if (runningFlow == null)
        {
            bool testing = true;
            var testingFlow = _sessionManager.AddRunningFlow(DateTime.Now, State.Open, flowId, "Testing", true, testing);
            runningFlow = _sessionManager.GetRunningFlowByIdWithCurrentFlow(testingFlow.Id);
            if (runningFlow.IsForTesting != true)
            {
                return NoContent();
            }
        }

        if (runningFlow.IsKiosk)
        {
            _sessionManager.AddSession(runningFlow.Id, Color.None);
        }
        
        Flow flow = _flowManager.GetFlowWithStepWithNextStepAndContent(flowId);
        Step step = flow.FirstStep;
        if (step == null)
        {
            return NoContent();
        }
        if (step.StepState == State.Closed)
        {
            step =_stepManager.GetStepWithStepContentThemeAndState(step.NextStep.Id);
        }
        SurveyViewModel surveyViewModel = new SurveyViewModel()
        {
            Step = step,
            IsKiosk = _sessionManager.GetRunningFlowById(runningFlow.Id).IsKiosk,
            IsLinear = flow.IsLinear,
            RunningFlowId = runningFlow.Id
        };
        
        ViewBag.Step = step.NextStep;
        return View(surveyViewModel);
    }

    [HttpGet]
    public IActionResult AddStep([FromQuery]long flowId)
    {
        if (User.IsInRole(CustomIdentityConstants.Organization))
        {
            ViewBag.Project = _projectManager.GetProjectByFlowId(flowId);
            Flow flow = _flowManager.GetFlowById(flowId);
            return View(flow);
        }

        return Forbid();
    }

    [HttpGet("Conditional/{flowId}")]
    public IActionResult AddConditionalStep(long flowId)
    {
        if (User.IsInRole(CustomIdentityConstants.Organization))
        {
            ViewBag.Project = _projectManager.GetProjectByFlowId(flowId);
            Flow flow = _flowManager.GetFlowById(flowId);
            return View(flow);
        }

        return Forbid();
    }

    [HttpGet]
    public IActionResult OpenRangeMobile([FromQuery] long runningFlowId, [FromQuery] long stepId)
    {
        RunningFlow runningFlow = _sessionManager.GetRunningFlowByIdWithCurrentFlow(runningFlowId);
        if (runningFlow == null)
        {
            return NoContent();
        }
        SurveyViewModel surveyViewModel = new SurveyViewModel()
        {
            IsKiosk = runningFlow.IsKiosk,
            Step = _stepManager.GetStepWithStepContentThemeAndState(stepId),
            RunningFlowId = runningFlow.Id
        };

        return View(surveyViewModel);
    }
    [HttpGet]
    public IActionResult FinishOpenQuestionMobilePage([FromQuery] long runningFlowId)
    {
        RunningFlow runningFlow = _sessionManager.GetRunningFlowByIdWithCurrentFlow(runningFlowId);
        if (runningFlow == null)
        {
            return NoContent();
        }
        return View(runningFlow);
    }
    [HttpGet]
    public IActionResult FinishMobilePage([FromQuery] long runningFlowId)
    {
        RunningFlow runningFlow = _sessionManager.GetRunningFlowByIdWithCurrentFlow(runningFlowId);
        if (runningFlow == null)
        {
            return NoContent();
        }
        return View(runningFlow);
    }
    [HttpGet]
    public IActionResult ThankYouMobilePage([FromQuery] long runningFlowId)
    {
        RunningFlow runningFlow = _sessionManager.GetRunningFlowByIdWithCurrentFlow(runningFlowId);
        if (runningFlow == null)
        {
            return NoContent();
        }
        ViewBag.Flow = runningFlow.CurrentFlow;
        return View(runningFlow);
    }

}