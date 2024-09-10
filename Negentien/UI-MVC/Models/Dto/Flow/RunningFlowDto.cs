using NT.BL.Domain.flowpck;

namespace NT.UI.MVC.Models.Dto;

public class RunningFlowDto
{
    public string CreatedAttendantName { get; set; }
    
    public long Id { get; set; }
    public DateTime RunningFlowTime { get; set; }
    public State State { get; set; }
    public long CurrentFlowId { get; set; }
    
    public bool IsKiosk { get; set; }
}