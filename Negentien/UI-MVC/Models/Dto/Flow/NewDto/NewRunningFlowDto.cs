using NT.BL.Domain.flowpck;

namespace NT.UI.MVC.Models.Dto.Flow.NewDto;

public class NewRunningFlowDto
{
    
    public string CreatedAttendantName { get; set; }
    public DateTime RunningFlowTime { get; set; }
    public State State { get; set; }
    public long CurrentFlowId { get; set; }
    
    public bool IsKiosk { get; set; }
}