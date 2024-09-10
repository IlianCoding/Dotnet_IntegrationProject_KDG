using Microsoft.AspNetCore.Mvc;
using NT.BL.Domain.flowpck;
using NT.BL.Domain.projectpck;
using NT.BL.Domain.users;
using NT.BL.FlowMngr;
using NT.BL.NoteMngr;
using NT.BL.ProjectMngr;
using NT.BL.SessionMngr;
using NT.BL.UnitOfWorkPck;
using NT.UI.MVC.Models;
using NT.UI.MVC.Models.UserViewModels;

namespace NT.UI.MVC.Controllers.FlowPck;

public class FlowController : Controller
{
    private readonly IFlowManager _flowManager;
    private readonly ISessionManager _sessionManager;
    private readonly INoteManager _noteManager;
    private readonly IProjectManager _projectManager;
    private readonly UnitOfWork _unitOfWork;

    public FlowController(IFlowManager flowManager, UnitOfWork unitOfWork, ISessionManager sessionManager,
        INoteManager noteManager, IProjectManager projectManager)
    {
        _flowManager = flowManager;
        _unitOfWork = unitOfWork;
        _sessionManager = sessionManager;
        _noteManager = noteManager;
        _projectManager = projectManager;
    }

    [HttpGet]
    public IActionResult FlowDetail(long id)
    {
        if (User.IsInRole(CustomIdentityConstants.Organization))
        {
            var flow = _flowManager.GetFlowWithStep(id);
            ViewBag.Project = _projectManager.GetProjectByFlowId(id);
            if (flow == null)
            {
                return NotFound();
            }
        
            if (flow.RunningFlows.Any(test => test.IsForTesting))
            {
                _unitOfWork.BeginTransaction();
                _flowManager.RemoveRunningFlowByFlowIdAndTesting(flow.Id);
                _unitOfWork.Commit();
            }

            ViewBag.Notes = _noteManager.GetNotesByFlowId(id);
            return View(flow);
        }

        return Forbid();
    }

    [HttpGet]
    public IActionResult AttendantFlowDetail(long id)
    {
        Project project = _projectManager.GetProjectByFlowId(id);
        if (User.IsInRole(CustomIdentityConstants.Attendent) || User.IsInRole(CustomIdentityConstants.Organization))
        {
            Flow flow = _flowManager.GetFlowByIdWithStepWithContentWithRunningFlow(id);
            AttendantFlowDetailViewModel attendantFlowDetailViewModel = new AttendantFlowDetailViewModel
            {
                Project = project,
                Flow = flow,
                Steps = flow.Steps,
                Contents = new List<Content>(),
                StepsCount = flow.Steps.Count
            };
            if (User.IsInRole(CustomIdentityConstants.Organization) ||
                User.IsInRole(CustomIdentityConstants.HeadOfPlatform))
            {
                attendantFlowDetailViewModel.OpenRunningFlows = _sessionManager.GetAllNotClosedRunningFlowById(flow.Id);
                attendantFlowDetailViewModel.ClosedRunningFlows = _sessionManager.GetAllClosedRunningFlowById(flow.Id);
                attendantFlowDetailViewModel.Notes = _noteManager.GetNotesByFlowId(id);
                return View(attendantFlowDetailViewModel);
            }

            if (User.Identity != null && User.Identity.Name is not null &&
                User.IsInRole(CustomIdentityConstants.Attendent))
            {
                attendantFlowDetailViewModel.OpenRunningFlows = _sessionManager.GetAllNotClosedRunningFlowByIdAndAttendantName(flow.Id, User.Identity.Name);
                attendantFlowDetailViewModel.ClosedRunningFlows =  _sessionManager.GetAllClosedRunningFlowByIdAndAttendantName(flow.Id, User.Identity.Name);
                attendantFlowDetailViewModel.Notes =_noteManager.GetNotesByFlowIdAndAttendantName(flow.Id, User.Identity.Name);
                return View(attendantFlowDetailViewModel);
            }

            return NoContent();
        }

        return Forbid();
    }

    [HttpGet]
    public IActionResult AddFlow(long projectId)
    {
        if (User.IsInRole(CustomIdentityConstants.Organization))
        {
            Project project = _projectManager.GetProjectById(projectId);
            if (project is not null)
            {
                NewFlowViewModel flowViewModel = new NewFlowViewModel()
                {
                    ProjectId = projectId,
                    IsActive = project.IsActive
                };
                ViewBag.Project = project;
                if (User.IsInRole(CustomIdentityConstants.Organization))
                {
                    return View(flowViewModel);
                }
                return Forbid();
            }
            return NoContent();
        }

        return Forbid();
    }

    [HttpPost]
    public IActionResult AddFlow(long projectId, NewFlowViewModel flowViewModel)
    {
        if (User.IsInRole(CustomIdentityConstants.Organization))
        {
            if (!ModelState.IsValid)
            {
                return View(flowViewModel);
            }

            _unitOfWork.BeginTransaction();
            var flow = _flowManager.AddFlow(flowViewModel.ProjectId, flowViewModel.State ? State.Open : State.Closed,
                flowViewModel.FlowName, flowViewModel.IsLinear);
            _unitOfWork.Commit();

            return RedirectToAction("FlowDetail", new { Id = flow.Id });
        }

        return Forbid();
    }
}