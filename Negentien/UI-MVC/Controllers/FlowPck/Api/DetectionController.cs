using Microsoft.AspNetCore.Mvc;
using NT.BL.Domain.flowpck;
using NT.BL.Domain.users;
using NT.BL.FlowMngr;
using NT.BL.RecognitionMngr;
using NT.BL.SessionMngr;
using NT.BL.StepMngr;
using NT.UI.MVC.Models.Dto.Flow.NewDto;
using NT.UI.MVC.Models.Dto.Step;
using Color = NT.BL.Domain.users.Color;

namespace NT.UI.MVC.Controllers.FlowPck.Api;

[ApiController]
[Route("api/[controller]")]
public class DetectionController : Controller
{
    private readonly ColorDetection _colorDetection;
    private readonly ISessionManager _sessionManager;
    private readonly IStepManager _stepManager;
    private readonly IFlowManager _flowManager;

    public DetectionController(ColorDetection colorDetection, ISessionManager sessionManager, IStepManager stepManager, IFlowManager flowManager)
    {
        _colorDetection = colorDetection;
        _sessionManager = sessionManager;
        _stepManager = stepManager;
        _flowManager = flowManager;
    }

    [HttpGet("GetState")]
    public IActionResult GetState([FromQuery] long runningFlowId)
    {
        if (User.IsInRole(CustomIdentityConstants.Attendent) || User.IsInRole(CustomIdentityConstants.Organization))
        {
            RunningFlow runningFlow = _flowManager.GetRunningFlowById(runningFlowId);

            if (runningFlow == null)
            {
                return NotFound("RunningFlow couldn't be found!");
            }

            return Ok(new { state = (int)runningFlow.State });
        }

        return Forbid();
    }

    [HttpPut("CloseSessions")]
    public IActionResult CloseAllActiveSessions([FromQuery] string runningFlowId)
    {
        if (User.IsInRole(CustomIdentityConstants.Attendent) || User.IsInRole(CustomIdentityConstants.Organization))
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Runningflow parameter is incorrect!");
            }
        
            RunningFlow runningFlow = _flowManager.GetRunningFlowById(long.Parse(runningFlowId));

            if (runningFlow != null)
            {
                _sessionManager.ChangeAllActiveSessionsOfRunningFlow(long.Parse(runningFlowId));
                return Ok("All active sessions got closed!");
            }
        
            return NotFound("RunningFlow couldn't be found!");
        }

        return Forbid();
    }

    [HttpPost("ReceivingChoiceAnswer")]
    public IActionResult ReceivingChoiceAnswer([FromForm] IFormFile image, [FromForm] string answerInput, [FromForm] string runningFlowId)
    {
        if (image == null || string.IsNullOrEmpty(runningFlowId))
        {
            return BadRequest("Parameters are incorrect.");
        }
        
        using (var stream = image.OpenReadStream())
        {
            using (var memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                var imageData = memoryStream.ToArray();
                Color color = _colorDetection.ImageProcessing(imageData);

                Session currentSession = _sessionManager.GetSessionByColorAndRunningFlow(color, long.Parse(runningFlowId));
                _stepManager.AddUserAnswer(long.Parse(answerInput), currentSession.Id, false);
                return Ok("Het petje werd juist gededecteerd");
            }
        }
    }
    
    [HttpPost("ReceivingOpenAnswer")]
    public IActionResult ReceivingOpenAnswer([FromBody] NewOpenAnswerDto newOpenAnswerDto)
    {
        if (string.IsNullOrEmpty(newOpenAnswerDto.AnswerInput) || string.IsNullOrEmpty(newOpenAnswerDto.RunningFlowId))
        {
            return BadRequest("Parameters are incorrect.");
        }

        Color color = (Color)newOpenAnswerDto.Color;

        Session currentSession = _sessionManager.GetSessionByColorAndRunningFlow(color, long.Parse(newOpenAnswerDto.RunningFlowId));
        _stepManager.AddUserOpenAnswer(currentSession, newOpenAnswerDto.AnswerInput, long.Parse(newOpenAnswerDto.QuestionId));
        return Ok("Het open antwoord werd correct ingevoerd!");
    }

    [HttpPost("StartingSession")]
    public IActionResult StartingSession([FromForm] StartingSessionDto startingSessionDto)
    {
        if (startingSessionDto.Image == null || string.IsNullOrEmpty(startingSessionDto.RunningFlowId))
        {
            return BadRequest("Parameters are incorrect.");
        }
        
        using (var stream = startingSessionDto.Image.OpenReadStream())
        {
            using (var memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                var imageData = memoryStream.ToArray();
                Color color = _colorDetection.ImageProcessing(imageData);
                
                _sessionManager.AddSession(long.Parse(startingSessionDto.RunningFlowId), color);
                return Ok("Het petje werd juist gededecteerd en de nieuwe sessie werd aangemaakt!");
            }
        }
    }
}