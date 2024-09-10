using System.ComponentModel.DataAnnotations;

namespace NT.UI.MVC.Models.Dto;

public class ConditionalPointDto
{
    [Key]
    public long Id { get; set; }
    public string ConditionalPointName { get; set; }
    public BL.Domain.flowpck.Step ConditionalStep { get; set; }
}