using System.ComponentModel.DataAnnotations;
using NT.BL.Domain.flowpck;

namespace NT.BL.Domain.projectpck;

public class Project
{
    [Key] 
    public long Id { get; set; }
    [Required]
    [MinLength(2, ErrorMessage = "Name has to be more than 2 letters")]
    public string Name { get; set; }
    public bool IsActive { get; set; }
    public Theme Theme { get; set; }
    public ICollection<Flow> Flows { get; set; }
    public string ProjectInformation { get; set; }
    public string PrimaryColor { get; set; } = "Default";
    public string Font { get; set; } = "Default";
}