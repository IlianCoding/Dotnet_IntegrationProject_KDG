using NT.BL.Domain.flowpck;

namespace NT.UI.MVC.Models;

public class SurveyViewModel
{
    public Step Step { get; set; }
    public bool IsKiosk { get; set; }
    public bool IsLinear { get; set; }
    public long RunningFlowId { get; set; }
    
}