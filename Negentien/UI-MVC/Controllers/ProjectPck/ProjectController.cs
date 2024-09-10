using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NT.BL.Domain.users;
using NT.BL.ProjectMngr;

namespace NT.UI.MVC.Controllers.ProjectPck;

public class ProjectController : Controller
{
    private readonly IProjectManager _projectManager;

    public ProjectController(IProjectManager projectManager)
    {
        _projectManager = projectManager;
    }
    
     [HttpGet]
     public IActionResult AddProject()
     {
         if (User.IsInRole(CustomIdentityConstants.Organization))
         {
             
             return View();
         }

         return Forbid();
     }
    public IActionResult ProjectDetail(long id)
    {
        if (User.IsInRole(CustomIdentityConstants.Organization))
        {
            var project = _projectManager.GetProjectWithThemes(id);
            return View(project);
        }

        return Forbid();
    }

    public IActionResult AllProjects()
    {
        if (User.IsInRole(CustomIdentityConstants.Organization))
        {
            return View();
        }

        return Forbid();
    }

    public IActionResult ProjectInformation(long stepId)
    {
        if (User.IsInRole(CustomIdentityConstants.Organization))
        {
            var project = _projectManager.GetProjectByStepId(stepId);
            return View(project);
        }

        return Forbid();
    }
}