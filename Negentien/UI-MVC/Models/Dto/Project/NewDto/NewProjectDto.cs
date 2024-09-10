namespace NT.UI.MVC.Models.Dto.Project.NewDto;

public class NewProjectDto
{
    public string Name { get; set; }

    public bool IsActive { get; set; }

    public long ThemeId { get; set; }
    public string ProjectInformation { get; set; }
}