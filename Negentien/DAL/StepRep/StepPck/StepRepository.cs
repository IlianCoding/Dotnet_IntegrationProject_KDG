using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using NT.BL.Domain.flowpck;
using NT.BL.Domain.questionpck.AnswerDomPck;
using NT.BL.Domain.questionpck.QuestionDomPck;
using NT.DAL.EF;

namespace NT.DAL.StepRep.StepPck;

public class StepRepository : IStepRepository
{
    private readonly PhygitalDbContext _phygitalDbContext;

    public StepRepository(PhygitalDbContext phygitalDbContext)
    {
        _phygitalDbContext = phygitalDbContext ?? throw new ArgumentNullException(nameof(phygitalDbContext));
    }

    public Step ReadStep(long stepId)
    {
        return _phygitalDbContext.Steps
            .Include(s => s.Content)
            .Include(s => s.Theme)
            .Include(s => s.NextStep)
            .SingleOrDefault(s => s.Id == stepId);
    }

    public Step ReadStepWithQuestionOptions(long stepId)
    {
        return _phygitalDbContext.Steps
            .Include(s => s.Content)
            .ThenInclude(q => (q as QuestionWithOption).AnswerOptions)
            .ThenInclude(ao=> ao.ConditionalPoint)
            .ThenInclude(cp=> cp.ConditionalStep)
            .Include(s => s.NextStep)
            .SingleOrDefault(s => s.Id == stepId);
    }

    public Step ReadStepWithQuestionOpen(long stepId)
    {
        return _phygitalDbContext.Steps
            .Include(s => s.Content)
            .ThenInclude(q => (q as QuestionOpen).AnswerOpen)
            .Include(s => s.NextStep)
            .SingleOrDefault(s => s.Id == stepId);
    }

    public Step ReadStepWithStep(long stepId)
    {
        return _phygitalDbContext.Steps
            .Include(step => step.NextStep)
            .SingleOrDefault(step => step.Id == stepId);
    }

    public IEnumerable<Step> ReadAllConditionalSteps()
    {
        return _phygitalDbContext.Steps
            .Where(step => step.IsConditioneel == true);
    }

    public void CreateStep(Step step)
    {
        _phygitalDbContext.Steps.Add(step);
        _phygitalDbContext.SaveChanges();
    }

    public ConditionalPoint ReadConditionalPoint(long id)
    {
        return _phygitalDbContext.ConditionalPoints
            .SingleOrDefault(cp => cp.Id == id);
    }

    public Content ReadContent(long stepContentId)
    {
        return _phygitalDbContext.Content
            .SingleOrDefault(c => c.Id == stepContentId);
    }

    public void CreateStepToFlow(Flow flow, Step step)
    {
        if (flow == null)
        {
            throw new ArgumentNullException(nameof(flow));
        }

        if (step == null)
        {
            throw new ArgumentNullException(nameof(step));
        }

        if (flow.Steps == null)
        {
            flow.Steps = new List<Step>();
        }

        flow.Steps.Add(step);
        _phygitalDbContext.SaveChanges();
    }

    public IEnumerable<Step> ReadStepsAttachedToTheme(Theme theme)
    {
        return _phygitalDbContext
            .Steps
            .Where(step => step.Theme.Id == theme.Id).AsEnumerable();
    }

    public void DeleteStep(long stepId)
    {
        Step step = _phygitalDbContext.Steps.Single(st => stepId == st.Id);
        step.NextStep = null;
        _phygitalDbContext.SaveChanges();
        _phygitalDbContext.Steps.Remove(step);
        _phygitalDbContext.SaveChanges();
    }

    public void UpdateStep(Step step)
    {
        _phygitalDbContext.Steps.Update(step);
        _phygitalDbContext.SaveChanges();
    }

    public ConditionalPoint UpdateConditionalPoint(ConditionalPoint conditionalPoint)
    {
        _phygitalDbContext.ConditionalPoints.Update(conditionalPoint);
        _phygitalDbContext.SaveChanges();
        return _phygitalDbContext.ConditionalPoints.Single(cp => cp.Id == conditionalPoint.Id);
    }
    
