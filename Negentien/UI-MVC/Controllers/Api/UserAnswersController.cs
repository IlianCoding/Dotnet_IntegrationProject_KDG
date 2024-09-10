using Microsoft.AspNetCore.Mvc;
using NT.BL.Domain.flowpck;
using NT.BL.SessionMngr;
using NT.BL.StepMngr;
using NT.UI.MVC.Models.Dto.Step;
using NT.UI.MVC.Models.Dto.Step.NewDto;

namespace NT.UI.MVC.Controllers.api;

[ApiController]
[Route("api/[controller]")]
public class UserAnswersController : ControllerBase
{
    private readonly IStepManager _stepManager;
    private readonly ISessionManager _sessionManager;

    public UserAnswersController(IStepManager stepManager, ISessionManager sessionManager)
    {
        _stepManager = stepManager;
        _sessionManager = sessionManager;
    }

    [HttpGet]
    public IActionResult Get(long answerId, Session session)
    {
        UserAnswer userAnswer = _stepManager.GetUserAnswer(answerId, session);

        if (userAnswer == null)
        {
            return NoContent();
        }

        UserAnswerDto userAnswerDto = new UserAnswerDto()
        {
            AnswerId = userAnswer.AnswerId,
            Session = userAnswer.Session,
            AnswerOpen = userAnswer.AnswerOpen,
            AnswerOption = userAnswer.AnswerOption
        };

        return Ok(userAnswerDto);
    }


    [HttpPost]
    public IActionResult SubmitAnswer([FromBody] NewUserAnswerDto newUserAnswerDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        Session userSession =
            _sessionManager.GetSessionByStateAndRunningFlow(State.Open, long.Parse(newUserAnswerDto.RunningFlowId));

        if (newUserAnswerDto.AnswerId == null)
        {
            if (newUserAnswerDto.QuestionId != null)
                _stepManager.AddUserOpenAnswer(userSession,
                    newUserAnswerDto.AnswerString, long.Parse(newUserAnswerDto.QuestionId));
        }
        else
        {
            _stepManager.AddUserAnswer((long)newUserAnswerDto.AnswerId , userSession.Id, false);
        }
        return Ok("Answer got submitted");
    }
}