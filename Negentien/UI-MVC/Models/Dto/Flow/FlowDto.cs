using NT.BL.Domain.flowpck;

namespace NT.UI.MVC.Models.Dto;

public class FlowDto
{
    public long Id { get; set; }
    public State State { get; set; }
    public string FlowName { get; set; }
    public bool IsLinear { get; set; }
}