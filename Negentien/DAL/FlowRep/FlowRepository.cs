using Microsoft.EntityFrameworkCore;
using NT.BL.Domain.flowpck;
using NT.BL.Domain.questionpck.AnswerDomPck;
using NT.BL.Domain.questionpck.QuestionDomPck;
using NT.BL.Domain.Util;
using NT.DAL.EF;

namespace NT.DAL.FlowRep;

public class FlowRepository : IFlowRepository
{
    private readonly PhygitalDbContext _phygitalDbContext;

    public FlowRepository(PhygitalDbContext phygitalDbContext)
    {
        _phygitalDbContext = phygitalDbContext ?? throw new ArgumentNullException(nameof(phygitalDbContext));
    }

    #region Reading Flows

    public Flow ReadFlowWithSteps(long flowId)
    {
        return _phygitalDbContext.Flows
            .Include(stp => stp.Steps)
            .ThenInclude(stp => stp.Content)
            .Include(stpp => stpp.Steps)
            .ThenInclude(th => th.Theme)
            .Include(stpp => stpp.Steps)
            .ThenInclude(th => th.NextStep)
            .Include(flow => flow.FirstStep)
            .Include(run => run.RunningFlows)
            .SingleOrDefault(flow => flow.Id == flowId);
    }

    public void UpdateFlow(Flow flow)
    {
        _phygitalDbContext.Flows.Update(flow);
        _phygitalDbContext.SaveChanges();
    }

    public void CreateFlow(Flow flow)
    {
        _phygitalDbContext.Flows.Add(flow);
    }

    public Flow ReadFlowWithStepWithNextStepAndContent(long flowId)
    {
        var query = _phygitalDbContext.Flows
            .Include(stpp => stpp.Steps)
            .ThenInclude(th => th.Theme)
            .Include(stpp => stpp.Steps)
            .ThenInclude(th => th.NextStep)
            .Include(fl => fl.FirstStep)
            .ThenInclude(st => st.Content)
            .Include(stpp => stpp.Steps)
            .SingleOrDefault(flow => flow.Id == flowId);

        if (query != null && query.FirstStep == null)
        {
            return _phygitalDbContext.Flows
                .Include(stpp => stpp.Steps)
                .ThenInclude(th => th.Theme)
                .Include(stpp => stpp.Steps)
                .ThenInclude(th => th.NextStep)
                .Include(flow => flow.FirstStep)
                .SingleOrDefault(flow => flow.Id == flowId);
        }

        if (query.FirstStep.Content is QuestionWithOption)
        {
            return _phygitalDbContext.Flows
                .Include(stpp => stpp.Steps)
                .ThenInclude(th => th.Theme)
                .Include(stpp => stpp.Steps)
                .ThenInclude(th => th.NextStep)
                .Include(stpp => stpp.Steps)
                .ThenInclude(th => ((QuestionWithOption)th.Content).AnswerOptions)
                .Include(flow => flow.FirstStep)
                .SingleOrDefault(flow => flow.Id == flowId);
        }
        else if (query.FirstStep.Content is QuestionOpen)
        {
            return _phygitalDbContext.Flows
                .Include(stpp => stpp.Steps)
                .ThenInclude(th => th.Theme)
                .Include(stpp => stpp.Steps)
                .ThenInclude(th => th.NextStep)
                .Include(stpp => stpp.Steps)
                .ThenInclude(th => ((QuestionOpen)th.Content).AnswerOpen)
                .Include(flow => flow.FirstStep)
                .SingleOrDefault(flow => flow.Id == flowId);
        }
        else
        {
            return _phygitalDbContext.Flows
                .Include(stpp => stpp.Steps)
                .ThenInclude(th => th.Theme)
                .Include(stpp => stpp.Steps)
                .ThenInclude(th => th.NextStep)
                .Include(stpp => stpp.Steps)
                .Include(flow => flow.FirstStep)
                .SingleOrDefault(flow => flow.Id == flowId);
        }
    }

    public Flow ReadFlowByIdWithRunningFlow(long id)
    {
        return _phygitalDbContext.Flows
            .Include(fl => fl.RunningFlows)
            .SingleOrDefault(fl => fl.Id == id);
    }

    public IEnumerable<Flow> ReadAllFlowsWithRunningFlowNotClosed()
    {
        return _phygitalDbContext.Flows
            .Include(fl => fl.RunningFlows)
            .Where(fl => fl.RunningFlows.Any(rf => rf.State != State.Closed));
    }


    public Flow ReadAllFlowsWithStepsWithContentAndRunningFlow(long flowId)
    {
        return _phygitalDbContext.Flows
            .Include(fl => fl.Steps)
            .ThenInclude(s => s.Content)
            .Include(fl => fl.RunningFlows)
            .SingleOrDefault(fl => fl.Id == flowId);
    }

