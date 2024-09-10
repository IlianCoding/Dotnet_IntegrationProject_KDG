using System.Globalization;
using NT.BL.Domain.flowpck;
using NT.BL.Domain.Util;
using NT.DAL.FlowRep;
using NT.DAL.PlatformRep.HeadplatformPck;
using NT.DAL.PlatformRep.SharingplatformPck;
using NT.DAL.ProjectRep.ThemePck;

namespace NT.BL.services;

public class StatisticsService
{
    private readonly IHeadplatformRepository _headPlatformRepository;
    private readonly ISharingplatformRepository _sharingPlatformRepository;
    private readonly IThemeRepository _themeRepository;
    private readonly IFlowRepository _flowRepository;

    public StatisticsService(IHeadplatformRepository headPlatformRepository,
        ISharingplatformRepository sharingPlatformRepository,
        IThemeRepository themeRepository, IFlowRepository flowRepository)
    {
        _headPlatformRepository = headPlatformRepository;
        _sharingPlatformRepository = sharingPlatformRepository;
        _themeRepository = themeRepository;
        _flowRepository = flowRepository;
    }

    #region headPlatform

    public HeadPlatformNumbers GetHeadPlatformStatistics()
    {
        return _headPlatformRepository.ReadHeadPlatformNumbers();
    }

    public IEnumerable<ProjectDataPerSharingPlatform> GetTotalAndActiveProjectCountPerSharingPlatform()
    {
        return _headPlatformRepository.ReadTotalAndActiveProjectCountPerSharingPlatform();
    }

    public IEnumerable<AvgFlowPerProjectPerSharingPlatformData> GetAvgFlowPerProjectPerSharingPlatform()
    {
        return _headPlatformRepository.ReadAvgFlowPerProjectPerSharingPlatform();
    }

    #endregion

    public SharingPlatformNumbers GetSharingPlatformStatistics(long platformId)
    {
        var sharingPlatformNumbers = _sharingPlatformRepository.ReadSharingPlatformStatistics(platformId);
        sharingPlatformNumbers.ProjectData = _sharingPlatformRepository.ReadProjectIds(platformId);
        sharingPlatformNumbers.FlowData = _sharingPlatformRepository.ReadFlowIds(platformId);
        return sharingPlatformNumbers;
    }

    public IEnumerable<ParticipantsPerFlow> GetParticipantsPerFlow(long platformId)
    {
        var flows = _sharingPlatformRepository.ReadAllFlowsFromPlatform(platformId);

        ICollection<ParticipantsPerFlow> participantsPerFlows = new List<ParticipantsPerFlow>();
        foreach (var flow in flows)
        {
            participantsPerFlows.Add(new ParticipantsPerFlow()
            {
                FlowName = flow.FlowName,
                Participants = flow.RunningFlows.Sum(rf => rf.Sessions.Count)
            });
        }


        return participantsPerFlows;
    }

    public IEnumerable<RunningFlowsPerFlow> GetRunningFlowDataOfPlatform(long platformId, int year)
    {
        var flows = _sharingPlatformRepository.ReadAllFlowsFromPlatform(platformId);

        ICollection<RunningFlowsPerFlow> runningFlowDataForCollectionOfFlows =
            new List<RunningFlowsPerFlow>();
        foreach (var flow in flows)
        {
            var runningFlowDataPerFlow = GetRunningFlowDataPerFlow(flow, year);

            if (runningFlowDataPerFlow != null)
            {
                runningFlowDataForCollectionOfFlows.Add(
                    new RunningFlowsPerFlow()
                    {
                        FlowName = flow.FlowName,
                        RunningFlowsPerFlowPerMonth = runningFlowDataPerFlow
                    });
            }
        }

        return runningFlowDataForCollectionOfFlows;
    }

