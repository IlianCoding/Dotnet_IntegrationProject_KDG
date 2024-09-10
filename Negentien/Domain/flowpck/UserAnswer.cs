using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using NT.BL.Domain.questionpck.AnswerDomPck;
using NT.BL.Domain.users;

namespace NT.BL.Domain.flowpck;

public class UserAnswer
{
    [Key] 
    public long UserAnswerId { get; set; }
    public long AnswerId { get; set; }
    public Session Session { get; set; }
    public AnswerOption AnswerOption { get; set; }
    public AnswerOpen AnswerOpen { get; set; }
    public Step Step { get; set; }
    
}