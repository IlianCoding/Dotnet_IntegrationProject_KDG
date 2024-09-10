using System.ComponentModel.DataAnnotations;

namespace NT.UI.MVC.Models.Dto;

public class UpdateProjectDto
{
    public long Id { get; set; }
    [Microsoft.Build.Framework.Required]
    [MinLength(2, ErrorMessage = "Name has to be more than 2 letters")]
    public string Name { get; set; }
    public bool IsActive { get; set; }
    public string ProjectInformation { get; set; }
    public string PrimaryColor { get; set; }
    public string Font { get; set; }
    
}