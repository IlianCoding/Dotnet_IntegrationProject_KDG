using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using NT.BL.Domain.flowpck;
using NT.BL.Domain.projectpck;
using NT.BL.Domain.questionpck.AnswerDomPck;
using NT.BL.Domain.questionpck.QuestionDomPck;
using NT.BL.Domain.users;
using NT.BL.Domain.validationpck;
using NT.BL.FlowMngr;
using NT.BL.ProjectMngr;
using NT.BL.StepMngr;
using NT.UI.MVC.Models.Dto;
using NT.UI.MVC.Models.Dto.Step;

namespace NT.UI.MVC.Controllers.api;

[ApiController]
[Route("api/[controller]")]
public class StepsController : ControllerBase
{
    private readonly IStepManager _stepManager;
    private readonly IFlowManager _flowManager;
    private readonly IProjectManager _projectManager;

    public StepsController(IStepManager stepManager, IFlowManager flowManager, IProjectManager projectManager)
    {
        _stepManager = stepManager;
        _flowManager = flowManager;
        _projectManager = projectManager;
    }

    [HttpGet]
    public ActionResult GetFirstStep(long flowId)
    {
        if (User.IsInRole(CustomIdentityConstants.Organization) || User.IsInRole(CustomIdentityConstants.Attendent))
        {
            Flow flow = _flowManager.GetFlowWithStep(flowId);

            if (flow == null)
            {
                return NotFound();
            }

            Step step = flow.FirstStep;
            if (step == null)
            {
                return NotFound();
            }

            return Ok(step);
        }

        return Forbid();
    }

    [HttpGet("/NextStep/{stepIdNextStep}")]
    public ActionResult GetNextStep(long stepIdNextStep)
    {
        if (User.IsInRole(CustomIdentityConstants.Organization) || User.IsInRole(CustomIdentityConstants.Attendent))
        {
            Step step = _stepManager.GetStepWithStep(stepIdNextStep);
            return Ok(step);
        }

        return Forbid();
    }


    [HttpGet("/api/Steps/Get/{stepId}")]
    public ActionResult<StepDto> Get(long stepId)
    {
        if (User.IsInRole(CustomIdentityConstants.Organization) || User.IsInRole(CustomIdentityConstants.Attendent))
        {
            Step step = _stepManager.GetStepWithStep(stepId);

            if (step == null)
            {
                return NotFound();
            }

            return new StepDto
            {
                Id = step.Id,
                Theme = step.Theme,
                Content = step.Content,
                IsConditioneel = step.IsConditioneel,
                StepState = step.StepState,
                NextStep = step.NextStep
            };
        }

        return Forbid();
    }

    [HttpGet("/api/Steps/GetStylingElements/{stepId}")]
    public ActionResult<StyleDto> GetStylingElements(long stepId)
    {
        if (User.IsInRole(CustomIdentityConstants.Organization) || User.IsInRole(CustomIdentityConstants.Attendent))
        {
            StyleDto style = new StyleDto();

            Project project = _projectManager.GetProjectByStepId(stepId);
            style.Font = project.Font;
            style.PrimaryColor = project.PrimaryColor;

            return Ok(style);
        }

        return Forbid();
    }

    [HttpPost("/api/Steps/AddQuestionWithOptions")]
    public ActionResult<StepDto> AddQuestionWithOptions([FromBody] NewQuestionWithOptionsDto newQuestionWithOptions)
    {
        if (User.IsInRole(CustomIdentityConstants.Organization) || User.IsInRole(CustomIdentityConstants.Attendent))
        {
            try
            {
                Content newQuestion = _stepManager
                    .AddQuestionWithOptions(newQuestionWithOptions.QuestionText, newQuestionWithOptions.AnswerOptions,
                        newQuestionWithOptions.QuestionType);
                return Ok(newQuestion);
            }
            catch (ValidationListException e)
            {
                return BadRequest(e.ValidationResults);
            }
        }

        return Forbid();
    }

    [HttpPost("/api/Steps/AddQuestionOpen")]
    public ActionResult<StepDto> AddQuestionOpen([FromBody] NewQuestionOpenDto newQuestionOpen)
    {
        if (User.IsInRole(CustomIdentityConstants.Organization) || User.IsInRole(CustomIdentityConstants.Attendent))
        {
            try
            {
                Content newQuestion = _stepManager.AddQuestionOpen(newQuestionOpen.QuestionText);
                return Ok(newQuestion);
            }
            catch (ValidationListException e)
            {
                return BadRequest(e.ValidationResults);
            }
        }

        return Forbid();
    }

    [HttpPost("/api/Steps/AddInformation")]
    public ActionResult<StepDto> AddInformation(NewInformationDto newInformation)
    {
        if (User.IsInRole(CustomIdentityConstants.Organization) || User.IsInRole(CustomIdentityConstants.Attendent))
        {
            if (!String.IsNullOrWhiteSpace(newInformation.ObjectName))
            {
                string contentType = newInformation.ContentType;

                if (!(contentType.StartsWith("image") || contentType.StartsWith("video")
                                                      || contentType.StartsWith("audio")))
                {
                    return BadRequest();
                }
            }


            try
            {
                Content newInfo = _stepManager.AddInformation(newInformation.Title, newInformation.TextInformation
                    , newInformation.ObjectName, newInformation.ContentType);
                return Ok(newInfo);
            }
            catch (ValidationListException e)
            {
                return BadRequest(e.ValidationResults);
            }
        }

        return Forbid();
    }

