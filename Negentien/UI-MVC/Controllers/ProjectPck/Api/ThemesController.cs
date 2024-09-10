using Microsoft.AspNetCore.Mvc;
using NT.BL.Domain.flowpck;
using NT.BL.Domain.users;
using NT.BL.ProjectMngr;
using NT.BL.UnitOfWorkPck;
using NT.UI.MVC.Models.Dto;

namespace NT.UI.MVC.Controllers.ProjectPck.Api;

[ApiController]
[Route("/api/[controller]")]
public class ThemesController : ControllerBase
{
    private readonly IProjectManager _projectManager;
    private readonly UnitOfWork _unitOfWork;

    public ThemesController(IProjectManager projectManager, UnitOfWork unitOfWork)
    {
        _projectManager = projectManager;
        _unitOfWork = unitOfWork;
    }

    [HttpGet("headThemes")]
    public ActionResult<IEnumerable<ThemeDto>> GetAllHeadThemes()
    {
        if (User.IsInRole(CustomIdentityConstants.Organization))
        {
            var headTemes = _projectManager.GetAllHeadThemes().ToList();
            if (!headTemes.Any())
            {
                return NoContent();
            }

            return Ok(headTemes.Select(theme => new ThemeDto()
            {
                Id = theme.Id,
                Name = theme.ThemeName
            }));
        }

        return Forbid();
    }

    [HttpGet]
    [Route("{id}")]
    public IActionResult Get(int id)
    {
        if (User.IsInRole(CustomIdentityConstants.Organization))
        {
            Theme theme = _projectManager.GetThemeById(id);

            if (theme == null)
            {
                return NotFound();
            }

            var themeDto = new ThemeDto
            {
                Id = theme.Id,
                Name = theme.ThemeName,
            };
            return Ok(themeDto);
        }

        return Forbid();
    }

    [HttpPost]
    public IActionResult Post(NewThemeDto theme) 
    {
        if (User.IsInRole(CustomIdentityConstants.Organization))
        {
            _unitOfWork.BeginTransaction();
            Theme themeToSend = _projectManager.AddThemeToProject(theme.ShortInformation, theme.ThemeName,
                theme.IsHeadTheme, theme.ProjectId);
            _unitOfWork.Commit();

            if (themeToSend == null)
            {
                return BadRequest();
            }

            return CreatedAtAction("Get", new { Id = themeToSend.Id }, themeToSend);
        }

        return Forbid();
    }

    [HttpPut]
    public IActionResult Put(UpdateThemeDto themeDto) 
    {
        if (User.IsInRole(CustomIdentityConstants.Organization))
        {
            var theme = _projectManager.GetTheme(themeDto.ProjectId, themeDto.OldThemeName, themeDto.IsHeadTheme);

            if (theme == null)
            {
                return NotFound();
            }

            _unitOfWork.BeginTransaction();
            var isChangeDone =
                _projectManager.Change(theme, themeDto.ShortInformation, themeDto.NewThemeName, themeDto.ProjectId);
            _unitOfWork.Commit();

            if (!isChangeDone)
            {
                return BadRequest();
            }

            return Ok(theme);
        }

        return Forbid();
    }

    [HttpPut("{themaId}/update")]
    public ActionResult<ProjectDto> UpdateTheme(long themaId, UpdateThemeDto updateThemeDto)
    {
        if (User.IsInRole(CustomIdentityConstants.Organization))
        {
            _unitOfWork.BeginTransaction();
            Theme theme = _projectManager.ChangeTheme(themaId, updateThemeDto.NewThemeName, updateThemeDto.ShortInformation,
                updateThemeDto.IsHeadTheme);
            _unitOfWork.Commit();
            if (theme == null)
            {
                return BadRequest("Can not update project");
            }

            return Accepted();
        }

        return Forbid();
    }

    [HttpDelete]
    public IActionResult Delete(DeleteSubThemeDto themeDto)
    {
        if (User.IsInRole(CustomIdentityConstants.Organization))
        {
            Theme theme = _projectManager.GetTheme
                (themeDto.ProjectId, themeDto.ThemeName, false);


            if (theme == null)
            {
                return NotFound();
            }

            _unitOfWork.BeginTransaction();
            _projectManager.RemoveTheme(theme, themeDto.ProjectId);
            _unitOfWork.Commit();
            return Accepted();
        }

        return Forbid();
    }
}