using System.ComponentModel.DataAnnotations;

namespace NT.BL.Domain.questionpck.AnswerDomPck;

public abstract class Answer
{
    [Key]
    public long Id { get; set; }
    public long? QuestionId { get; set; }
}