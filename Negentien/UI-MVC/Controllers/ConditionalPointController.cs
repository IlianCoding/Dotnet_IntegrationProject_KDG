using Microsoft.AspNetCore.Mvc;
using NT.BL.Domain.flowpck;
using NT.BL.Domain.users;
using NT.BL.FlowMngr;
using NT.BL.StepMngr;

namespace NT.UI.MVC.Controllers;

public class ConditionalPointController : Controller
{
    
    private readonly IStepManager _stepManager;
    private readonly IFlowManager _flowManager;

    public ConditionalPointController(IStepManager stepManager, IFlowManager flowManager)
    {
        _stepManager = stepManager;
        _flowManager = flowManager;
    }
    [HttpGet]
    public IActionResult AddConditionalPoint(long stepId)
    {
        if (User.IsInRole(CustomIdentityConstants.Organization))
        {
            ViewBag.Flow = _flowManager.GetFlowByStepId(stepId);
            Step step = _stepManager.GetStepWithStepContentThemeAndState(stepId);
            return View(step);
        }

        return Forbid();
        
    }

    [HttpGet("ConditionalPoint/UpdateConditionalPoint/{cpId}/{stepId}")]
    public IActionResult UpdateConditionalPoint(long stepId, long cpId)
    {
        if (User.IsInRole(CustomIdentityConstants.Organization))
        {
            Step step = _stepManager.GetStepWithStepContentThemeAndState(stepId);
            ConditionalPoint conditionalPoint = _stepManager.GetConditionalPointWithConditionalStep(cpId);
            Flow flow = _flowManager.GetFlowByStepId(stepId);
            ViewBag.Flow = flow;
            ViewBag.CP = conditionalPoint;
            return View(step);
        }

        return Forbid();
    }

    [HttpDelete("/api/ConditionalPoints/RemoveCP/${cpId}")]
    public ActionResult DeleteStep(long cpId)
    {
        if (User.IsInRole(CustomIdentityConstants.Organization))
        {
            _stepManager.RemoveConditionalPoint(cpId);
            return Accepted();
        }

        return Forbid();
    }
}