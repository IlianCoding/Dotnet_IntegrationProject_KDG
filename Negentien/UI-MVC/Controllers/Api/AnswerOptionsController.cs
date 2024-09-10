using Microsoft.AspNetCore.Mvc;
using NT.BL.Domain.users;
using NT.BL.FlowMngr;
using NT.BL.StepMngr;
using NT.UI.MVC.Models.Dto;

namespace NT.UI.MVC.Controllers.Api;

[ApiController]
[Route("api/[controller]")]
public class AnswerOptionsController : Controller
{
    private readonly IStepManager _stepManager;

    public AnswerOptionsController(IStepManager stepManager)
    {
        _stepManager = stepManager;
    }
    
    [HttpPut("{id}")]
    public ActionResult Change(long id, [FromBody] UpdateAnswerOptionDto answerOptionDto)
    {
        if (User.IsInRole(CustomIdentityConstants.Organization))
        {
            var conditionalPoint = _stepManager.GetConditionalPoint(answerOptionDto.ConditionalPointId);
            if (conditionalPoint == null)
            {
                return BadRequest("Can't find Conditional Point");
            }
            _stepManager.ChangeAnswerOptionCp(id, conditionalPoint);
            return Ok();
        }

        return Forbid();
    }
}