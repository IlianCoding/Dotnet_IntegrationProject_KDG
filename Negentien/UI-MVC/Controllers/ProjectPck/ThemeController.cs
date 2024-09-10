using Microsoft.AspNetCore.Mvc;
using NT.BL.Domain.flowpck;
using NT.BL.Domain.projectpck;
using NT.BL.Domain.users;
using NT.BL.FlowMngr;
using NT.BL.ProjectMngr;
using NT.BL.UnitOfWorkPck;
using NT.UI.MVC.Models;

namespace NT.UI.MVC.Controllers.ProjectPck;

public class ThemeController : Controller
{
    private readonly IProjectManager _projectManager;
    private readonly IFlowManager _flowManager;
    
    private readonly UnitOfWork _unitOfWork;

    public ThemeController(IProjectManager projectManager, IFlowManager flowManager, UnitOfWork unitOfWork)
    {
        _projectManager = projectManager;
        _flowManager = flowManager;
        _unitOfWork = unitOfWork;
    }

    public IActionResult ThemeDetail(long themeId, long projectId)
    {
        if (User.IsInRole(CustomIdentityConstants.Organization))
        {
            Theme theme = _projectManager.GetThemeById(themeId);
            Project project = _projectManager.GetProjectById(projectId);
            ViewBag.Project = project;
            return View(theme);
        }

        return Forbid();
    }

    [HttpGet]
    public IActionResult AddSubTheme(long flowId)
    {
        if (User.IsInRole(CustomIdentityConstants.Organization))
        {
            Project project = _projectManager.GetProjectByFlowId(flowId);
            ViewBag.Project = project;
            ViewBag.FlowId = flowId;
            NewSubThemeViewModel newSubThemeViewModel = new NewSubThemeViewModel
            {

            };
            return View(newSubThemeViewModel);
        }

        return Forbid();
    }

    [HttpPost]
    public IActionResult AddSubTheme(long flowId, NewSubThemeViewModel newSubThemeViewModel)
    {
        if (User.IsInRole(CustomIdentityConstants.Organization))
        {
            Project project = _projectManager.GetProjectByFlowId(flowId);
            Flow flow = _flowManager.GetFlowById(flowId);
            _unitOfWork.BeginTransaction();
            _projectManager.AddThemeToProject(newSubThemeViewModel.ShortInformation, newSubThemeViewModel.ThemeName,false,
                project.Id);
        
            _unitOfWork.Commit();
            return RedirectToAction("AddStep", "Step",new
            {
                flowId = flow.Id
            });
        }

        return Forbid();
    }
}