using System.ComponentModel.DataAnnotations;
using NT.BL.Domain.flowpck;

namespace NT.UI.MVC.Models.Dto;

public class NewStepDto
{
    public long ThemeId { get; set; }
    [Required]
    public long ContentId { get; set; }
    public long FlowId { get; set; }
    public string Name { get; set; }
    public bool IsConditioneel { get; set; }
    
}