using System.ComponentModel.DataAnnotations;

namespace NT.UI.MVC.Models.Dto.Step.NewDto;

public class NewUserAnswerDto
{
    public string RunningFlowId { get; set; }
    public int? AnswerId { get; set; }
    public string AnswerString { get; set; }
    public string QuestionId { get; set; }
}