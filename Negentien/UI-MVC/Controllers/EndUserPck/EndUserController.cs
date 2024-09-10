using Microsoft.AspNetCore.Mvc;
using NT.BL.Domain.flowpck;
using NT.BL.Domain.projectpck;
using NT.BL.Domain.users;
using NT.BL.ProjectMngr;
using NT.BL.services;
using NT.BL.SessionMngr;
using NT.BL.UserMngr;
using NT.UI.MVC.Models.UserViewModels;

namespace NT.UI.MVC.Controllers.EndUserPck;

public class EndUserController : Controller
{
    private readonly ISessionManager _sessionManager;
    private readonly IGeneralUserManager _generalUserManager;
    private readonly IProjectManager _projectManager;
    private readonly EmailSender _emailSender;

    public EndUserController(ISessionManager sessionManager, IGeneralUserManager generalUserManager, IProjectManager projectManager, EmailSender emailSender)
    {
        _sessionManager = sessionManager;
        _generalUserManager = generalUserManager;
        _projectManager = projectManager;
        _emailSender = emailSender;
    }

    [HttpGet]
    public IActionResult FillInContact([FromQuery] long runningFlowId)
    {
        RunningFlow runningFlow = _sessionManager.GetRunningFlowByIdWithCurrentFlow(runningFlowId);
        ViewBag.RunningFlow = runningFlow;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> FillInContact([FromQuery] long runningFlowId, NewUserViewModel user)
    {
        RunningFlow runningFlow = _sessionManager.GetRunningFlowByIdWithCurrentFlow(runningFlowId);
        if (!ModelState.IsValid)
        {
            ViewBag.RunningFlow = runningFlow;
            return View();
        }
        
        if (runningFlow.IsKiosk)
        {
            await _generalUserManager.AddApplicationUser(user.FirstName, user.LastName, user.Email, user.PhoneNumber,
                user.MoreInfo,
                user.Birthday, user.Password, runningFlow, (Color)6);
        }
        else
        {
            await _generalUserManager.AddApplicationUser(user.FirstName, user.LastName, user.Email, user.PhoneNumber, user.MoreInfo,
                user.Birthday, user.Password, runningFlow, (Color)user.Color);
        }
        
        Project project = _projectManager.GetProjectByRunningFlowId(runningFlowId);
        
        string themeOverviewUrl = Url.ActionLink("ThemeOverview", "WebPlatform", new { projectId = project.Id });
        string mailBody = $"Hello {user.FirstName} {user.LastName},<br><br>" +
                          $"You have been added to the project {project.Name}.<br>" +
                          $"Please use <a href='{themeOverviewUrl}'>this link</a> to go to the themes overview for this project.<br>" +
                          $"Here you can leave ideas about the themes of this project.<br><br>" +
                          $"Kind regards,<br>" +
                          $"The Phygital team";
        
        await _emailSender.SendMailForKnowledgeOfUserCreation(user.Email, "You have been added to a Phygital project", mailBody, user.FirstName, user.LastName);
        
        ModelState.Clear();
        if (!runningFlow.IsKiosk)
        {
            return RedirectToAction("FinishMobilePage", "Step", new
            {
                runningFlowId = runningFlow.Id
            });
        }
        else
        {
            return RedirectToAction("IndexSurveyHome", "SurveyHome", new
            {
                runningFlowId = runningFlow.Id,
                flowId = runningFlow.CurrentFlow.Id
            });
        }
    }

    [HttpGet]
    public IActionResult LinkFinishedSessionToExistingUser([FromQuery] long runningFlowId)
    {
        var runningFlow = _sessionManager.GetRunningFlowById(runningFlowId);
        ViewBag.RunningFlow = runningFlow;
        return View();
    }

    [HttpPut]
    public async Task<IActionResult> LinkFinishedSessionToExistingUser([FromQuery] long runningFlowId, [FromBody] LinkingSessionViewModel linkingSessionViewModel)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("The email was not valid!");
        }

        Session session = _sessionManager.GetSessionByColorAndRunningFlow((Color)linkingSessionViewModel.Color, runningFlowId);
        ApplicationUser applicationUser = _generalUserManager.GetApplicationUserByEmail(linkingSessionViewModel.UserEmail);

       _sessionManager.ChangeApplicationUserInSession(session, applicationUser);
        
        
        Project project = _projectManager.GetProjectByRunningFlowId(runningFlowId);
        
        string themeOverviewUrl = Url.ActionLink("ThemeOverview", "WebPlatform", new { projectId = project.Id });
        string mailBody = $"Hello {applicationUser.FirstName} {applicationUser.LastName},<br><br>" +
                          $"You have been added to the project {project.Name}.<br>" +
                          $"Please use <a href='{themeOverviewUrl}'>this link</a> to go to the themes overview for this project.<br>" +
                          $"Here you can leave ideas about the themes of this project.<br><br>" +
                          $"Kind regards,<br>" +
                          $"The Phygital team";
        
        await _emailSender.SendMailForKnowledgeOfUserCreation(applicationUser.Email, "You have been added to a Phygital project", mailBody, applicationUser.FirstName, applicationUser.LastName);

        return Ok();
    }
}