using Microsoft.AspNetCore.Mvc;
using NT.BL.Domain.flowpck;
using NT.BL.Domain.projectpck;
using NT.BL.ProjectMngr;
using NT.BL.WebPlatformMngr;

namespace NT.UI.MVC.Controllers.WebPlatformPck;

public class WebPlatformController : Controller
{
    private readonly IWebPlatformManager _webPlatformManager;
    private readonly IProjectManager _projectManager;

    public WebPlatformController(IWebPlatformManager webplatformManager, IProjectManager projectManager)
    {
        _webPlatformManager = webplatformManager;
        _projectManager = projectManager;
    }

    public IActionResult ThemeOverview(long projectId)
    {
        Project project = _projectManager.GetProjectWithThemes(projectId);
        return View(project);
    }
    
    
    public IActionResult ThemeDetail(long themeId, long projectId)
    {
        Theme theme = _projectManager.GetThemeById(themeId);
        ViewBag.ProjectId = projectId;
        return View(theme);
    }
    
}