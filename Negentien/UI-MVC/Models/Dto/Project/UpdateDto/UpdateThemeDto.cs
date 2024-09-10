using Microsoft.Build.Framework;

namespace NT.UI.MVC.Models.Dto;

public class UpdateThemeDto
{
    public long ProjectId { get; set; }
    public string ShortInformation { get; set; }
    
    public string OldThemeName { get; set; }
    
    public string NewThemeName { get; set; }
    public bool IsHeadTheme { get; set; }
}