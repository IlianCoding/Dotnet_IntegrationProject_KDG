using System.ComponentModel.DataAnnotations;
using NT.BL.Domain.flowpck;
using NT.BL.Domain.questionpck.AnswerDomPck;
using NT.BL.Domain.questionpck.QuestionDomPck;
using NT.BL.Domain.validationpck;
using NT.BL.FlowMngr;
using NT.BL.NoteMngr;
using NT.BL.services;
using NT.DAL.FlowRep;
using NT.DAL.SessionRep.SessionPck;
using NT.DAL.StepRep.AnswerPck;
using NT.DAL.StepRep.ConditionalPck;
using NT.DAL.StepRep.InformationPck;
using NT.DAL.StepRep.QuestionPck;
using NT.DAL.StepRep.StepPck;
using NT.DAL.StepRep.UserAnswerPck;

namespace NT.BL.StepMngr;

public class StepManager : IStepManager
{
    private readonly IAnswerRepository _answerRepository;
    private readonly IConditionalPointRepository _conditionalPointRepository;
    private readonly IInformationRepository _informationRepository;
    private readonly ISessionRepository _sessionRepository;
    private readonly IQuestionRepository _questionRepository;
    private readonly IStepRepository _stepRepository;
    private readonly IUserAnswerRepository _userAnswerRepository;
    private readonly IFlowRepository _flowRepository;
    private readonly IFlowManager _flowManager;
    private readonly INoteManager _noteManager;
    private readonly CloudStorageService _cloudStorageService;

    public StepManager(IAnswerRepository answerRepository, IConditionalPointRepository conditionalPointRepository, IInformationRepository informationRepository, ISessionRepository sessionRepository, IQuestionRepository questionRepository, IStepRepository stepRepository, IUserAnswerRepository userAnswerRepository, IFlowRepository flowRepository, IFlowManager flowManager, INoteManager noteManager, CloudStorageService cloudStorageService)
    {
        _answerRepository = answerRepository;
        _conditionalPointRepository = conditionalPointRepository;
        _informationRepository = informationRepository;
        _sessionRepository = sessionRepository;
        _questionRepository = questionRepository;
        _stepRepository = stepRepository;
        _userAnswerRepository = userAnswerRepository;
        _flowRepository = flowRepository;
        _flowManager = flowManager;
        _noteManager = noteManager;
        _cloudStorageService = cloudStorageService;
    }

    #region Answer

    public Answer AddAnswerOpen(string answerString, long questionId)
    {
        AnswerOpen answerOpen = new AnswerOpen
        {
            Answer = answerString,
            QuestionId = questionId
        };

        Answer newAnswer = _answerRepository.CreateAnswerOpen(answerOpen);

        return _answerRepository.ReadAnswer(newAnswer.Id);
    }

    public AnswerOption ChangeAnswerOptionConditionalPoint(long id, ConditionalPoint conditionalPoint)
    {
        AnswerOption answerOption = _answerRepository.ReadAnswer(id) as AnswerOption;
        if (answerOption != null)
        {
            answerOption.ConditionalPoint = conditionalPoint;
            _answerRepository.UpdateAnswerOptionConditionalPoint(answerOption);
            return answerOption;
        }

        return null;
    }

    #endregion
    #region ConditionalPoint

    public ConditionalPoint AddConditionalPoint(string name, Step step)
    {
        ConditionalPoint newConditionalPoint = new ConditionalPoint
        {
            ConditionalPointName = name,
            ConditionalStep = step
        };
        ICollection<ValidationResult> addVisitorErrors = new List<ValidationResult>();
        bool isValid = Validator.TryValidateObject(newConditionalPoint, new ValidationContext(newConditionalPoint),
            addVisitorErrors
            , validateAllProperties: true);
        if (!isValid)
        {
            string errorMessage = "";
            foreach (ValidationResult vr in addVisitorErrors)
            {
                errorMessage += vr.ErrorMessage + "\n";
            }

            throw new ValidationException(errorMessage);
        }

        _conditionalPointRepository.CreateConditionalPoint(newConditionalPoint);
        return newConditionalPoint;
    }

    public ConditionalPoint GetConditionalPoint(long id)
    {
        return _stepRepository.ReadConditionalPoint(id);
    }

