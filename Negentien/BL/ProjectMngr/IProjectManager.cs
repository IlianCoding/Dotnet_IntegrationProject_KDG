using NT.BL.Domain.flowpck;
using NT.BL.Domain.projectpck;
using NT.BL.Domain.users;

namespace NT.BL.ProjectMngr;

public interface IProjectManager
{
    IEnumerable<Theme> GetAllHeadThemes();
    Theme GetTheme(long projectId, string chosenTheme, bool isHeadTheme);
    Theme GetThemeById(long stepContentId);
    Theme GetThemeWithComments(long themeId);
    Theme AddThemeToProject(string shortInfo, string themeName, bool isHeadTheme, long projectId);
    bool Change(Theme theme, string shortInfo, string newThemeName, long projectId);
    void RemoveTheme(Theme theme, long projectId);
    IEnumerable<Project> GetAllProjects();
    Project GetProjectById(long id);
    Project GetProjectByFlowId(long flowId);
    Project GetProjectByStepId(long stepId);
    Project GetProjectByRunningFlowId(long runningFlowId);
    Project GetProjectWithThemes(long id);
    Project GetProjectWithThemeAndFlows(long id);
    Project AddProject(string name, bool isActive, OrganizationUser organizationUser, string projectInformation);
    Project ChangeProject(long projectId, string name, bool isActive, string projectInformation, string primaryColor, string font);
    Project ChangeProjectStatus(long projectId, bool isActive);
    void RemoveProject(long projectId);
    Theme ChangeTheme(long themaId, string newThemeName, string shortInformation, bool isHeadTheme);
}