using Microsoft.AspNetCore.Mvc;
using NT.BL.Domain.flowpck;
using NT.BL.Domain.sessionpck;
using NT.BL.Domain.users;
using NT.BL.FlowMngr;
using NT.BL.NoteMngr;

namespace NT.UI.MVC.Controllers.NotePck;

public class NoteController : Controller
{
    private readonly INoteManager _noteManager;
    private readonly IFlowManager _flowManager;

    public NoteController(INoteManager noteManager, IFlowManager flowManager)
    {
        _noteManager = noteManager;
        _flowManager = flowManager;
    }
    
    public IActionResult NoteDetail(long noteId)
    {
        if (User.IsInRole(CustomIdentityConstants.Attendent) || User.IsInRole(CustomIdentityConstants.Organization))
        {
            Flow flow = _noteManager.GetFlowByNoteId(noteId);
            var note = _noteManager.GetNoteByIdWithStep(noteId);
            ViewBag.Flow = flow;
            return View(note);
        }

        return Forbid();
    }
    public IActionResult AllNote(long flowId)
    {
        if (User.IsInRole(CustomIdentityConstants.Attendent) || User.IsInRole(CustomIdentityConstants.Organization))
        {
            IEnumerable<Note> notes = _noteManager.GetAllNotesByFlowIdWithStep(flowId);
            return View(notes);
        }

        return Forbid();
    }
}