using NT.BL.Domain.flowpck;
using NT.BL.Domain.projectpck;

namespace NT.DAL.ProjectRep.ProjectPck;

public interface IProjectRepository
{
    IEnumerable<Project> ReadAllProjects();
    Project ReadProjectById(long id);
    Project ReadProjectWithFlows(long id);
    Project ReadProjectByName(string assignedProjectName);
    Project ReadProjectWithThemes(long id);
    Project ReadProjectWithThemeAndFlows(long id);
    IEnumerable<Project> ReadAllProjectsWithThemeAndFlows();
    void CreateProject(Project project);
    void Update(Project project);
    Theme ReadThemeById(long stepContentId);
    Project ReadProjectByIdWithFlows(long projectId);
    void DeleteProject(Project project);
    Project ReadProjectByIdWithFlowsAndProject(long projectId);
    Project ReadProjectByIdWithAll(long projectId);
}