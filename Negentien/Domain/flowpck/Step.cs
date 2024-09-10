using System.ComponentModel.DataAnnotations;

namespace NT.BL.Domain.flowpck;

public class Step
{
    [Key]
    public long Id { get; set; }
    // [Required]
    public string Name { get; set; }
    [Required]
    public Theme Theme { get; set; }
    public Content Content { get; set; }
    public State StepState { get; set; }
    [Required]
    public bool IsConditioneel { get; set; }
    public Step NextStep { get; set; }
}