    public void ChangeStep(long id, string stepDtoName, Theme stepDtoTheme, bool stepDtoIsConditioneel, Content stepDtoContent)
    {
        var changedStep = _stepRepository.ReadStep(id);
        changedStep.Name = stepDtoName;
        changedStep.Theme = stepDtoTheme;
        changedStep.IsConditioneel = stepDtoIsConditioneel;
        changedStep.Content = stepDtoContent;
        _stepRepository.UpdateStep(changedStep);
    }

    public void ChangeStepIsConditioneel(long id, bool stepDtoIsConditioneel)
    {
        var step = _stepRepository.ReadStep(id);
        step.IsConditioneel = stepDtoIsConditioneel;
        _stepRepository.UpdateStep(step);
    }

    public void ChangeAnswerOptionCp(long id, ConditionalPoint conditionalPoint)
    {
        var changedAnswerOption = _answerRepository.ReadAnswerOption(id);
        changedAnswerOption.ConditionalPoint = conditionalPoint;
        _stepRepository.UpdateAnswerOption(changedAnswerOption);
    }

    public AnswerOption GetAnswerOption(long id)
    {
        return _answerRepository.ReadAnswerOption(id);
    }

    public void ChangeStepNextStep(long stepId, Step nextStep)
    {
        var step = _stepRepository.ReadStep(stepId);
        step.NextStep = nextStep;
        _stepRepository.UpdateStep(step);
    }

    public Step GetStepWithStepContentThemeAndState(long stepId)
    {
        return _stepRepository.ReadStepWithStepContentThemeAndState(stepId);
    }

  

    public AnswerOption GetAnswerOptionWithConditionalPoint(long answerOptionId)
    {
        return _answerRepository.ReadAnswerOptionWithConditionalPoint(answerOptionId);
    }

    public UserAnswer GetUserAnswerByStep(long stepId)
    {
        var userAnswers = _answerRepository.ReadAllUserAnswersWithStep();
        Step step = _stepRepository.ReadStep(stepId);
        //ICollection<UserAnswer> userAnswersWithStep = new List<UserAnswer>();
        //TODO: deze code moet aangepast worden wanneer de functionaliteit is geïmplementeerd waarbij meerdere useranwswers op eenzelfde step kan gegeven worden
        foreach (var userAnswer in userAnswers)
        {
            if (userAnswer.Step == step)
            {
                return userAnswer;
            }
        }

        return null;
    }

    public Step GetStepByNextStepId(long stepId)
    {
        IEnumerable<Step> steps =_stepRepository.ReadAllStepsWithNextStep();
        foreach (Step step in steps)
        {
            
            if (step.NextStep!=null && step.NextStep.Id == stepId)
            {
                return step;
            }
        }

        return null;
    }

    public Step GetStepWithContentAndTheme(long stepId)
    {
        return _stepRepository.ReadStepWithContentAndTheme(stepId);
    }

    public void RemoveConditionalPoint(long conditionalPointId)
    {
        _stepRepository.DeleteConditionalPoint(conditionalPointId);
    }

    public ConditionalPoint GetConditionalPointByStepid(long stepId)
    {
        return _stepRepository.ReadConditionalPointByStepid(stepId);
    }

    public ConditionalPoint GetConditionalPointWithConditionalStep(long cpId)
    {
        return _stepRepository.ReadConditionalPointWithConditionalStep(cpId);
    }

    public Content ChangeQuestionWithOptions(long id, string questionText)
    {
        QuestionWithOption questionWithOption = _questionRepository.ReadQuestionWithOptions(id);
        questionWithOption.QuestionText = questionText;
        return _questionRepository.UpdateQuestionWithOption(questionWithOption);
    }

    public Content ChangeQuestionOpen(long id, string questionText)
    {
        QuestionOpen questionOpen = _questionRepository.ReadQuestionOpen(id);
        questionOpen.QuestionText = questionText;
         return _questionRepository.UpdateQuestionOpen(questionOpen);
    }

