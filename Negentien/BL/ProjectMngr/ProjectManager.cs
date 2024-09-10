using System.ComponentModel.DataAnnotations;
using System.Text;
using NT.BL.Domain.flowpck;
using NT.BL.Domain.projectpck;
using NT.BL.Domain.users;
using NT.BL.Domain.webplatformpck;
using NT.BL.FlowMngr;
using NT.BL.PlatformMngr;
using NT.BL.UserMngr;
using NT.DAL.FlowRep;
using NT.DAL.PlatformRep.SharingplatformPck;
using NT.DAL.ProjectRep.ProjectPck;
using NT.DAL.ProjectRep.ThemePck;
using NT.DAL.StepRep.StepPck;
using NT.DAL.WebPlatformRep;

namespace NT.BL.ProjectMngr;

public class ProjectManager : IProjectManager
{
    private readonly IProjectRepository _projectRepository;
    private readonly IFlowRepository _flowRepository;
    private readonly IThemeRepository _themeRepository;
    private readonly IStepRepository _stepRepository;
    private readonly ISharingplatformRepository _sharingplatformRepository;
    private readonly ICommentRepository _commentRepository;
    private readonly IFlowManager _flowManager;
    private readonly IGeneralUserManager _generalUserManager;

    public ProjectManager(IProjectRepository projectRepository, IThemeRepository themeRepository,
        IStepRepository stepRepository, ISharingplatformRepository sharingplatformRepository,
        ICommentRepository commentRepository, IFlowManager flowManager, IFlowRepository flowRepository, IGeneralUserManager generalUserManager)
    {
        _projectRepository = projectRepository;
        _flowRepository = flowRepository;
        _themeRepository = themeRepository;
        _stepRepository = stepRepository;
        _sharingplatformRepository = sharingplatformRepository;
        _commentRepository = commentRepository;
        _flowManager = flowManager;
        _generalUserManager = generalUserManager;
    }

    #region Theme

    public Theme GetTheme(long projectId, string chosenTheme, bool isHeadTheme)
    {
        return _themeRepository.ReadThemeByProjectIdAndThemeName(projectId, chosenTheme, isHeadTheme);
    }

    public Theme GetThemeById(long stepContentId)
    {
        return _projectRepository.ReadThemeById(stepContentId);
    }

    public Theme GetThemeByName(string themeName)
    {
        return _themeRepository.ReadThemeByName(themeName);
    }

    public Theme GetThemeWithComments(long themeId)
    {
        return _themeRepository.ReadThemeWithComments(themeId);
    }

    public IEnumerable<Theme> GetAllHeadThemes()
    {
        return _themeRepository.ReadAllHeadThemesWithSubThemes();
    }

    private Theme AddTheme(string shortInfo, string themeName, bool isHeadTheme)
    {
        Theme theme = new Theme()
        {
            ThemeName = themeName,
            ShortInformation = shortInfo,
            IsHeadTheme = isHeadTheme,
            Subthemes = new List<Theme>()
        };

        ValidateTheme(theme);
        _themeRepository.CreateTheme(theme);

        return theme;
    }

    private void ConnectThemeToProject(Theme theme, Project projectOfTheme)
    {
        if (theme.IsHeadTheme)
        {
            projectOfTheme.Theme = theme;
            _projectRepository.Update(projectOfTheme);
        }
        else
        {
            if (projectOfTheme.Theme == null)
            {
                return;
            }

            Theme headTheme = projectOfTheme.Theme;
            projectOfTheme.Theme.Subthemes.Add(theme);
            _themeRepository.UpdateTheme(projectOfTheme.Theme);
        }
    }

    public Theme AddThemeToProject(string shortInfo, string themeName, bool isHeadTheme, long projectId)
    {
        if (DoesThemeExist(projectId, themeName, isHeadTheme))
        {
            return null;
        }

        Theme theme = AddTheme(shortInfo, themeName, isHeadTheme);

        Project projectOfTheme = GetProjectWithThemes(projectId);

        ConnectThemeToProject(theme, projectOfTheme);
        return theme;
    }

