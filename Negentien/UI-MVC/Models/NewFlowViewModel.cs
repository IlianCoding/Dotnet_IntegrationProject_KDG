using System.ComponentModel.DataAnnotations;
using NT.BL.Domain.flowpck;

namespace NT.UI.MVC.Models;

public class NewFlowViewModel
{
    public long ProjectId { get; set; }
    public bool State { get; set; }

    [Required]
    [MinLength(2, ErrorMessage = "Name has to be more than 2 letters")]
    public string FlowName { get; set; }

    public bool IsLinear { get; set; }
    public bool IsActive { get; set; }
}