    public void DeleteFlow(Flow flow)
    {
        _phygitalDbContext.Flows.Remove(flow);
        _phygitalDbContext.SaveChanges();
    }

    public long ReadProjectIdByFlow(long flowId)
    {
        return _phygitalDbContext.Projects
            .Where(p => p.Flows.Any(f => f.Id == flowId))
            .Select(p => p.Id)
            .SingleOrDefault();
    }

    public RunningFlow ReadFlowByRunningFlowId(long runningFlowId)
    {
        return _phygitalDbContext.RunningFlows
            .Include(rf => rf.CurrentFlow)
            .Single(rf => rf.Id == runningFlowId);
    }

    public void DeleteStepFromFlow(long stepId, long flowId)
    {
        Flow flow = _phygitalDbContext.Flows.Include(fl => fl.Steps)
            .SingleOrDefault(fl => fl.Id == flowId);

        if (flow != null)
            foreach (var step in flow.Steps)
            {
                if (step.Id == stepId)
                {
                    flow.Steps.Remove(step);
                }
            }
    }

    #endregion

    public Flow ReadFlowById(long id)
    {
        return _phygitalDbContext.Flows
            .SingleOrDefault(fl => fl.Id == id);
    }

    public IEnumerable<Flow> ReadAllFlowsWithSteps()
    {
        return _phygitalDbContext.Flows
            .Include(fl => fl.Steps)
            .Include(fl => fl.FirstStep);
    }

    public IEnumerable<Flow> ReadAllFlowsWithStepsWithNextStep()
    {
        return _phygitalDbContext.Flows
            .Include(fl => fl.Steps)
            .ThenInclude(st => st.NextStep)
            .Include(fl => fl.FirstStep);
    }

    public IEnumerable<AnswerOption> ReadAllUserAnswerOptionsUnderFlow(long flowId)
    {
        var userAnswerIds = _phygitalDbContext.Flows
            .Where(fl => fl.Id == flowId)
            .SelectMany(fl => fl.RunningFlows)
            .SelectMany(rf => rf.Sessions)
            .SelectMany(s => s.UserAnswers)
            .Select(ua => ua.AnswerId)
            .ToList();

        return _phygitalDbContext.AnswerOptions
            .Where(ao => userAnswerIds.Contains(ao.Id))
            .ToList();
    }

    public IEnumerable<OptionsWithQuestion> ReadAllSingleAndMultipleChoiceQuestions(long flowId)
    {
        return _phygitalDbContext.Flows.Where(fl => fl.Id == flowId)
                .SelectMany(fl => fl.Steps
                    .Where(st => st.Content is QuestionWithOption)
                    .Select(st => new OptionsWithQuestion()
                    {
                        QuestionWithOption = st.Content as QuestionWithOption,
                        AnswerOptions = (st.Content as QuestionWithOption).AnswerOptions.ToList()
                    })
                )
            ;
    }

    public IEnumerable<StatisticalSession> GetStatisticalSessions(long flowId)
    {
        return _phygitalDbContext
            .Flows
            .Where(fl => fl.Id == flowId)
            .SelectMany(fl => fl.RunningFlows
                .SelectMany(rf => rf.Sessions
                    .Select(se => new StatisticalSession()
                    {
                        StartTime = se.StartTime,
                        EndTime = se.EndTime,
                        UserAnswers = se.UserAnswers.Select(answer => new AnswerWithQuestionId()
                        {
                            QuestionId = 
                                _phygitalDbContext.Answers
                                    .SingleOrDefault(qi => qi.Id == answer.AnswerId)
                                    .QuestionId ?? -1
                            ,
                            Answer = _phygitalDbContext.AnswerOptions
                                .SingleOrDefault(a => a.Id == answer.AnswerId) != null
                                ? _phygitalDbContext.AnswerOptions
                                    .SingleOrDefault(a => a.Id == answer.AnswerId).TextAnswer
                                : _phygitalDbContext.AnswerOpen
                                    .SingleOrDefault(a => a.Id == answer.AnswerId).Answer
                        }).ToList()
                    })));
    }

    public IEnumerable<StatisticalQuestion> GetQuestionsOfFlow(long flowId)
    {
        var kale = _phygitalDbContext
            .Flows
            .Where(fl => fl.Id == flowId)
            .SelectMany(fl =>
                fl.Steps
                    .Where(st => st.Content is Question)
                    .Select(st => new StatisticalQuestion()
                    {
                        QuestionId = st.Content.Id,
                        QuestionText = _phygitalDbContext.Questions
                            .Where(qu => qu.Id == st.Content.Id)
                            .Select(que => que.QuestionText).SingleOrDefault()
                    })
            );

        return kale;
    }
}