    public void UpdateAnswerOption(AnswerOption changedAnswerOption)
    {
        _phygitalDbContext.AnswerOptions.Update(changedAnswerOption);
        _phygitalDbContext.SaveChanges();
    }

    public Step ReadStepWithStepContentThemeAndState(long stepId)
    {
        Step step = _phygitalDbContext.Steps.Include(st => st.NextStep)
            .Include(st => st.Content)
            .Include((st => st.Theme))
            .Single(st => st.Id == stepId);

        if (step.Content is QuestionWithOption)
        {
            return _phygitalDbContext.Steps.Include(st => st.NextStep)
                .Include(st => st.Content)
                .ThenInclude(co => ((QuestionWithOption)co).AnswerOptions)
                .ThenInclude(ao=>ao.ConditionalPoint).
                ThenInclude(cp=> cp.ConditionalStep)
                .Include((st => st.Theme)).
                ThenInclude(th=>th.Subthemes)
                .Single(st => st.Id == stepId);
        }else if (step.Content is QuestionOpen)
        {
            return _phygitalDbContext.Steps.Include(st => st.NextStep)
                .Include(st => st.Content)
                .ThenInclude(co=> ((QuestionOpen)co).AnswerOpen)
                .Include((st=>st.Theme))
                .ThenInclude(th=>th.Subthemes)
                .Single(st => st.Id == stepId);
        }
        else if (step.Content is InformationContent)
        {
            return _phygitalDbContext.Steps.Include(st => st.NextStep)
                .ThenInclude(ns => ns.Content)
                .Include(st => st.Content)
                .Include(st=>st.Theme)
                .ThenInclude(th=>th.Subthemes)
                .Single(st => st.Id == stepId);
        }

        return null;
    }

    public IEnumerable<Step> ReadAllStepsWithNextStep()
    {
        return _phygitalDbContext.Steps.Include(st => st.NextStep);
    }

    public Step ReadStepWithContentAndTheme(long stepId)
    {
        Step step = _phygitalDbContext.Steps
            .Include(st => st.Content)
            .Include((st => st.Theme))
            .Single(st => st.Id == stepId);

        if (step.Content is QuestionWithOption)
        {
            return _phygitalDbContext.Steps
                .Include(st => st.Content)
                .ThenInclude(co => ((QuestionWithOption)co).AnswerOptions)
                .ThenInclude(ao=>ao.ConditionalPoint).ThenInclude(cp=>cp.ConditionalStep)
                .Include((st => st.Theme))
                .Single(st => st.Id == stepId);
        }
        else
        {
            return _phygitalDbContext.Steps
                .Include(st => st.Content)
                .Single(st => st.Id == stepId);
        }

        return null;
    }

    public void DeleteConditionalPoint(long id)
    {
       
        ConditionalPoint conditionalPoint = _phygitalDbContext.ConditionalPoints
            .Include(cp=>cp.ConditionalStep)
            .ThenInclude(cs=>cs.NextStep)
            .Single(cp => cp.Id == id);
        
        AnswerOption answerOption = _phygitalDbContext.AnswerOptions
            .Single(ao => ao.ConditionalPoint == conditionalPoint);
        
        answerOption.ConditionalPoint = null;
        Step step = conditionalPoint.ConditionalStep;
        step.NextStep = null;
        UpdateStep(step);
        UpdateAnswerOption(answerOption);
        
        _phygitalDbContext.ConditionalPoints.Remove(conditionalPoint);
        _phygitalDbContext.SaveChanges();
    }

    public void DeleteAnswerFromQuestionWithOption(long answerId, long questionId)
    {
        QuestionWithOption questionWithOption = _phygitalDbContext.QuestionWithOptions
            .Include(qo=>qo.AnswerOptions)
            .Single(qo =>qo.Id == questionId);
        
        foreach (var answer in questionWithOption.AnswerOptions)
        {
            if (answer.Id == answerId)
            {
                questionWithOption.AnswerOptions.Remove(answer);
            }
        }
    }

