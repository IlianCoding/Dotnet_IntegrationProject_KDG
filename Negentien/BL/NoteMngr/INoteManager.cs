using System.Collections;
using NT.BL.Domain.flowpck;
using NT.BL.Domain.sessionpck;

namespace NT.BL.NoteMngr;

public interface INoteManager
{
    Note AddNote(long stepId,string noteTitle,string noteText, string createdAttendantName);
    Note GetNoteById(long noteId);
    Note GetNoteByIdWithStep(long noteId);
    Note ChangeNote(long noteId, string noteTitle, string noteText);
    void RemoveNote(long noteId);
    IEnumerable<Note> GetNotesByFlowId(long flowId);
    IEnumerable<Note> GetNotesByFlowIdAndAttendantName(long flowId, string identityName);
    Flow GetFlowByNoteId(long noteId);
    void RemoveNoteByStepId(long stepId);
    IEnumerable<Note> GetAllNotesByFlowIdWithStep(long flowId);
}