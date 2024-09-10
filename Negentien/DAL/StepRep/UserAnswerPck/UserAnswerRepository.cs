using Microsoft.EntityFrameworkCore;
using NT.BL.Domain.flowpck;
using NT.DAL.EF;

namespace NT.DAL.StepRep.UserAnswerPck;

public class UserAnswerRepository : IUserAnswerRepository
{
    private readonly PhygitalDbContext _phygitalDbContext;

    public UserAnswerRepository(PhygitalDbContext phygitalDbContext)
    {
        _phygitalDbContext = phygitalDbContext ?? throw new ArgumentNullException(nameof(phygitalDbContext));
    }
    public UserAnswer ReadUserAnswer(long answerOptionId, long sessionId)
    {
        return _phygitalDbContext.UserAnswers
            .Include(incl => incl.Session)
            .SingleOrDefault(ua => ua.Session.Id == sessionId && ua.AnswerId == answerOptionId);
    }

    public void CreateUserAnswer(UserAnswer userAnswer)
    {
        _phygitalDbContext.UserAnswers.Add(userAnswer);
        _phygitalDbContext.SaveChanges();
    }

    public void UpdateUserAnswer(UserAnswer existingUserAnswer, long answerId, long sessionId)
    {
        UserAnswer updateUserAnswer = _phygitalDbContext.UserAnswers
            .SingleOrDefault(ex => ex.UserAnswerId == existingUserAnswer.UserAnswerId);
        
        Session session = _phygitalDbContext.Sessions
            .Find(sessionId);

        if (updateUserAnswer != null && session != null)
        {
            updateUserAnswer.AnswerId = answerId;
            updateUserAnswer.Session = session;
        }

        _phygitalDbContext.SaveChanges();
    }

    public void DeleteUserAnswers(IEnumerable<UserAnswer> userAnswers)
    {
        _phygitalDbContext.UserAnswers.RemoveRange(userAnswers);
    }
}