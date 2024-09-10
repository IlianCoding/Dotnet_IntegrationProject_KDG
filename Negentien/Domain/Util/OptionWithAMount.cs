using NT.BL.Domain.questionpck.AnswerDomPck;

namespace NT.BL.Domain.Util;

public class OptionWithAMount
{
    public AnswerOption AnswerOption { get; set; }
    public int AmountOfTimesChosen { get; set; }
}