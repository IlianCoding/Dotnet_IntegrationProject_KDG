using NT.BL.Domain.flowpck;

namespace NT.DAL.StepRep.UserAnswerPck;

public interface IUserAnswerRepository
{
    UserAnswer ReadUserAnswer(long answerOptionId, long sessionId);
    void CreateUserAnswer(UserAnswer userAnswer);
    void UpdateUserAnswer(UserAnswer existingUserAnswer, long answerId, long sessionId);
    void DeleteUserAnswers(IEnumerable<UserAnswer> userAnswers);
}