    private IEnumerable<RunningFlowsPerFlowPerMonth> GetRunningFlowDataPerFlow(Flow flow, int year)
    {
        ICollection<RunningFlowsPerFlowPerMonth> runningFlowData = new List<RunningFlowsPerFlowPerMonth>();
        string[] names = DateTimeFormatInfo.CurrentInfo.MonthNames;

        for (var i = 0; i < names.Length - 1; i++)
        {
            runningFlowData.Add(new RunningFlowsPerFlowPerMonth()
            {
                Month = names[i],
                AmountOfRunningFlowsCreated = flow.RunningFlows.Count(rf =>
                    rf.RunningFlowTime.Month == (i + 1) && rf.RunningFlowTime.Year == year)
            });
        }

        foreach (var runningFlowsPerFlowPerMonth in runningFlowData)
        {
            if (runningFlowsPerFlowPerMonth.AmountOfRunningFlowsCreated != 0)
            {
                return runningFlowData;
            }
        }

        return null;
    }

    public IEnumerable<ThemeAndAmountOfStepsAttached> GetThemesAndAmountOfStepsAttachedToIt(long projectId)
    {
        var steps = _sharingPlatformRepository.ReadStepsWithTheme(projectId);
        var themesAndAmountOfStepsAttached =
            steps.GroupBy(step => step.Theme)
                .Select(group => new ThemeAndAmountOfStepsAttached()
                {
                    ThemeName = group.Key.ThemeName,
                    AmountOfStepsAttached = group.Count()
                }).ToList();

        CheckAllThemeNamesPresent(projectId, themesAndAmountOfStepsAttached);
        return themesAndAmountOfStepsAttached;
    }

    private void CheckAllThemeNamesPresent(long projectId,
        ICollection<ThemeAndAmountOfStepsAttached> themesAndAmountOfStepsAttached)
    {
        var themeNames = _themeRepository.ReadThemesByProjectId(projectId).ToList();

        if (themesAndAmountOfStepsAttached.Count == themeNames.Count)
        {
            return;
        }

        var stepsThemeNames = themesAndAmountOfStepsAttached
            .Select(step => step.ThemeName).ToList();

        foreach (var themeName in themeNames)
        {
            if (!stepsThemeNames.Contains(themeName))
            {
                themesAndAmountOfStepsAttached.Add(new ThemeAndAmountOfStepsAttached()
                {
                    ThemeName = themeName,
                    AmountOfStepsAttached = 0
                });
            }
        }
    }

    public IEnumerable<QuestionWithMostChosenAnswer> GetMostPopularAnswerPerQuestion(long flow)
    {
        ICollection<QuestionWithMostChosenAnswer> questionWithMostChosenAnswers =
            new List<QuestionWithMostChosenAnswer>();
        var allAnswerOptions = _flowRepository.ReadAllUserAnswerOptionsUnderFlow(flow).ToList();
        var questions = _flowRepository.ReadAllSingleAndMultipleChoiceQuestions(flow);

        foreach (var question in questions)
        {
            if (question.AnswerOptions == null)
            {
                continue;
            }

            OptionWithAMount optionWithAmount = new OptionWithAMount()
            {
                AmountOfTimesChosen = 0
            };

            if (allAnswerOptions != null )
            {
                foreach (var option in question.AnswerOptions)
                {
                    int amountOfTimesChosenOption = allAnswerOptions.Count(au => au.Id == option.Id);

                    if (optionWithAmount.AmountOfTimesChosen < amountOfTimesChosenOption)
                    {
                        optionWithAmount.AnswerOption = option;
                        optionWithAmount.AmountOfTimesChosen = amountOfTimesChosenOption;
                    }
                }
            }

            if (optionWithAmount.AnswerOption == null)
            {
                questionWithMostChosenAnswers.Add(new QuestionWithMostChosenAnswer()
                {
                    Question = question.QuestionWithOption.QuestionText,
                    MostPopularAnswer = null
                });
                continue;
            }

            questionWithMostChosenAnswers.Add(new QuestionWithMostChosenAnswer()
            {
                Question = question.QuestionWithOption.QuestionText,
                MostPopularAnswer = optionWithAmount.AnswerOption.TextAnswer
            });
        }

        return questionWithMostChosenAnswers;
    }
}