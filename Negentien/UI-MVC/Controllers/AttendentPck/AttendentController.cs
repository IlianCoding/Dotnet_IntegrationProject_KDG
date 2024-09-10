using System.Security.AccessControl;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NT.BL.Domain.users;
using NT.BL.ProjectMngr;
using NT.BL.UserMngr;

namespace NT.UI.MVC.Controllers.AttendentPck;

public class AttendentController : Controller
{
    private readonly IGeneralUserManager _generalUserManager;
    private readonly IProjectManager _projectManager;
    private readonly UserManager<IdentityUser> _userManager;

    public AttendentController(IGeneralUserManager generalUserManager, IProjectManager projectManager, UserManager<IdentityUser> userManager)
    {
        _generalUserManager = generalUserManager;
        _projectManager = projectManager;
        _userManager = userManager;
    }

    [HttpGet]
    public IActionResult AttendantFlowOverview()
    {
        if (User.IsInRole(CustomIdentityConstants.Attendent))
        {
            var user = _userManager.GetUserAsync(User).Result;
            if (user != null)
            {
                var assignedProject = _generalUserManager.GetAttendentUserByEmail(user.Email).AssignedProject;
                return View(assignedProject);
            }

            return NotFound("The user was not found");
        }

        return Forbid();
    }
}