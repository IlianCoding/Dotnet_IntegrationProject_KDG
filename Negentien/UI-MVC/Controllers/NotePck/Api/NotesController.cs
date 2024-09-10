using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using NT.BL.Domain.flowpck;
using NT.BL.Domain.sessionpck;
using NT.BL.Domain.users;
using NT.BL.FlowMngr;
using NT.BL.NoteMngr;
using NT.UI.MVC.Models.Dto;
using NT.UI.MVC.Models.Dto.Note;

namespace NT.UI.MVC.Controllers.NotePck.Api;

[Route("/api/[controller]")]
[ApiController]
public class NotesController : ControllerBase
{
    private readonly INoteManager _noteManager;
    private readonly IFlowManager _flowManager;

    public NotesController(INoteManager noteManager, IFlowManager flowManager)
    {
        _noteManager = noteManager;
        _flowManager = flowManager;
    }

    [HttpPost]
    public IActionResult AddNewNote(NewNoteDto newNoteDto)
    {
        if (User.IsInRole(CustomIdentityConstants.Attendent))
        {
            var note = _noteManager.AddNote(newNoteDto.StepId, newNoteDto.NoteTitle, newNoteDto.NoteText,
                newNoteDto.CreatedAttendantName);
            if (note is null)
            {
                return BadRequest("Note not registered");
            }

            return CreatedAtAction("GetNote", new
            {
                noteId = note.Id
            }, new NoteDto
            {
                Id = note.Id,
                StepId = note.Step.Id,
                NoteTitle = note.NoteTitle,
                NoteText = note.NoteText,
                CreatedAttendantName = note.CreatedAttendantName,
            });
        }

        return Forbid();
    }

    public IActionResult GetNote(long noteId)
    {
        if (User.IsInRole(CustomIdentityConstants.Attendent) || User.IsInRole(CustomIdentityConstants.Organization))
        {
            var note = _noteManager.GetNoteById(noteId);
            if (note is null)
            {
                return NotFound();
            }

            var notDto = new NoteDto
            {
                Id = note.Id,
                NoteTitle = note.NoteTitle,
                NoteText = note.NoteText,
                CreatedAttendantName = note.CreatedAttendantName
            };
            return Ok(notDto);
        }

        return Forbid();
    }

    [HttpPut("{noteId}/update")]
    public ActionResult<NoteDto> UpdatNote(long noteId, UpdateNoteDto updateNoteDto)
    {
        if (User.IsInRole(CustomIdentityConstants.Attendent))
        {
            var note = _noteManager.ChangeNote(noteId, updateNoteDto.NoteTitle, updateNoteDto.NoteText);
            if (note == null)
            {
                return BadRequest("Can not update note");
            }

            return Accepted();
        }

        return Forbid();
    }

    [HttpDelete("{noteId}/delete")]
    public IActionResult DeleteNote(long noteId)
    {
        if (User.IsInRole(CustomIdentityConstants.Attendent))
        {
            Note note = _noteManager.GetNoteById(noteId);
            if (note is null)
            {
                return NotFound($"Note with Id = {noteId} not found");
            }

            Flow flow = _noteManager.GetFlowByNoteId(noteId);
            _noteManager.RemoveNote(noteId);
            return Ok(new FlowDto
            {
                Id = flow.Id
            });
        }

        return Forbid();
    }
}