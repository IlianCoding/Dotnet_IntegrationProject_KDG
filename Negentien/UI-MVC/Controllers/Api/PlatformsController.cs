using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using NT.BL.Domain.users;
using NT.BL.PlatformMngr;
using NT.BL.UnitOfWorkPck;
using NT.UI.MVC.Models.Dto;

namespace NT.UI.MVC.Controllers.Api;

[Route("api/[controller]")]
[ApiController]
public class PlatformsController : ControllerBase
{
    private readonly IPlatformManager _platformManager;
    private readonly UnitOfWork _unitOfWork;


    public PlatformsController(IPlatformManager platformManager, UnitOfWork unitOfWork)
    {
        _platformManager = platformManager;
        _unitOfWork = unitOfWork;
    }

    [HttpPut]
    public IActionResult Put(UpdateLogoPlatform dtoLogo)
    {
        if (User.IsInRole(CustomIdentityConstants.Organization))
        {
            if (!dtoLogo.ContentType.StartsWith("image"))
            {
                return BadRequest();
            }
        
            var platform = _platformManager.GetSharingPlatform(dtoLogo.Id);

            if (platform == null)
            {
                return NotFound();
            }

            _unitOfWork.BeginTransaction();
            _platformManager.ChangeLogo(dtoLogo.ObjectName, platform);
            _unitOfWork.Commit();

            return Ok();
        }

        return Forbid();
    }

    [HttpGet("GetLogoByProjectId")]
    public IActionResult GetLogoByProjectId(long projectId)
    {
        if (User.IsInRole(CustomIdentityConstants.Organization) || User.IsInRole(CustomIdentityConstants.Attendent) )
        {
            string logoObjectName=  _platformManager.GetLogoByProjectId(projectId);

            return Ok(new { objectName =logoObjectName });
        }

        return Forbid();
    }

    [HttpGet("GetLogoByStepId")]
    public IActionResult GetLogoByStepId(long stepId)
    {
        if (User.IsInRole(CustomIdentityConstants.Organization) || User.IsInRole(CustomIdentityConstants.Attendent))
        {
            string logoObjectName=  _platformManager.GetLogoByStepId(stepId);
    
            return Ok(new { objectName =logoObjectName });
        }

        return Forbid();
    }
}