    public void DeleteAnswerOption(long answerid)
    {
        AnswerOption answerOption = _phygitalDbContext.AnswerOptions
            .Single(ao => ao.Id == answerid);
        
        _phygitalDbContext.AnswerOptions.Remove(answerOption);
        _phygitalDbContext.Answers.Remove(answerOption);
    }

    public void DeleteAnswerFromQuestionOpen(long answerId, long questionId)
    {
        QuestionOpen questionOpen = _phygitalDbContext.OpenQuestions
            .Include(qo=>qo.AnswerOpen)
            .Single(qo =>qo.Id == questionId);
        
        foreach (var answer in questionOpen.AnswerOpen)
        {
            if (answer.Id == answerId)
            {
                questionOpen.AnswerOpen.Remove(answer);
            }
        }
    }
    public void DeleteAnswerOpen(long answerId)
    {
        AnswerOpen answerOpen = _phygitalDbContext.AnswerOpen
            .Single(ao => ao.Id == answerId);
        
        _phygitalDbContext.AnswerOpen.Remove(answerOpen);
        _phygitalDbContext.Answers.Remove(answerOpen);
    }

    public void DeleteQuestionOpen(long questionOpenId)
    {
        QuestionOpen questionOpen = _phygitalDbContext.OpenQuestions
            .Include(qo=>qo.AnswerOpen)
            .Single(qo => qo.Id == questionOpenId);
        
        foreach (var answer in questionOpen.AnswerOpen)
        {
            DeleteAnswerFromQuestionOpen(answer.Id, questionOpenId);
            DeleteAnswerOpen(answer.Id);
        }

        _phygitalDbContext.Questions.Remove(questionOpen);
        _phygitalDbContext.OpenQuestions.Remove(questionOpen);
        _phygitalDbContext.SaveChanges();
    }

    public ConditionalPoint ReadConditionalPointByStepid(long stepId)
    {
        return _phygitalDbContext.ConditionalPoints
            .SingleOrDefault(cp => cp.ConditionalStep.Id == stepId);
    }

    public void DeleteInformationContent(long informationContentId)
    {
        InformationContent info = _phygitalDbContext.InformationContent
            .Single(info =>info.Id==informationContentId);
        
        _phygitalDbContext.InformationContent.Remove(info);
    }

    public ConditionalPoint ReadConditionalPointWithConditionalStep(long cpId)
    {
        return _phygitalDbContext.ConditionalPoints.Include(cp=>cp.ConditionalStep)
            .ThenInclude(cp=>cp.NextStep)
            .Single(cp => cp.Id == cpId);
    }

    public QuestionWithOption ReadQuestionWithOption(long id)
    {
        return _phygitalDbContext.QuestionWithOptions
            .Include(qo=> qo.AnswerOptions)
            .ThenInclude(ao=>ao.ConditionalPoint)
            .ThenInclude(co=>co.ConditionalStep)
            .Single(qo => qo.Id == id);
    }
    public void DeleteQuestionWithOption(long id)
    {
        QuestionWithOption questionWithOption = ReadQuestionWithOption(id);
        foreach (var answeroption in questionWithOption.AnswerOptions)
        {
            if (answeroption.ConditionalPoint != null)
            {
                answeroption.ConditionalPoint.ConditionalStep = null;
                UpdateConditionalPoint(answeroption.ConditionalPoint);
                DeleteConditionalPoint(answeroption.ConditionalPoint.Id);
            }

            DeleteAnswerFromQuestionWithOption(answeroption.Id, questionWithOption.Id);
            DeleteAnswerOption(answeroption.Id);
        }

        _phygitalDbContext.QuestionWithOptions.Remove(questionWithOption);
        _phygitalDbContext.Questions.Remove(questionWithOption);
        _phygitalDbContext.SaveChanges();
    }
}