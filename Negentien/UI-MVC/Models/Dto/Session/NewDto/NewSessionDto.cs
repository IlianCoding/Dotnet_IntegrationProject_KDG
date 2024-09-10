using NT.BL.Domain.flowpck;

namespace NT.UI.MVC.Models.Dto.Session.NewDto;

public class NewSessionDto
{
    public DateTime StartTime { get; set; }
    public long RunningFlowId { get; set; }
}