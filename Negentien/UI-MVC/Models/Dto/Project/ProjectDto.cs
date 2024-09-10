namespace NT.UI.MVC.Models.Dto;

public class ProjectDto
{
    public long Id { get; set; }
    public string Name { get; set; }
    public bool IsActive { get; set; }
    public long ThemeId { get; set; }
    public string ProjectInformation { get; set; }
    public string PrimaryColor { get; set; }
    public string Font { get; set; }
}