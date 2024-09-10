using System.ComponentModel.DataAnnotations;
using NT.BL.Domain.flowpck;

namespace NT.UI.MVC.Models;

public class NewStepViewModel
{
    public Theme Theme { get; set; }
    [Required]
    public Content Content { get; set; }
    
    [Required]
    public bool IsConditioneel { get; set; }
    public Step NextStep { get; set; }
}