    public Theme AddHeadThemeToProject(string themeName, string shortInformation, long projectId)
    {
        Theme theme = new Theme
        {
            ThemeName = themeName,
            ShortInformation = shortInformation,
            IsHeadTheme = true,
            Subthemes = null
        };
        _themeRepository.CreateTheme(theme);

        Project project = _projectRepository.ReadProjectById(projectId);
        project.Theme = theme;
        _projectRepository.Update(project);

        return theme;
    }

    public Theme UpdateTheme(long themeId, string name, string description)
    {
        Theme theme = _themeRepository.ReadThemeById(themeId);
        theme.ThemeName = name;
        theme.ShortInformation = description;
        _themeRepository.UpdateTheme(theme);
        return theme;
    }

    public void RemoveTheme(Theme theme, long projectId)
    {
        var headTheme = _themeRepository.ReadHeadThemeByProjectId(projectId);
        RemoveCommentsOfTheme(theme);
        RemoveThemeFromSteps(theme, headTheme);
        _themeRepository.DeleteTheme(theme);
    }

    private void RemoveCommentsOfTheme(Theme theme)
    {
        var themeWithComments = _themeRepository.ReadThemeWithComments(theme.Id);
        _commentRepository.DeleteCommentsOfTheme(themeWithComments);
    }

    private void RemoveThemeFromSteps(Theme oldTheme, Theme newTheme)
    {
        var steps = _stepRepository.ReadStepsAttachedToTheme(oldTheme);

        foreach (Step step in steps)
        {
            step.Theme = newTheme;
        }
    }

    private bool DoesThemeExist(long projectId, string themeName, bool isHeadTheme)
    {
        var theme = _themeRepository.ReadThemeByProjectIdAndThemeName(projectId, themeName, isHeadTheme);
        if (theme == null)
        {
            theme = _themeRepository.ReadThemeByProjectIdAndThemeName(projectId, themeName, !isHeadTheme);
        }

        return theme != null;
    }

    public bool Change(Theme theme, string shortInfo, string newThemeName, long projectId)
    {
        if (DoesThemeExist(projectId, newThemeName, theme.IsHeadTheme))
        {
            return false;
        }

        if (!String.IsNullOrEmpty(shortInfo))
        {
            theme.ShortInformation = shortInfo;
        }

        if (!String.IsNullOrEmpty(newThemeName))
        {
            theme.ThemeName = newThemeName;
        }

        ValidateTheme(theme);
        _themeRepository.UpdateTheme(theme);
        return true;
    }

    #endregion

    #region Project

    public IEnumerable<Project> GetAllProjects()
    {
        return _projectRepository.ReadAllProjects();
    }

    public Project GetProjectById(long id)
    {
        return _projectRepository.ReadProjectById(id);
    }

    public Project GetProjectByFlowId(long flowId)
    {
        IEnumerable<Project> projects = _projectRepository.ReadAllProjectsWithThemeAndFlows();
        foreach (var project in projects)
        {
            foreach (var flow in project.Flows)
            {
                if (flow.Id == flowId)
                {
                    return project;
                }
            }
        }

        return null;
    }

    public Project GetProjectByStepId(long stepId)
    {
        IEnumerable<Project> projects = _projectRepository.ReadAllProjectsWithThemeAndFlows();
        foreach (var project in projects)
        {
            foreach (var flow in project.Flows)
            {
                foreach (var step in flow.Steps)
                {
                    if (step.Id == stepId)
                    {
                        return project;
                    }
                }
            }
        }
        return null;
    }

    public Project GetProjectByRunningFlowId(long runningFlowId)
    {
        RunningFlow runningFlow = _flowManager.GetRunningFlowById(runningFlowId);
        return GetProjectByFlowId(runningFlow.CurrentFlow.Id);
    }

    public Project GetProjectWithThemeAndFlows(long id)
    {
        return _projectRepository.ReadProjectWithThemeAndFlows(id);
    }

    public Project GetProjectWithThemes(long id)
    {
        return _projectRepository.ReadProjectWithThemes(id);
    }

