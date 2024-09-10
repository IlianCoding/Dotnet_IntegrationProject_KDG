using System.ComponentModel.DataAnnotations;

namespace NT.BL.Domain.flowpck;

public class Flow 
{
    [Key]
    public long Id { get; set; }
    public State State { get; set; }
    [Required]
    [MinLength(2, ErrorMessage = "Name has to be more than 2 letters")]
    public string FlowName { get; set; }
    public bool IsLinear { get; set; }
    public Step FirstStep { get; set; }
    public ICollection<Step> Steps { get; set; }
    public ICollection<RunningFlow> RunningFlows { get; set; }
}