    public Content ChangeInformation(long id, string updateInformationContentType, string updateInformationObjectName,
        string updateInformationTextInformation, string updateInformationTitle)
    {
        InformationContent info = _questionRepository.ReadInformation(id);
        _cloudStorageService.RemoveMedia(info.ObjectName);
        info.ContentType = updateInformationContentType;
        info.ObjectName = updateInformationObjectName;
        info.TextInformation = updateInformationTextInformation;
        info.Title = updateInformationTitle;
        return _questionRepository.UpdateInfo(info);
    }

    public ConditionalPoint ChangeConditionalPoint(long id, string conditionalPointName, Step step)
    {
        ConditionalPoint conditionalPoint = _stepRepository.ReadConditionalPointWithConditionalStep(id);
        Step stepCP = conditionalPoint.ConditionalStep;
        stepCP.NextStep = null;
        _stepRepository.UpdateStep(stepCP);
        conditionalPoint.ConditionalPointName = conditionalPointName;
        conditionalPoint.ConditionalStep = step;
        return _stepRepository.UpdateConditionalPoint(conditionalPoint);
    }

    #endregion
    #region Question

    public QuestionWithOption AddQuestionWithOptions(string questionString, ICollection<string> options, string questionType)
    {
        QuestionType type;
        switch (questionType)
        {
            case "multi":
                type = QuestionType.Multiple;
                break;
            case "single":
                type = QuestionType.Single;
                break;
            case "range":
                type = QuestionType.Range;
                break;
            default:
                throw new Exception("Invalid question type");
        }
        
        List<ValidationResult> errorsQuestion = new List<ValidationResult>();
        QuestionWithOption questionWithOptions = new QuestionWithOption
        {
            QuestionText = questionString,
            QuestionType = type
        };
        bool isValidQuestion = Validator.TryValidateObject(questionWithOptions,
            new ValidationContext(questionWithOptions), errorsQuestion, true);
        if (!isValidQuestion)
        {
            List<ValidationResult> valResults = new List<ValidationResult>();
            foreach (ValidationResult validationResult in errorsQuestion)
            {
                valResults.Add(validationResult);
            }

            throw new ValidationListException(valResults);
        }

        QuestionWithOption newQuestionWithOptions = _questionRepository.CreateQuestionWithOptions(questionWithOptions);

        foreach (var answerText in options)
        {
            List<ValidationResult> errorsAnswer = new List<ValidationResult>();
            AnswerOption answerOption = new AnswerOption
            {
                QuestionId = newQuestionWithOptions.Id,
                TextAnswer = answerText
            };
            bool isValidAnswer =
                Validator.TryValidateObject(answerOption, new ValidationContext(answerOption), errorsAnswer, true);
            if (!isValidAnswer)
            {
                List<ValidationResult> valResults = new List<ValidationResult>();
                foreach (ValidationResult validationResult in errorsAnswer)
                {
                    valResults.Add(validationResult);
                }

                throw new ValidationListException(valResults);
            }

            AnswerOption newAnswerOption = _answerRepository.CreateAnswerOption(answerOption);
            _questionRepository.UpdateAnswerOptionToQuestion(newQuestionWithOptions.Id, newAnswerOption);
        }

        return _questionRepository.ReadQuestionWithOptions(newQuestionWithOptions.Id);
    }

    public Content AddQuestionOpen(string questionText)
    {
        QuestionOpen questionOpen = new QuestionOpen
        {
            QuestionType = QuestionType.Open,
            QuestionText = questionText
        };
        List<ValidationResult> errors = new List<ValidationResult>();
        bool isValid = Validator.TryValidateObject(questionOpen, new ValidationContext(questionOpen), errors, true);

        if (!isValid)
        {
            List<ValidationResult> valResults = new List<ValidationResult>();
            foreach (ValidationResult validationResult in errors)
            {
                valResults.Add(validationResult);
            }

            throw new ValidationListException(valResults);
        }

        return _questionRepository.CreateQuestionOpen(questionOpen);
    }

    public Content AddInformation(string newInformationTitle, string newInformationTextInformation, string objectName, string contentType)
    {
        InformationContent newInformation = new InformationContent()
        {
            Title = newInformationTitle,
            TextInformation = newInformationTextInformation
        };

        newInformation.ObjectName = objectName;
        newInformation.ContentType=contentType;

       

        List<ValidationResult> errors = new List<ValidationResult>();
        bool isValid = Validator.TryValidateObject(newInformation, new ValidationContext(newInformation), errors, true);

        if (!isValid)
        {
            List<ValidationResult> valResults = new List<ValidationResult>();
            foreach (ValidationResult validationResult in errors)
            {
                valResults.Add(validationResult);
            }

            throw new ValidationListException(valResults);
        }
        return _informationRepository.CreateInformation(newInformation);
    }