    [HttpPost("/api/Steps/AddStep")]
    public ActionResult<StepDto> AddStep([FromBody] NewStepDto step)
    {
        if (User.IsInRole(CustomIdentityConstants.Organization) || User.IsInRole(CustomIdentityConstants.Attendent))
        {
            var lastStep = _flowManager.GetLastStep(step.FlowId);
            var newStep = _stepManager.AddStep(_projectManager.GetThemeById(step.ThemeId),
                _stepManager.GetContent(step.ContentId), State.Open, step.IsConditioneel, null, step.Name);
            _stepManager.AddStepToFlow(step.FlowId, newStep.Id);
            if (!newStep.IsConditioneel)
            {
                if (lastStep == null)
                {
                    _flowManager.ChangFlowFirstStep(step.FlowId, newStep);
                }
                else
                {
                    _stepManager.ChangeStepNextStep(lastStep.Id, newStep);
                }
            }
            else
            {
                
            }

            return CreatedAtAction("Get",
                new { id = newStep.Id },
                new StepDto
                {
                    Theme = newStep.Theme,
                    IsConditioneel = newStep.IsConditioneel,
                    Content = newStep.Content,
                    StepState = newStep.StepState,
                    NextStep = newStep.NextStep,
                    Name = newStep.Name
                });
        }

        return Forbid();
    }

    [HttpPut("{stepId}/conditional")]
    public ActionResult<UpdateStepDto> UpdateConditional(long stepId, UpdateStepDto stepDto)
    {
        if (User.IsInRole(CustomIdentityConstants.Organization) || User.IsInRole(CustomIdentityConstants.Attendent))
        {
            _stepManager.ChangeStepIsConditioneel(stepId, stepDto.IsConditioneel);
            return Ok();
        }

        return Forbid();
    }

    [HttpPut("{stepId}/nextStep")]
    public ActionResult<UpdateStepDto> UpdateNextStep(long stepId, UpdateStepDto stepDto)
    {
        if (User.IsInRole(CustomIdentityConstants.Organization) || User.IsInRole(CustomIdentityConstants.Attendent))
        {
            var nextStep = _stepManager.GetStep(stepDto.NextStepId);
            if (nextStep == null)
            {
                return BadRequest();
            }

            _stepManager.ChangeStepNextStep(stepId, nextStep);
            return Ok();
        }

        return Forbid();
    }

    [HttpDelete("/api/Steps/RemoveStep/{stepId}")]
    public ActionResult DeleteStep(long stepId)
    {
        if (User.IsInRole(CustomIdentityConstants.Organization) || User.IsInRole(CustomIdentityConstants.Attendent))
        {
            _stepManager.RemoveStepAndRelocate(stepId);
            return Accepted();
        }

        return Forbid();
    }

    [HttpPut("/api/Steps/DeactivateStep/{stepId}")]
    public ActionResult DeactivateStep(long stepId)
    {
        if (User.IsInRole(CustomIdentityConstants.Organization) || User.IsInRole(CustomIdentityConstants.Attendent))
        {
            _stepManager.DeactivateStep(stepId);
            return Accepted();
        }

        return Forbid();
    }

    [HttpPut("/api/Steps/ActivateStep/{stepId}")]
    public ActionResult ActivateStep(long stepId)
    {
        if (User.IsInRole(CustomIdentityConstants.Organization) || User.IsInRole(CustomIdentityConstants.Attendent))
        {
            _stepManager.ActivateStep(stepId);
            return Accepted();
        }

        return Forbid();
    }


    [HttpPut("/api/Steps/UpdateQuestionWithOptions/{id}")]
    public ActionResult<Content> UpdateQuestionWithOptions(long id,
        [FromBody] UpdateQuestionWithOptionDto newQuestionWithOptions)
    {
        if (User.IsInRole(CustomIdentityConstants.Organization) || User.IsInRole(CustomIdentityConstants.Attendent))
        {
            Content content = _stepManager.ChangeQuestionWithOptions(id, newQuestionWithOptions.QuestionText);
            return Ok(content);
        }

        return Forbid();
    }

    [HttpPut("/api/Steps/UpdateQuestionOpen/{id}")]
    public ActionResult<StepDto> UpdateQuestionOpen(long id, [FromBody] UpdateQuestionOpenDto newQuestionOpen)
    {
        if (User.IsInRole(CustomIdentityConstants.Organization) || User.IsInRole(CustomIdentityConstants.Attendent))
        {
            Content content = _stepManager.ChangeQuestionOpen(id, newQuestionOpen.QuestionText);
            return Ok(content);
        }

        return Forbid();
    }

    [HttpPut("/api/Steps/UpdateInformation/{id}")]
    public ActionResult<StepDto> AddInformation(long id, [FromBody] UpdateInformationDto updateInformation)
    {
        if (User.IsInRole(CustomIdentityConstants.Organization) || User.IsInRole(CustomIdentityConstants.Attendent))
        {
            Content content = _stepManager.ChangeInformation(id, updateInformation.ContentType,
                updateInformation.ObjectName,
                updateInformation.TextInformation, updateInformation.Title);
            return Ok(content);
        }

        return Forbid();
    }

    [HttpPut("/api/Steps/UpdateStep/{id}")]
    public ActionResult<StepDto> AddInformation(long id, UpdateStepDto updateStepDto)
    {
        if (User.IsInRole(CustomIdentityConstants.Organization) || User.IsInRole(CustomIdentityConstants.Attendent))
        {
            Step step = _stepManager.GetStep(id);
            Theme theme = _projectManager.GetThemeById(updateStepDto.ThemeId);
            Content content = _stepManager.GetContent(updateStepDto.ContentId);
            if (content == null || step == null || theme == null)
            {
                return BadRequest();
            }

            _stepManager.ChangeStep(id, updateStepDto.Name, theme, step.IsConditioneel, content);
            return Ok();
        }

        return Forbid();
    }
}