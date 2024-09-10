using System.ComponentModel.DataAnnotations;
using NT.BL.Domain.flowpck;

namespace NT.UI.MVC.Models.Dto;

public class UpdateStepDto
{
    public string Name { get; set; }
    public long ThemeId { get; set; }
    public long ContentId { get; set; }
    public bool IsConditioneel { get; set; }
    public long NextStepId { get; set; }
}