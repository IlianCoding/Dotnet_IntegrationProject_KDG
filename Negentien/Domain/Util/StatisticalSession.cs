namespace NT.BL.Domain.Util;

public class StatisticalSession
{
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public ICollection<AnswerWithQuestionId> UserAnswers { get; set; }
}