    public Content GetContent(long stepContentId)
    {
        return _stepRepository.ReadContent(stepContentId);
    }

    public Step AddStep(Theme theme, Content content, State state, bool isConditioneel, Step nextStep)
    {
        Step newStep = new Step
        {
            Theme = theme,
            Content = content,
            StepState = state,
            IsConditioneel = isConditioneel,
            NextStep = nextStep
        };
        ICollection<ValidationResult> addVisitorErrors = new List<ValidationResult>();
        bool isValid = Validator.TryValidateObject(newStep, new ValidationContext(newStep),
            addVisitorErrors
            , validateAllProperties: true);
        if (!isValid)
        {
            string errorMessage = "";
            foreach (ValidationResult vr in addVisitorErrors)
            {
                errorMessage += vr.ErrorMessage + "\n";
            }

            throw new ValidationException(errorMessage);
        }

        _stepRepository.CreateStep(newStep);
        return newStep;
    }

    public void AddStepToFlow(long stepFlowId, long stepId)
    {
        Step step = _stepRepository.ReadStep(stepId);
        if (step == null)
        {
            throw new InvalidOperationException("Step does not exist");
        }
        Flow flow = _flowRepository.ReadFlowById(stepFlowId);
        if (flow == null)
        {
            throw new InvalidOperationException("Flow does not exist");
        }
        _stepRepository.CreateStepToFlow(flow, step);
    }

    #endregion
    #region UserAnswers

    public UserAnswer AddUserAnswer(long answerId, long sessionId, bool answerOpen)
    {
        Session session = _sessionRepository.ReadSessionById(sessionId);
        
        if (answerOpen == false)
        {
            Answer answer = _answerRepository.ReadAnswer(answerId);

            if (answer == null)
            {
                throw new InvalidOperationException("Answer does not exist");
            }

            QuestionWithOption questionWithOption = _questionRepository.ReadQuestionFromAnswer((AnswerOption)answer);
            Question question = _questionRepository.ReadQuestionType(questionWithOption.Id);

            if (question.QuestionType == QuestionType.Range || question.QuestionType == QuestionType.Single)
            {
                var existingUserAnswer = session.UserAnswers
                    .FirstOrDefault(ua => ua.AnswerOption != null && ua.AnswerOption.QuestionId == question.Id);

                if (existingUserAnswer != null)
                {
                    _userAnswerRepository.UpdateUserAnswer(existingUserAnswer, answerId, sessionId);
                    return existingUserAnswer;
                }
            }
            UserAnswer userAnswer = new UserAnswer()
            {
                AnswerId = answerId,
                Session = session,
                AnswerOption = (AnswerOption)answer
            };
            
            _userAnswerRepository.CreateUserAnswer(userAnswer);
            return userAnswer;
        }
        else
        {
            UserAnswer userAnswer = new UserAnswer()
            {
                AnswerId = answerId,
                Session = session
            };
            _userAnswerRepository.CreateUserAnswer(userAnswer);
            return userAnswer;
        }
    }
    
    public void AddUserOpenAnswer(Session session, string answerString, long questionId)
    {
        Answer newAnswerOpen = AddAnswerOpen(answerString, questionId);

        if (newAnswerOpen == null)
        {
            throw new InvalidOperationException("Answer not created");
        }

        AddUserAnswer(newAnswerOpen.Id, session.Id, true);
    }

    public UserAnswer GetUserAnswer(long answerId, Session session)
    {
        return _userAnswerRepository.ReadUserAnswer(answerId, session.Id);
    }

    #endregion
    #region Steps

    public Step GetStep(long stepId)
    {
        Step step = _stepRepository.ReadStep(stepId);
        if (step == null)
        {
            throw new InvalidOperationException("Step does not exist");
        }

        if (step.Content is QuestionWithOption)
        {
            return _stepRepository.ReadStepWithQuestionOptions(stepId);
        }

        if (step.Content is QuestionOpen)
        {
            return _stepRepository.ReadStepWithQuestionOpen(stepId);
        }

        return step;
    }

