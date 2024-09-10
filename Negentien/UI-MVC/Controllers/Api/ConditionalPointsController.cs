using Microsoft.AspNetCore.Mvc;
using NT.BL.FlowMngr;
using NT.BL.StepMngr;
using NT.UI.MVC.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using NT.BL.Domain.flowpck;
using NT.BL.Domain.users;

namespace NT.UI.MVC.Controllers.Api;

[ApiController]
[Route("api/[controller]")]
public class ConditionalPointsController : Controller
{
    private readonly IStepManager _stepManager;
    private readonly IFlowManager _flowManager;

    public ConditionalPointsController(IStepManager stepManager, IFlowManager flowManager)
    {
        _stepManager = stepManager;
        _flowManager = flowManager;
    }

    [HttpGet("{id}")]
    public ActionResult<ConditionalPointDto> GetCP(long id)
    {
        if (User.IsInRole(CustomIdentityConstants.Organization))
        {
            ConditionalPoint conditionalPoint = _stepManager.GetConditionalPoint(id);
            if (conditionalPoint == null)
            {
                return NotFound(); 
            }
        
            return new ConditionalPointDto
            {
                Id = conditionalPoint.Id,
                ConditionalStep = conditionalPoint.ConditionalStep,
                ConditionalPointName = conditionalPoint.ConditionalPointName,
            };
        }

        return Forbid();
    }
    
    
    [HttpPost]
    public ActionResult<ConditionalPointDto> Add (NewConditionalPointDto conditionalPointDto)
    {
        if (User.IsInRole(CustomIdentityConstants.Organization))
        {
            var step = _stepManager.GetStep(conditionalPointDto.StepId);
            var newConditionalPoint = _stepManager.AddConditionalPoint(conditionalPointDto.ConditionalPointName, step);
            return CreatedAtAction("GetCP",
                new { id = newConditionalPoint.Id },
                new ConditionalPointDto
                {
                    ConditionalStep = newConditionalPoint.ConditionalStep,
                    ConditionalPointName = newConditionalPoint.ConditionalPointName
                });
        }

        return Forbid();
    }

    [HttpDelete("RemoveCP/{conditionalPointId}")]

    public IActionResult RemoveConditionalPoint(long conditionalPointId)
    {
        if (User.IsInRole(CustomIdentityConstants.Organization))
        {
            _stepManager.RemoveConditionalPoint(conditionalPointId);
            return Accepted();
        }

        return Forbid();
    }

    [HttpPut("Update/{id}")]

    public IActionResult UpdateConditionalPoint(long id, UpdateConditionalPointDto updateConditionalPointDto)
    {
        if (User.IsInRole(CustomIdentityConstants.Organization))
        {
            Step step = _stepManager.GetStepWithStepContentThemeAndState(updateConditionalPointDto.StepId);
            ConditionalPoint updated = _stepManager.ChangeConditionalPoint(id, updateConditionalPointDto.ConditionalPointName,
                step);
            return Ok(updated);
        }

        return Forbid();
    }
}
