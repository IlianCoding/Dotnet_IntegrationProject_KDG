using Microsoft.EntityFrameworkCore;
using NT.BL.Domain.flowpck;
using NT.BL.Domain.projectpck;
using NT.DAL.EF;

namespace NT.DAL.ProjectRep.ProjectPck;

public class ProjectRepository : IProjectRepository
{
    private readonly PhygitalDbContext _phygitalDbContext;

    public ProjectRepository(PhygitalDbContext phygitalDbContext)
    {
        _phygitalDbContext = phygitalDbContext ?? throw new ArgumentNullException(nameof(phygitalDbContext));
    }
    public Project ReadProjectById(long id)
    {
        return _phygitalDbContext.Projects
            .Include(pr =>pr.Theme)
            .SingleOrDefault(pr => pr.Id == id);
    }

    public Project ReadProjectWithFlows(long id)
    {
        return _phygitalDbContext.Projects
            .Include(p => p.Flows)
            .SingleOrDefault(p => p.Id == id);
    }


    public Project ReadProjectByName(string assignedProjectName)
    {
        return _phygitalDbContext.Projects
            .SingleOrDefault(name => name.Name == assignedProjectName);
    }

    public IEnumerable<Project> ReadAllProjects()
    {
        return _phygitalDbContext.Projects
            .Include(pr => pr.Theme);
    }
    
    public Project ReadProjectWithThemeAndFlows(long id)
    {
        return _phygitalDbContext.Projects
            .Include(pr => pr.Flows)
            .ThenInclude(fl => fl.Steps)
            .Include(pr => pr.Theme)
            .ThenInclude(th => th.Subthemes)
            .SingleOrDefault(pr => pr.Id == id);
    }

    public IEnumerable<Project> ReadAllProjectsWithThemeAndFlows()
    {
        return _phygitalDbContext.Projects.Include(pr => pr.Flows)
            .ThenInclude(fl => fl.Steps).Include(pr => pr.Theme)
            .ThenInclude(th => th.Subthemes);
    }
    public Project ReadProjectWithThemes(long id)
    {
        return _phygitalDbContext.Projects
            .Include(p => p.Theme)
            .ThenInclude(t => t.Subthemes)
            .Include(p => p.Flows)
            .SingleOrDefault(p => p.Id == id);
    }

    public Project FindProjectByName(string assignedProjectName)
    {
        return _phygitalDbContext.Projects
            .SingleOrDefault(name => name.Name == assignedProjectName);
    }

    public void CreateProject(Project project)
    {
        _phygitalDbContext.Projects.Add(project);
        _phygitalDbContext.SaveChanges();
    }
    
    public void Update(Project project)
    {
        _phygitalDbContext.Projects.Update(project);
        _phygitalDbContext.SaveChanges();
    }
    public void DeleteProject(Project project)
    {
        _phygitalDbContext.Projects.Remove(project);
        _phygitalDbContext.SaveChanges();
    }
    public Theme ReadThemeById(long stepContentId)
    {
        return _phygitalDbContext.Themes
            .SingleOrDefault(th => th.Id == stepContentId);
    }

    public Project ReadProjectByIdWithFlows(long projectId)
    {
        return _phygitalDbContext.Projects
            .Include(pr =>pr.Flows)
            .SingleOrDefault(pr => pr.Id == projectId);
    }
    

    public Project ReadProjectByIdWithFlowsAndProject(long projectId)
    {
        return _phygitalDbContext.Projects
            .Include(p => p.Flows)
            .Include(p => p.Theme)
            .ThenInclude(th => th.Subthemes)
            .SingleOrDefault(pr => pr.Id == projectId);
    }

    public Project ReadProjectByIdWithAll(long projectId)
    {
        return _phygitalDbContext.Projects
            .Include(p => p.Flows)
            .ThenInclude(fl => fl.Steps)
            .ThenInclude(fl => fl.Theme)
            .ThenInclude(fl => fl.Comments)
            
            .Include(p => p.Flows)
            .ThenInclude(fl => fl.Steps)
            .ThenInclude(fl => fl.Content)
            
            .Include(p => p.Flows)
            .ThenInclude(fl => fl.RunningFlows)
            .ThenInclude(fl => fl.Sessions)
            
            .Include(p => p.Theme)
            .ThenInclude(p => p.Comments)
            .ThenInclude(p => p.SubComments)
            .SingleOrDefault(p => p.Id == projectId);
    }

    public Project ReadProjectByIdWithFlowsWithTheme(long projectId)
    {
        return _phygitalDbContext.Projects
            .Include(p => p.Flows)
            .Include(p => p.Theme)
            .SingleOrDefault(p => p.Id == projectId);
    }
}