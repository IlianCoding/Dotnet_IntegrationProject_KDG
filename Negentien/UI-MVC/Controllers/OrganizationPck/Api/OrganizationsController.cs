using Microsoft.AspNetCore.Mvc;
using NT.BL.Domain.users;
using NT.BL.ProjectMngr;
using NT.BL.UserMngr;
using NT.UI.MVC.Models.Dto;
using NT.UI.MVC.Models.Dto.Project;

namespace NT.UI.MVC.Controllers.OrganizationPck.Api;

[Route("api/[controller]")]
[ApiController]
public class OrganizationsController : ControllerBase
{
    private readonly IGeneralUserManager _generalUserManager;
    private readonly IProjectManager _projectManager;

    public OrganizationsController(IGeneralUserManager generalUserManager, IProjectManager projectManager)
    {
        _generalUserManager = generalUserManager;
        _projectManager = projectManager;
    }
    
    [HttpPut("ReassignProject")]
    public IActionResult AssignProject([FromBody] AssignProjectDto projectDto)
    {
        if (!User.IsInRole(CustomIdentityConstants.Organization))
        {
            return Forbid();
        }

        if (projectDto == null || string.IsNullOrWhiteSpace(projectDto.Email))
        {
            return BadRequest("Invalid request data.");
        }

        var attendant = _generalUserManager.GetAttendentUserByEmail(projectDto.Email);
        if (attendant == null)
        {
            return NotFound($"Attendant with email {projectDto.Email} not found");
        }

        var project = _projectManager.GetProjectById(projectDto.ProjectId);
        if (project == null)
        {
            return NotFound($"Project with ID {projectDto.ProjectId} not found");
        }

        _generalUserManager.ChangeAttendentUserProject(attendant, project);
        return Ok($"Project successfully reassigned to attendant {projectDto.Email}");
    }
}