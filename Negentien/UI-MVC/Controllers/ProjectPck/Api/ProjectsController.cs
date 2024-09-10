using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NT.BL.Domain.projectpck;
using NT.BL.Domain.users;
using NT.BL.ProjectMngr;
using NT.BL.UserMngr;
using NT.UI.MVC.Models.Dto;
using NT.UI.MVC.Models.Dto.Project.NewDto;
using NT.UI.MVC.Models.Dto.Users;

namespace NT.UI.MVC.Controllers.ProjectPck.Api;

[Route("/api/[controller]")]
[ApiController]
public class ProjectsController : ControllerBase
{
    private readonly IProjectManager _projectManager;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IGeneralUserManager _generalUserManager;

    public ProjectsController(IProjectManager projectManager, UserManager<IdentityUser> userManager, IGeneralUserManager generalUserManager)
    {
        _projectManager = projectManager;
        _userManager = userManager;
        _generalUserManager = generalUserManager;
    }

    [HttpPost]
    public IActionResult AddNewProject(NewProjectDto newProjectDto)
    {
        if (User.IsInRole(CustomIdentityConstants.Organization))
        {
            var user = _userManager.GetUserAsync(User).Result;
            if (user != null)
            {
                var organizationUser = _generalUserManager.GetOrganizationUserByEmail(user.Email);
                var createdProject = _projectManager.AddProject(newProjectDto.Name, newProjectDto.IsActive, organizationUser, newProjectDto.ProjectInformation);
                if (createdProject == null)
                {
                    return BadRequest("Project not registered");
                }
                return CreatedAtAction("GetProject",
                    new
                    {
                        id = createdProject.Id
                    }, new ProjectDto
                    {
                        Name = createdProject.Name,
                        IsActive = createdProject.IsActive,
                        ProjectInformation = createdProject.ProjectInformation,
                    });
            }
            return NotFound("De gebruiker werd niet gevonden");
        }
        return Forbid();
    }

    [HttpGet]
    public ActionResult<IEnumerable<ProjectDto>> GetAllProjects()
    {
        if (User.IsInRole(CustomIdentityConstants.Organization))
        {
            var projects = _projectManager.GetAllProjects().ToList();
            if (!projects.Any())
            {
                return NoContent();
            }

            return Ok(projects.Select(project => new ProjectDto
            {
                Id = project.Id,
                Name = project.Name,
                IsActive = project.IsActive,
                ThemeId = project.Theme.Id
            }));
        }

        return Forbid();
    }


    [HttpGet("{projectId}")]
    public IActionResult GetProject(long projectId)
    {
        if (User.IsInRole(CustomIdentityConstants.Organization))
        {
            var project = _projectManager.GetProjectById(projectId);
            if (project == null)
            {
                return NotFound();
            }
            var projectDto = new ProjectDto()
            {
                Id = project.Id,
                Name = project.Name,
                IsActive = project.IsActive,
                ThemeId = project.Theme.Id,
                ProjectInformation = project.ProjectInformation,
                PrimaryColor = project.PrimaryColor,
                Font = project.Font
            };
            return Ok(projectDto);
        }

        return Forbid();
    }

    [HttpGet("{projectId}/attendants")]
    public IActionResult GetProjectAttendants(long projectId)
    {
        if (User.IsInRole(CustomIdentityConstants.Organization))
        {
            var project = _projectManager.GetProjectById(projectId);

            if (project == null)
            {
                return NotFound("The project couldn't be found!");
            }

            var attendants = _generalUserManager.GetAttendantUsersByProjectId(projectId);
            if (attendants == null)
            {
                return NoContent();
            }

            return Ok(attendants.Select(attendant => new AttendantDto
            {
                Firstname = attendant.FirstName,
                Lastname = attendant.LastName,
                Username = attendant.UserName,
                BirthDay = attendant.BirthDate
            }));
        }

        return Forbid();
    }

    [HttpPut("{projectId}/update")]
    public ActionResult<ProjectDto> UpdateProject(long projectId, UpdateProjectDto updateProjectDto)
    {
        if (User.IsInRole(CustomIdentityConstants.Organization))
        {
            var project = _projectManager
                .ChangeProject(projectId, updateProjectDto.Name,
                    updateProjectDto.IsActive, updateProjectDto.ProjectInformation, updateProjectDto.PrimaryColor, updateProjectDto.Font);
            if (project == null)
            {
                return BadRequest("Can not update project");
            }

            return Accepted();
        }

        return Forbid();
    }
    
    [HttpPut("{projectId}/updateStatus")]
    public ActionResult<ProjectDto> UpdateProjectStatus(long projectId, UpdateProjectDto updateProjectDto)
    {
        if (User.IsInRole(CustomIdentityConstants.Organization))
        {
            var project = _projectManager
                .ChangeProjectStatus(projectId, updateProjectDto.IsActive);
            if (project == null)
            {
                return BadRequest("Can not update project");
            }

            return Accepted();
        }

        return Forbid();
    }

    [HttpDelete("Delete/{projectId}")]
    public ActionResult DeleteProject(long projectId)
    {
        if (User.IsInRole(CustomIdentityConstants.Organization))
        {
            Project project = _projectManager.GetProjectById(projectId);
            if (project is null)
            {
                return NotFound();
            }
            _projectManager.RemoveProject(projectId);
            return Ok();
        }

        return Forbid();
    }
}