using Microsoft.EntityFrameworkCore;
using NT.BL.Domain.flowpck;
using NT.BL.Domain.questionpck.AnswerDomPck;
using NT.BL.Domain.questionpck.QuestionDomPck;
using NT.DAL.EF;

namespace NT.DAL.StepRep.AnswerPck;

public class AnswerRepository : IAnswerRepository
{
    private readonly PhygitalDbContext _phygitalDbContext;

    public AnswerRepository(PhygitalDbContext phygitalDbContext)
    {
        _phygitalDbContext = phygitalDbContext ?? throw new ArgumentNullException(nameof(phygitalDbContext));
    }
    public Answer ReadAnswer(long answerId)
    {
        return _phygitalDbContext.Answers
            .Find(answerId);
    }

    public AnswerOption ReadAnswerOption(long answerId)
    {
        return _phygitalDbContext.AnswerOptions
            .Find(answerId);
    }

    public Answer CreateAnswerOpen(AnswerOpen answerOpen)
    {
        _phygitalDbContext.Answers.Add(answerOpen);
        
        _phygitalDbContext.SaveChanges();
        return answerOpen;
    }

    public void UpdateAnswerOptionConditionalPoint(AnswerOption answerOption)
    {
        _phygitalDbContext.AnswerOptions.Update(answerOption);
    }

    public AnswerOption CreateAnswerOption(AnswerOption answerOption)
    {
        _phygitalDbContext.AnswerOptions.Add(answerOption);
        _phygitalDbContext.SaveChanges();
        return answerOption;
    }
    public AnswerOption ReadAnswerOptionWithConditionalPoint(long answerOptionId)
    {
            var answerWithCp =_phygitalDbContext.AnswerOptions
                .Include((an => an.ConditionalPoint))
                .ThenInclude(cp=>cp.ConditionalStep)
                .ThenInclude(cs=>(cs.Content))
                .Single(an => an.Id == answerOptionId);
            if (answerWithCp.ConditionalPoint!=null && answerWithCp.ConditionalPoint.ConditionalStep.Content is QuestionWithOption)
            {
                return _phygitalDbContext.AnswerOptions
                    .Include((an => an.ConditionalPoint))
                    .ThenInclude(cp=>cp.ConditionalStep)
                    .ThenInclude(cs=>(cs.Content))
                    .ThenInclude(co=>((QuestionWithOption)co).AnswerOptions)
                    .Single(an => an.Id == answerOptionId);
            } else if (answerWithCp.ConditionalPoint!=null &&answerWithCp.ConditionalPoint.ConditionalStep.Content is QuestionOpen)
            {
                return _phygitalDbContext.AnswerOptions
                    .Include((an => an.ConditionalPoint))
                    .ThenInclude(cp=>cp.ConditionalStep)
                    .ThenInclude(cs=>(cs.Content))
                    .ThenInclude(co=>((QuestionOpen)co).AnswerOpen)
                    .Single(an => an.Id == answerOptionId);
            }
            else
            {
                return answerWithCp;
            }

    }

    public IEnumerable<UserAnswer> ReadAllUserAnswersWithStep()
    {
        return _phygitalDbContext.UserAnswers
            .Include(ua => ua.Step);
    }
}