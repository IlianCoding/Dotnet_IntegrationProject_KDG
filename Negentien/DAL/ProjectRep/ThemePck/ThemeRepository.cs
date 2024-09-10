using Microsoft.EntityFrameworkCore;
using NT.BL.Domain.flowpck;
using NT.DAL.EF;
using NT.DAL.StepRep.StepPck;

namespace NT.DAL.ProjectRep.ThemePck;

public class ThemeRepository : IThemeRepository
{
    private readonly PhygitalDbContext _phygitalDbContext;

    public ThemeRepository(PhygitalDbContext phygitalDbContext)
    {
        _phygitalDbContext = phygitalDbContext ?? throw new ArgumentNullException(nameof(phygitalDbContext));
    }
    public Theme ReadThemeById(long themeId)
    {
        return _phygitalDbContext.Themes
            .SingleOrDefault(th => th.Id == themeId);
    }

    public Theme ReadHeadThemeByProjectId(long projectId)
    {
        return _phygitalDbContext.Projects
            .Where(p => p.Id == projectId)
            .Select(p => p.Theme)
            .SingleOrDefault();
    }

    public Theme ReadThemeByName(string chosenTheme)
    {
        return _phygitalDbContext.Themes
            .Include(t => t.Subthemes)
            .SingleOrDefault(t => t.ThemeName == chosenTheme);
    }

    public Theme ReadThemeByProjectIdAndThemeName(long projectId, string themeName, bool isHeadTheme)
    {
        if (isHeadTheme)
        {
            return _phygitalDbContext.Projects
                .Where(p => p.Id == projectId)
                .Select(p => p.Theme)
                .SingleOrDefault(t => t.ThemeName == themeName);
        }


        return _phygitalDbContext.Projects
            .Where(p => p.Id == projectId)
            .SelectMany(p => p.Theme.Subthemes)
            .SingleOrDefault(t => t.ThemeName == themeName);
    }

    public Theme ReadThemeWithComments(long themeId)
    {
        return _phygitalDbContext.Themes
            .Include(theme => theme.Comments)
            .ThenInclude(comment => comment.User)
            .Include(theme => theme.Comments)
            .ThenInclude(comment => comment.SubComments)
            .ThenInclude(subcomment => subcomment.User)
            .SingleOrDefault(theme => theme.Id == themeId);
    }

    public IEnumerable<Theme> ReadAllHeadThemesWithSubThemes()
    {
        return _phygitalDbContext.Themes
            .Include(theme => theme.Subthemes)
            .Where(theme => theme.Subthemes != null && theme.IsHeadTheme == true);
    }

    public IEnumerable<Theme> ReadAllThemes()
    {
        return _phygitalDbContext.Themes
            .Include(theme => theme.Subthemes);
    }

    public IEnumerable<string> ReadThemesByProjectId(long projectId)
    {
        ICollection<string> names = new List<string>();
        var project = _phygitalDbContext.Projects
            .Where(pro => pro.Id == projectId)
            .Include(th => th.Theme) // Include the HeadTheme navigation property within Theme
            .ThenInclude(th => th.Subthemes) 
            .SingleOrDefault();
        if (project == null )
        {
            return null;
        }

        names = project.Theme.Subthemes.Select(sub => sub.ThemeName).ToList();
        names.Add(project.Theme.ThemeName);
        return names;
    }

    public void UpdateTheme(Theme theme)
    {
        _phygitalDbContext.Themes.Update(theme);
    }

    public void CreateTheme(Theme theme)
    {
        _phygitalDbContext.Themes.Add(theme);
        _phygitalDbContext.SaveChanges();
    }

    public void DeleteTheme(Theme theme)
    {
        _phygitalDbContext.Themes.Remove(theme);
        _phygitalDbContext.SaveChanges();
    }
}