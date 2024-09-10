using Microsoft.AspNetCore.Mvc;
using NT.BL.Domain.flowpck;
using NT.BL.Domain.users;
using NT.BL.SessionMngr;
using NT.BL.UserMngr;
using NT.UI.MVC.Models.UserViewModels;

namespace NT.UI.MVC.Controllers.EndUserPck.Api;

[Route("/api/[controller]")]
[ApiController]
public class EndUsersController : ControllerBase
{
    private readonly ISessionManager _sessionManager;
    private readonly IGeneralUserManager _generalUserManager;

    public EndUsersController(ISessionManager sessionManager, IGeneralUserManager generalUserManager)
    {
        _sessionManager = sessionManager;
        _generalUserManager = generalUserManager;
    }

    [HttpPut("LinkFinishedSessionToExistingUser")]
    public IActionResult LinkFinishedSessionToExistingUser([FromQuery] long runningFlowId, [FromBody] LinkingSessionViewModel linkingSessionViewModel)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("The email was not valid!");
        }

        Session session = _sessionManager.GetSessionByColorAndRunningFlow((Color)linkingSessionViewModel.Color, runningFlowId);
        ApplicationUser applicationUser = _generalUserManager.GetApplicationUserByEmail(linkingSessionViewModel.UserEmail);

        _sessionManager.ChangeApplicationUserInSession(session, applicationUser);

        return Ok();
    }
}