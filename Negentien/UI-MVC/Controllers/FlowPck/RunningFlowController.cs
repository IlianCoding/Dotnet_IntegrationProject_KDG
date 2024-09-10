using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using NT.BL.Domain.users;
using NT.BL.SessionMngr;

namespace NT.UI.MVC.Controllers.FlowPck;

public class RunningFlowController : Controller
{
    
    private readonly ISessionManager _sessionManager;

    public RunningFlowController(ISessionManager sessionManager)
    {
        _sessionManager = sessionManager;
    }

    public IActionResult GetAllRunningFlow()
    {
        if (User.IsInRole(CustomIdentityConstants.Attendent) || User.IsInRole(CustomIdentityConstants.Organization))
        {
            var runningFlows = _sessionManager.GetAllRunningFlow();
            return View(runningFlows);
        }

        return Forbid();
    }
}