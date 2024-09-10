using System.ComponentModel.DataAnnotations;
using NT.BL.Domain.flowpck;

namespace NT.UI.MVC.Models.Dto;

public class UpdateAnswerOptionDto
{
    public long ConditionalPointId { get; set; }
    public string TextAnswer { get; set; }
}