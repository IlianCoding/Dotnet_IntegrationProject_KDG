using System.Text;
using NT.BL.Domain.Util;
using NT.DAL.FlowRep;

namespace NT.BL.services;

public class CsvService
{
    private readonly IFlowRepository _flowRepository;

    public CsvService(IFlowRepository flowRepository)
    {
        _flowRepository = flowRepository;
    }


    public string GetAllAnswers(long flowId)
    {
        List<StatisticalQuestion> statisticalQuestions = _flowRepository.GetQuestionsOfFlow(flowId).ToList();

        if (!statisticalQuestions.Any())
        {
            return null;
        }

        StringBuilder stringBuilder = new StringBuilder();

        MakeHeaderAllAnswers(statisticalQuestions, stringBuilder);
        List<StatisticalSession> statisticalSessions = _flowRepository.GetStatisticalSessions(flowId).ToList();

        
        MakeBodyAllAnswers(statisticalQuestions, statisticalSessions, stringBuilder);


        return stringBuilder.ToString();
    }

    private void MakeHeaderAllAnswers(List<StatisticalQuestion> statisticalQuestions,
        StringBuilder stringBuilder)
    {
        foreach (var statisticalQuestion in statisticalQuestions)
        {
            stringBuilder.Append(statisticalQuestion.QuestionId + ": " + statisticalQuestion.QuestionText.Trim() + ";\n");
        }

        foreach (var statisticalQuestion in statisticalQuestions)
        {
            stringBuilder.Append(statisticalQuestion.QuestionId + ";");
        }

        stringBuilder.Append("Start Time;End Time;\n");
    }

    private void MakeBodyAllAnswers(List<StatisticalQuestion> statisticalQuestions,
        List<StatisticalSession> statisticalSessions, StringBuilder stringBuilder)
    {
       

        foreach (var statisticalSession in statisticalSessions)
        {
            foreach (var statisticalQuestion in statisticalQuestions)
            {
                AnswerWithQuestionId answer =
                    statisticalSession.UserAnswers.SingleOrDefault(ua
                        => ua.QuestionId == statisticalQuestion.QuestionId);

                if (answer != null)
                {
                    stringBuilder.Append(answer.Answer + ";");
                }
                else
                {
                    stringBuilder.Append("null;");
                }
            }

            stringBuilder.Append($"{statisticalSession.StartTime};{statisticalSession.EndTime};\n");
        }
    }
}