    public Step GetStepWithStep(long stepId)
    {
        return _stepRepository.ReadStepWithStep(stepId);
    }

    public Step AddStep(Theme theme, Content content, State state, bool isConditioneel, Step nextStep, string name)
    {
        Step newStep = new Step
        {
            Theme = theme,
            Content = content,
            StepState = state,
            IsConditioneel = isConditioneel,
            NextStep = nextStep,
            Name = name
        };
        ICollection<ValidationResult> addVisitorErrors = new List<ValidationResult>();
        bool isValid = Validator.TryValidateObject(newStep, new ValidationContext(newStep),
            addVisitorErrors
            , validateAllProperties: true);
        if (!isValid)
        {
            string errorMessage = "";
            foreach (ValidationResult vr in addVisitorErrors)
            {
                errorMessage += vr.ErrorMessage + "\n";
            }

            throw new ValidationException(errorMessage);
        }

        _stepRepository.CreateStep(newStep);
        return newStep;
    }

    public IEnumerable<Step> GetAllConditionalSteps()
    {
        return _stepRepository.ReadAllConditionalSteps();
    }

    public void RemoveStepAndRelocate(long stepId)
    {
        Flow flow = _flowManager.GetFlowByStepId(stepId);
        Step firstStep = flow.FirstStep;
        Step step = GetStepWithStepContentThemeAndState(stepId);
        //TODO: een juist aangemaakt step is niet de eerste step maar heeft ook geen previousstep
        if (step == firstStep && step.NextStep==null || step.IsConditioneel)
        {
            ConditionalPoint conditionalPoint = GetConditionalPointByStepid(stepId);
            if (step.IsConditioneel && conditionalPoint!=null)
            {
                RemoveConditionalPoint(conditionalPoint.Id);
            }
            RemoveStep(stepId, flow.Id);

        }
        else if (firstStep==step)
        {
            Step nextStep = GetStep(step.NextStep.Id);
            _flowManager.ChangeFlowFirstStep(flow.Id, nextStep);
            RemoveStep(stepId, flow.Id);

        }else if (step.NextStep== null)
        {
            Step previousStep = GetStepByNextStepId(step.Id);
            ChangeStepNextStep(previousStep.Id, null);
            RemoveStep(stepId, flow.Id);

        }
        else
        {
            Step previousStep = GetStepByNextStepId(step.Id);
            Step nextStep = GetStep(step.NextStep.Id);
            ChangeStepNextStep(previousStep.Id, nextStep);
            RemoveStep(stepId, flow.Id);

        }
    }
    public void RemoveStep(long stepId, long flowId)
    {
        Step step = _stepRepository.ReadStepWithStepContentThemeAndState(stepId);
        _noteManager.RemoveNoteByStepId(stepId);
        _flowRepository.DeleteStepFromFlow(stepId, flowId);
        if (step.Content is QuestionWithOption questionWithOption)
        {
            _stepRepository.DeleteQuestionWithOption(questionWithOption.Id);
        }
        else if (step.Content is QuestionOpen questionOpen)
        {
            _stepRepository.DeleteQuestionOpen(questionOpen.Id);
            
        }
        else if (step.Content is InformationContent informationContent)
        {
            _cloudStorageService.RemoveMedia(informationContent.ObjectName);
            _stepRepository.DeleteInformationContent(informationContent.Id);
        }
        ConditionalPoint conditionalPoint = GetConditionalPointByStepid(stepId);
        if (step.IsConditioneel && conditionalPoint!=null)
        {
            RemoveConditionalPoint(conditionalPoint.Id);
        }
        _stepRepository.DeleteStep(stepId);
    }

    public void DeactivateStep(long stepId)
    {
        Step step = _stepRepository.ReadStep(stepId);
        step.StepState = State.Closed;
        _stepRepository.UpdateStep(step);
    }

    public void ActivateStep(long stepId)
    {
        Step step = _stepRepository.ReadStep(stepId);
        step.StepState = State.Open;
        _stepRepository.UpdateStep(step);
    }

    #endregion
}