    public Project AddProject(string name, bool isActive, OrganizationUser organizationUser, string projectInformation)
    {
        var platform =
            _sharingplatformRepository.ReadSharingPlatformOfOrganizationUserWithAllProjects(organizationUser);
        Project project = new Project()
        {
            Name = name,
            IsActive = isActive,
            ProjectInformation = projectInformation
        };

        ValidationContext vc = new ValidationContext(project);
        List<ValidationResult> result = new List<ValidationResult>();
        bool isOk = Validator.TryValidateObject(project, vc, result,
            validateAllProperties: true);
        if (!isOk)
        {
            string errorMessage = "";
            foreach (ValidationResult vr in result)
            {
                errorMessage += vr.ErrorMessage + "\n";
            }

            throw new ValidationException(errorMessage);
        }

        _projectRepository.CreateProject(project);
        platform.ProjectsAssigned.Add(project);
        _sharingplatformRepository.UpdateSharingPlatform(platform);
        return project;
    }

    public void RemoveProject(long projectId)
    {
        Project project = _projectRepository.ReadProjectByIdWithFlowsAndProject(projectId);
        IEnumerable<Flow> flows = project.Flows;
        Theme theme = project.Theme;
        IEnumerable<Theme> subThemes = theme.Subthemes;
        
        var attendentUsers = _generalUserManager.GetAttendentUsersByProjectId(projectId);
        
        foreach (var attendentUser in attendentUsers)
        {
            _generalUserManager.ChangeAttendentUserProject(attendentUser, null);
        }
        
        foreach (Flow fl in flows)
        {
             _flowManager.RemoveFlow(fl.Id);
        }
        

        foreach (Theme subTheme in subThemes)
        {
            // Remove all comments of the subthemes
            RemoveCommentsOfTheme(subTheme);
            _themeRepository.DeleteTheme(subTheme);
        }
        RemoveCommentsOfTheme(theme);
        _themeRepository.DeleteTheme(theme);
        
        _projectRepository.DeleteProject(project);
    }

    public Theme ChangeTheme(long themaId, string newThemeName, string shortInformation, bool isHeadTheme)
    {
        Theme theme = _themeRepository.ReadThemeById(themaId);
        theme.ThemeName = newThemeName;
        theme.ShortInformation = shortInformation;
        _themeRepository.UpdateTheme(theme);
        return theme;
    }


    public Project ChangeProject(long projectId, string name, bool isActive, string projectInformation,
        string primaryColor,
        string font)
    {
        Project project = _projectRepository.ReadProjectByIdWithFlows(projectId);
        if (project != null)
        {
            project.Name = name;
            project.IsActive = isActive;
            project.ProjectInformation = projectInformation;
            project.PrimaryColor = primaryColor;
            project.Font = font;
            _projectRepository.Update(project);

            IEnumerable<Flow> flows = project.Flows;
            if (flows.Any())
            {
                State state = project.IsActive ? State.Open : State.Closed;
                foreach (Flow flow in flows)
                {
                    flow.State = state;
                    _flowRepository.UpdateFlow(flow);
                }
            }
            return project;
        }
        return null;
    }

    public Project ChangeProjectStatus(long projectId, bool isActive)
    {
        Project project = _projectRepository.ReadProjectByIdWithFlows(projectId);
        if (project != null)
        {
            project.IsActive = isActive;
            _projectRepository.Update(project);
            IEnumerable<Flow> flows = project.Flows;
            if (flows.Any())
            {
                State state = project.IsActive ? State.Open : State.Closed;
                foreach (Flow flow in flows)
                {
                    flow.State = state;
                    _flowRepository.UpdateFlow(flow);
                }
            }
            return project;
        }
        return null;
    }

    #endregion

    private void ValidateTheme(Theme theme)
    {
        List<ValidationResult> errors = new List<ValidationResult>();
        ValidationContext vc = new ValidationContext(theme);
        bool valid = Validator.TryValidateObject(theme, vc, errors, true);

        if (!valid)
        {
            StringBuilder builder = new StringBuilder();
            foreach (ValidationResult vResult in errors)
            {
                builder.Append(vResult.ErrorMessage + " at members: ");
                foreach (string member in vResult.MemberNames)
                {
                    builder.Append(member + ", ");
                }

                builder.Append('\n');
            }

            throw new ValidationException(builder.ToString());
        }
    }
}