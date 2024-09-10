using Microsoft.AspNetCore.Mvc;
using NT.BL.Domain.flowpck;
using NT.BL.Domain.users;
using NT.BL.FlowMngr;
using NT.BL.UnitOfWorkPck;
using NT.UI.MVC.Models.Dto;
using NT.UI.MVC.Models.Dto.Flow.UpdateDto;

namespace NT.UI.MVC.Controllers.FlowPck.Api;

[Route("/api/[controller]")]
[ApiController]
public class FlowsController : ControllerBase
{
    private readonly IFlowManager _flowManager;
    private readonly UnitOfWork _unitOfWork;

    public FlowsController(IFlowManager flowManager, UnitOfWork unitOfWork)
    {
        _flowManager = flowManager;
        _unitOfWork = unitOfWork;
    }

    [HttpPut("{flowId}/update")]
    public ActionResult<FlowDto> UpdateFlow(long flowId, UpdateFlowDto updateFlowDto)
    {
        if (User.IsInRole(CustomIdentityConstants.Organization))
        {
            var flow = _flowManager
                .ChangeFlow(flowId, updateFlowDto.FlowName, updateFlowDto.State, updateFlowDto.IsLinear);
            if (flow == null)
            {
                return BadRequest("Can not update flow");
            }

            return Accepted();
        }

        return Forbid();
    }

    [HttpGet("{flowId}")]
    public IActionResult GetFlow(long flowId)
    {
        if (User.IsInRole(CustomIdentityConstants.Organization))
        {
            var flow = _flowManager.GetFlowWithStep(flowId);
            if (flow == null)
            {
                return NotFound();
            }

            var flowDto = new FlowDto
            {
                Id = flow.Id,
                FlowName = flow.FlowName,
                State = flow.State,
                IsLinear = flow.IsLinear
            };
            return Ok(flowDto);
        }

        return Forbid();
    }
    [HttpGet("FlowWithOpenRunningFlow")]
    public ActionResult<IEnumerable<RunningFlowDto>> GetFlowWithNotClosedRunningFlow()
    {
        if (User.IsInRole(CustomIdentityConstants.Organization))
        {
            var flows = _flowManager.GetFlowsWithRunningFlowNotClosed();
            if (flows == null)
            {
                return NoContent();
            }

            return Ok(flows.Select(rn =>
                new FlowDto()
                {
                    Id = rn.Id,
                    FlowName = rn.FlowName
                }));
        }

        return Forbid();
    }

    

    [HttpDelete("{flowId}")]
    public ActionResult DeleteFlow(long flowId)
    {
        if (User.IsInRole(CustomIdentityConstants.Organization))
        {
            Flow flow = _flowManager.GetFlowById(flowId);
            if (flow == null)
            {
                return NotFound();
            }

            long projectId = _flowManager.GetProjectByFlow(flowId);

            _unitOfWork.BeginTransaction();
            _flowManager.RemoveFlow(flow.Id);
            _unitOfWork.Commit();

            return Ok( new { Id = projectId });
        }

        return Forbid();
    }
}