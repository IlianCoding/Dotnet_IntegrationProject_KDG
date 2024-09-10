using System.ComponentModel.DataAnnotations;

namespace NT.BL.Domain.flowpck;

public class RunningFlow
{
    [Key] 
    public long Id { get; set; }
    public DateTime RunningFlowTime { get; set; }
    public State State { get; set; }
    [Required] 
    public Flow CurrentFlow { get; set; }
    public bool IsKiosk { get; set; }
    public bool IsForTesting { get; set; }
    public string CreatedAttendantName { get; set; }
    public ICollection<Session> Sessions { get; set; }
}