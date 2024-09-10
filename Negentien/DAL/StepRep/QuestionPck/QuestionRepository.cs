using Microsoft.EntityFrameworkCore;
using NT.BL.Domain.flowpck;
using NT.BL.Domain.questionpck.AnswerDomPck;
using NT.BL.Domain.questionpck.QuestionDomPck;
using NT.DAL.EF;

namespace NT.DAL.StepRep.QuestionPck;

public class QuestionRepository : IQuestionRepository
{
    private readonly PhygitalDbContext _phygitalDbContext;

    public QuestionRepository(PhygitalDbContext phygitalDbContext)
    {
        _phygitalDbContext = phygitalDbContext ?? throw new ArgumentNullException(nameof(phygitalDbContext));
    }

    public QuestionWithOption CreateQuestionWithOptions(QuestionWithOption questionWithOption)
    {
        _phygitalDbContext.Questions.Add(questionWithOption);
        _phygitalDbContext.SaveChanges();
        return ReadQuestionWithOptions(questionWithOption.Id);
    }

    public void UpdateAnswerOptionToQuestion(long id, AnswerOption newAnswerOption)
    {
        QuestionWithOption questionWithOption = ReadQuestionWithOptions(id);
        questionWithOption.AnswerOptions.Add(newAnswerOption);
        _phygitalDbContext.SaveChanges();
    }

    public QuestionWithOption ReadQuestionWithOptions(long id)
    {
        return _phygitalDbContext.Questions
            .OfType<QuestionWithOption>()
            .SingleOrDefault(q => q.Id == id);
    }

    public Question ReadQuestionType(long questionId)
    {
        return _phygitalDbContext.Questions
            .Find(questionId);
    }

    public QuestionWithOption ReadQuestionFromAnswer(AnswerOption answer)
    {
        return _phygitalDbContext.QuestionWithOptions
            .Include(qwo => qwo.AnswerOptions)
            .SingleOrDefault(qwo => qwo.AnswerOptions.Any(ao => ao.Id == answer.Id));
    }


    public QuestionWithOption ReadQuestionWithOptionWithAnswerOptions(long questionId)
    {
        return _phygitalDbContext.QuestionWithOptions
            .Include(ans => ans.AnswerOptions)
            .SingleOrDefault(rea => rea.Id == questionId);
    }

    public QuestionOpen CreateQuestionOpen(QuestionOpen questionOpen)
    {
        _phygitalDbContext.Questions.Add(questionOpen);
        _phygitalDbContext.SaveChanges();
        
        return ReadQuestionOpen(questionOpen.Id);
    }

    public Content UpdateQuestionWithOption(QuestionWithOption questionWithOption)
    {
        _phygitalDbContext.QuestionWithOptions.Update(questionWithOption);
        _phygitalDbContext.Questions.Update(questionWithOption);
        _phygitalDbContext.Content.Update(questionWithOption);

        _phygitalDbContext.SaveChanges();
        return _phygitalDbContext.Content
            .SingleOrDefault(content => content.Id == questionWithOption.Id);
    }

    public Content UpdateQuestionOpen(QuestionOpen questionOpen)
    {
        _phygitalDbContext.OpenQuestions.Update(questionOpen);
        _phygitalDbContext.Questions.Update(questionOpen);
        _phygitalDbContext.Content.Update(questionOpen);

        _phygitalDbContext.SaveChanges();
        return _phygitalDbContext.Content
            .SingleOrDefault(content => content.Id == questionOpen.Id);
    }

    public InformationContent ReadInformation(long id)
    {
        return _phygitalDbContext.InformationContent
            .SingleOrDefault(i => i.Id == id);

    }

    public Content UpdateInfo(InformationContent info)
    {
        _phygitalDbContext.InformationContent.Update(info);
        _phygitalDbContext.Content.Update(info);
        _phygitalDbContext.SaveChanges();    
        
        return _phygitalDbContext.Content
            .SingleOrDefault(content => content.Id == info.Id);
    }

    public QuestionOpen ReadQuestionOpen(long questionOpenId)
    {
        return _phygitalDbContext.Questions
            .OfType<QuestionOpen>()
            .SingleOrDefault(q => q.Id == questionOpenId);
    }
}