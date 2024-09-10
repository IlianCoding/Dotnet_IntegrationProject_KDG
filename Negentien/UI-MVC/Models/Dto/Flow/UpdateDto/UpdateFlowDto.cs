using NT.BL.Domain.flowpck;

namespace NT.UI.MVC.Models.Dto.Flow.UpdateDto;

public class UpdateFlowDto
{
    public long Id { get; set; }
    public State State { get; set; }
    public string FlowName { get; set; }
    public bool IsLinear { get; set; }
}