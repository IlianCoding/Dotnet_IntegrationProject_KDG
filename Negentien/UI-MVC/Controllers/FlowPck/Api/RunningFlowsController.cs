using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NT.BL.Domain.users;
using NT.BL.SessionMngr;
using NT.BL.UserMngr;
using NT.UI.MVC.Models.Dto;
using NT.UI.MVC.Models.Dto.Flow.NewDto;

namespace NT.UI.MVC.Controllers.FlowPck.Api;

[ApiController]
[Route("/api/[controller]")]
public class RunningFlowsController : ControllerBase
{
    private readonly ISessionManager _sessionManager;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IGeneralUserManager _generalUserManager;

    public RunningFlowsController(ISessionManager sessionManager, UserManager<IdentityUser> userManager,
        IGeneralUserManager generalUserManager)
    {
        _sessionManager = sessionManager;
        _userManager = userManager;
        _generalUserManager = generalUserManager;
    }

    [HttpPost]
    public IActionResult AddNewRunningFlow(NewRunningFlowDto newRunningFlowDto)
    {
        if (User.IsInRole(CustomIdentityConstants.Attendent) || User.IsInRole(CustomIdentityConstants.Organization))
        {
            var createdRunningFlow = _sessionManager.AddRunningFlow(newRunningFlowDto.RunningFlowTime,
                newRunningFlowDto.State, newRunningFlowDto.CurrentFlowId, newRunningFlowDto.CreatedAttendantName, newRunningFlowDto.IsKiosk, false);
            if (createdRunningFlow == null)
            {
                return BadRequest("Project not registered");
            }

            return CreatedAtAction("GetRunningFlow",
                new
                {
                    id = createdRunningFlow.Id
                }, new RunningFlowDto()
                {
                    Id = createdRunningFlow.Id,
                    RunningFlowTime = createdRunningFlow.RunningFlowTime,
                    State = createdRunningFlow.State,
                    CurrentFlowId = createdRunningFlow.CurrentFlow.Id,
                    CreatedAttendantName = createdRunningFlow.CreatedAttendantName,
                    IsKiosk = createdRunningFlow.IsKiosk
                });
        }

        return Forbid();
    }


    [HttpGet("{runningFlowId}")]
    public IActionResult GetRunningFlow(long runningFlowId)
    {
        if (User.IsInRole(CustomIdentityConstants.Attendent) || User.IsInRole(CustomIdentityConstants.Organization))
        {
            var runningFlow = _sessionManager.GetRunningFlowById(runningFlowId);
            if (runningFlow == null)
            {
                return NotFound();
            }

            var runningFlowDto = new RunningFlowDto
            {
                Id = runningFlow.Id
            };
            return Ok(runningFlowDto);
        }

        return Forbid();
    }

    [HttpPut("{runningFlowId}/update")]
    public ActionResult<RunningFlowDto> UpdateRunningFlow(long runningFlowId, UpdateRunningFlowDto updateRunningFlow)
    {
        if (User.IsInRole(CustomIdentityConstants.Attendent) || User.IsInRole(CustomIdentityConstants.Organization))
        {
            var runningFlow = _sessionManager.ChangeRunningFlow(runningFlowId, updateRunningFlow.State);
            if (runningFlow == null)
            {
                return NoContent();
            }

            return Accepted();
        }

        return Forbid();
    }

    [HttpGet("GetTestingFlow")]
    public IActionResult GetTestRunningFlowFromFlow([FromQuery] string flowId)
    {
        if (User.IsInRole(CustomIdentityConstants.Attendent) || User.IsInRole(CustomIdentityConstants.Organization))
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var runningFlow = _sessionManager.GetRunningFlowByIdAndTesting(long.Parse(flowId));
            if (runningFlow == null)
            {
                return NotFound("No Testing flow could be found!");
            }

            return Ok(runningFlow.Id);
        }

        return Forbid();
    }


    [HttpGet("AttendantNamesAndCurrentFlow")]
    public ActionResult<IEnumerable<AttendantFlowsDto>> GetAllAttendantNamesByNotClosedRunningFlow()
    {
        if (User.IsInRole(CustomIdentityConstants.Attendent) || User.IsInRole(CustomIdentityConstants.Organization))
        {
            var attendantNamesAndFlows = _sessionManager.GetDistinctCreatedAttendantNamesByNotClosedRunningFlow();
            if (attendantNamesAndFlows == null || attendantNamesAndFlows.Count == 0)
            {
                return NoContent();
            }

            var result = attendantNamesAndFlows.Select(keyValuePair => new AttendantFlowsDto
            {
                AttendantName = keyValuePair.Key,
                CurrentFlowsIds = keyValuePair.Value.Select(flow => flow.Id).ToList()
            }).ToList();

            return Ok(result);
        }

        return Forbid();
    }

    [HttpGet("RunningFlows")]
    public ActionResult<IEnumerable<RunningFlowDto>> GetAllRunningFlowWithNotCloseStatus()
    {
        if (User.IsInRole(CustomIdentityConstants.Attendent) || User.IsInRole(CustomIdentityConstants.Organization))
        {
            var runningflows = _sessionManager.GetAllRunningFlowWithNotCloseStatus();
            if (runningflows == null)
            {
                return NoContent();
            }

            return Ok(runningflows.Select(rn =>
                new RunningFlowDto
                {
                    Id = rn.Id,
                    RunningFlowTime = rn.RunningFlowTime,
                    CreatedAttendantName = rn.CreatedAttendantName,
                    CurrentFlowId = rn.CurrentFlow.Id,
                    IsKiosk = rn.IsKiosk
                }));
        }

        return Forbid();
    }
}