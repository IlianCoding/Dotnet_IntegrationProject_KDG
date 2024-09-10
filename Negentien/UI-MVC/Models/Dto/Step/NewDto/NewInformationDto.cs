using System.ComponentModel.DataAnnotations;

namespace NT.UI.MVC.Models.Dto;

public class NewInformationDto
{
    [Required] public string Title { get; set; }

    [Required] public string TextInformation { get; set; }
    public string ObjectName { get; set; }
    public string ContentType { get; set; }
}