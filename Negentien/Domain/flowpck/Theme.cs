using System.ComponentModel.DataAnnotations;
using NT.BL.Domain.webplatformpck;

namespace NT.BL.Domain.flowpck;

 public class Theme
{
    [Key] public long Id { get; set; }
    public ICollection<Theme> Subthemes { get; set; }
    public string ShortInformation { get; set; }
    [Required]
    public string ThemeName { get; set; }
    public bool IsHeadTheme { get; set; }
    public ICollection<Comment> Comments { get; set; }
}