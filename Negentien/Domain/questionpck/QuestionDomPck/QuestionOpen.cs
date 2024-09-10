using System.ComponentModel.DataAnnotations;
using NT.BL.Domain.questionpck.AnswerDomPck;

namespace NT.BL.Domain.questionpck.QuestionDomPck;

public class QuestionOpen : Question
{
    public ICollection<AnswerOpen> AnswerOpen { get; set; }
    
}