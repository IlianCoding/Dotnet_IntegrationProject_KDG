using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using NT.BL.Domain.users;

namespace NT.BL.Domain.flowpck;

public class Session
{
    [Key]
    public long Id { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public TimeOnly ExecusionTime { get; set; }
    public RunningFlow Flow { get; set; }
    public AnonymousUser EndUser { get; set; }
    public ApplicationUser ApplicationUser { get; set; }
    public Color Color { get; set; }
    public State State { get; set; }
    public ICollection<UserAnswer> UserAnswers { get; set; }
}