using Microsoft.AspNetCore.Mvc;
using NT.BL.Domain.users;
using NT.BL.Domain.Util;
using NT.BL.FlowMngr;
using NT.BL.PlatformMngr;
using NT.BL.ProjectMngr;
using NT.BL.services;

namespace NT.UI.MVC.Controllers.Api;

[Route("api/[controller]")]
[ApiController]
public class StatisticsController : ControllerBase
{
    private readonly StatisticsService _statisticsService;
    private readonly CsvService _csvService;
    private readonly IPlatformManager _platformManager;
    private readonly IProjectManager _projectManager;
    private readonly IFlowManager _flowManager;


    public StatisticsController(StatisticsService statisticsService, IPlatformManager platformManager
    , IProjectManager projectManager, IFlowManager flowManager, CsvService csvService)
    {
        _statisticsService = statisticsService;
        _platformManager = platformManager;
        _projectManager = projectManager;
        _flowManager = flowManager;
        _csvService = csvService;
    }

    [HttpGet("ProjectDataPerSharingPlatform")]
    public IActionResult GetProjectDataPerSharingPlatform()
    {
        if (User.IsInRole(CustomIdentityConstants.HeadOfPlatform))
        {
            var data =
                _statisticsService.GetTotalAndActiveProjectCountPerSharingPlatform();

            return Ok(data);
        }

        return Forbid();
    }

    [HttpGet("AvgFlowPerProjectPerSharingPlatform")]
    public IActionResult GetAvgFlowPerProjectPerSharingPlatform()
    {
        if (User.IsInRole(CustomIdentityConstants.HeadOfPlatform))
        {
            var data =
                _statisticsService.GetAvgFlowPerProjectPerSharingPlatform();

            return Ok(data);
        }

        return Forbid();
    }

    [HttpGet("ParticipantsPerFlow/{platformId}")]
    public IActionResult GetParticipantsPerFlow(long platformId)
    {
        if (User.IsInRole(CustomIdentityConstants.Organization))
        {
            if (_platformManager.GetSharingPlatformOnly(platformId) == null)
            {
                return NotFound();
            }
      
            IEnumerable<ParticipantsPerFlow> data =
                _statisticsService.GetParticipantsPerFlow(platformId);
        
            return Ok(data);
        }

        return Forbid();
    }
    
    
    [HttpGet("RunningFlowDataOfPlatform/{platformId}")]
    public IActionResult GetRunningFlowDataOfPlatform(long platformId,int year)
    {
        if (User.IsInRole(CustomIdentityConstants.Organization))
        {
            if (_platformManager.GetSharingPlatformOnly(platformId) == null)
            {
                return NotFound();
            }
      
            IEnumerable<RunningFlowsPerFlow> data =
                _statisticsService.GetRunningFlowDataOfPlatform(platformId,year);
        
            return Ok(data);
        }

        return Forbid();
    }
    
    [HttpGet("ThemesAndAmountOfStepsAttachedToIt/{projectId}")]
    public IActionResult GetThemesAndAmountOfStepsAttachedToIt(long projectId)
    {
        if (User.IsInRole(CustomIdentityConstants.Organization))
        {
            if (_projectManager.GetProjectById(projectId) == null)
            {
                return NotFound();
            }
      
            IEnumerable<ThemeAndAmountOfStepsAttached> data =
                _statisticsService.GetThemesAndAmountOfStepsAttachedToIt(projectId);
        
            return Ok(data);
        }

        return Forbid();
    }
    
    
    [HttpGet("MostPopularAnswerPerQuestionPerFlow/{flowId}")]
    public IActionResult GetMostPopularAnswerPerQuestion(long flowId)
    {
        if (User.IsInRole(CustomIdentityConstants.Organization))
        {
            if (_flowManager.GetFlowById(flowId) == null)
            {
                return NotFound();
            }
      
            IEnumerable<QuestionWithMostChosenAnswer> data =
                _statisticsService.GetMostPopularAnswerPerQuestion(flowId);
        
            return Ok(data);
        }

        return Forbid();
    }
    
    [HttpGet("AllAnswersInCsvFormat/{flowId}")]
    public IActionResult GetAllAnswersInCsvFormat(long flowId)
    {

        if (User.IsInRole(CustomIdentityConstants.Organization))
        {
            if (_flowManager.GetFlowById(flowId) == null)
            {
                return NotFound();
            }
      
            string data =
                _csvService.GetAllAnswers(flowId);

            if (data == null)
            {
                return NoContent();
            }
            
            return Ok(new {data = data});
        }

        return Forbid();
    }
}