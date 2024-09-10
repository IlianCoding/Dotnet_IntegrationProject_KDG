namespace NT.UI.MVC.Models.Dto;

public class UpdateQuestionWithOptionDto
{
    public string QuestionText { get; set; }
    public ICollection<string> AnswerOptions { get; set; }
    public string QuestionType { get; set; }
}