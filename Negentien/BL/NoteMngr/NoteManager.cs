using System.Collections;
using NT.BL.Domain.flowpck;
using NT.BL.Domain.sessionpck;
using NT.BL.FlowMngr;
using NT.BL.SessionMngr;
using NT.DAL.FlowRep;
using NT.DAL.StepRep.NotePck;
using NT.DAL.StepRep.StepPck;

namespace NT.BL.NoteMngr;

public class NoteManager : INoteManager
{
    private readonly INoteRepository _noteRepository;
    private readonly IStepRepository _stepRepository;
    private readonly IFlowManager _flowManager;

    public NoteManager(INoteRepository noteRepository, IStepRepository stepRepository,
        IFlowManager flowManager)
    {
        _noteRepository = noteRepository;
        _stepRepository = stepRepository;
        _flowManager = flowManager;
    }

    public Note AddNote(long stepId, string noteTitle, string noteText, string createdAttendantName)
    {
        Step step = _stepRepository.ReadStep(stepId);
        if (step is not null)
        {
            Note note = new Note
            {
                Step = step,
                NoteTitle = noteTitle,
                NoteText = noteText,
                CreatedAttendantName = createdAttendantName
            };
            _noteRepository.CreateNote(note);
            return note;
        }

        return null;
    }

    public Note GetNoteById(long noteId)
    {
        return _noteRepository.ReadNoteById(noteId);
    }

    public Note GetNoteByIdWithStep(long noteId)
    {
        return _noteRepository.ReadNoteByIdWithStep(noteId);
    }

    public Note ChangeNote(long noteId, string noteTitle, string noteText)
    {
        Note note = _noteRepository.ReadNoteById(noteId);
        note.NoteTitle = noteTitle;
        note.NoteText = noteText;
        _noteRepository.UpdateNote(note);
        return note;
    }

    public void RemoveNote(long noteId)
    {
        Note note = _noteRepository.ReadNoteById(noteId);
        _noteRepository.DeleteNote(note);
    }

    public IEnumerable<Note> GetNotesByFlowId(long flowId)
    {
        Flow flow = _flowManager.GetFlowWithStep(flowId);
        IEnumerable<Note> notes = _noteRepository.ReadNotesWithStep();

        var foundedNotes = flow.Steps
            .SelectMany(step => notes.Where(note => note.Step.Id == step.Id))
            .ToList();

        return foundedNotes;
    }

    public IEnumerable<Note> GetNotesByFlowIdAndAttendantName(long flowId, string identityName)
    {
        Flow flow = _flowManager.GetFlowWithStep(flowId);
        IEnumerable<Note> notes = _noteRepository.ReadNotesWithStep();
        var foundedNotes = flow.Steps
            .SelectMany(step =>
                notes.Where(note => note.Step.Id == step.Id).Where(note => note.CreatedAttendantName == identityName))
            .ToList();

        return foundedNotes;
    }

    public Flow GetFlowByNoteId(long noteId)
    {
        Note note = _noteRepository.ReadNoteByIdWithStep(noteId);
        if (note is not null)
        {
            Step step = note.Step;
            if (step is not null)
            {
                Flow flow = _flowManager.GetFlowByStepId(step.Id);
                return flow;
            }
        }

        return null;
    }

    public void RemoveNoteByStepId(long stepId)
    {
        IEnumerable<Note> notes = _noteRepository.ReadNotesWithStep();
        _noteRepository.DeleteNotes(notes.Where(nt => nt.Step.Id == stepId));
    }

    public IEnumerable<Note> GetAllNotesWithStep()
    {
        return _noteRepository.ReadAllNotesWithStep();
    }

    public IEnumerable<Note> GetAllNotesByFlowIdWithStep(long flowId)
    {
        Flow flow = _flowManager.GetFlowWithStep(flowId);
        IEnumerable<Step> steps = flow.Steps;
        IEnumerable<Note> notes = _noteRepository.ReadAllNotesWithStep();

        return steps
            .SelectMany(step => notes.Where(note => step.Id == note.Step.Id));
    }

}