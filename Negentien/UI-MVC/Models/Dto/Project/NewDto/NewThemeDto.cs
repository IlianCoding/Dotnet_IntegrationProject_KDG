using Microsoft.Build.Framework;

namespace NT.UI.MVC.Models.Dto;

public class NewThemeDto
{
    
    public long ProjectId { get; set; }
    public string ShortInformation { get; set; }
    [Required]
    public string ThemeName { get; set; }
    
    public bool IsHeadTheme { get; set; }
}