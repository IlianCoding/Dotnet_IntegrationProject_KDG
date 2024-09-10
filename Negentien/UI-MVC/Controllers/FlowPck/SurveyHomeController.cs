using Microsoft.AspNetCore.Mvc;
using NT.BL.FlowMngr;
using NT.BL.ProjectMngr;
using NT.BL.SessionMngr;

namespace NT.UI.MVC.Controllers.FlowPck;

public class SurveyHomeController : Controller
{
    private readonly IProjectManager _projectManager;
    private readonly IFlowManager _flowManager;
    private readonly ISessionManager _sessionManager;

    public SurveyHomeController(IProjectManager projectManager, IFlowManager flowManager,
        ISessionManager sessionManager)
    {
        _projectManager = projectManager;
        _flowManager = flowManager;
        _sessionManager = sessionManager;
    }

    [HttpGet]
    public IActionResult IndexSurveyHome([FromQuery] long runningFlowId, [FromQuery] long flowId)
    {
        var flow = _flowManager.GetFlowById(flowId);
        ViewBag.Flow = flow;
        var runningFlow = _sessionManager.GetRunningFlowById(runningFlowId);
        ViewBag.RunningFlow = runningFlow;
        if (flow is null && runningFlow is null)
        {
            return NoContent();
        }
        return View(runningFlow);
    }
}