#nullable enable
using NT.BL.Domain.flowpck;

namespace NT.UI.MVC.Models.Dto.Step;

public class StepDto
{
    public long Id { get; set; }
    public Theme Theme { get; set; }
    public State StepState { get; set; }
    public Content Content { get; set; }
    public bool IsConditioneel { get; set; }
    
    public BL.Domain.flowpck.Step NextStep { get; set; }
    public string Name { get; set; }
}