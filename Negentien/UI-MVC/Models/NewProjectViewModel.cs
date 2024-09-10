using System.ComponentModel.DataAnnotations;
using NT.BL.Domain.flowpck;

namespace NT.UI.MVC.Models;

public class NewProjectViewModel
{
    [Required]
    [MinLength(2, ErrorMessage = "Name has to be more than 2 letters")]
    public string Name { get; set; }

    public bool IsActive { get; set; }
    
    public Theme Theme { get; set; }
}