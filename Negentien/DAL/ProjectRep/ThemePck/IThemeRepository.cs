using NT.BL.Domain.flowpck;

namespace NT.DAL.ProjectRep.ThemePck;

public interface IThemeRepository
{
    Theme ReadThemeById(long id);
    Theme ReadHeadThemeByProjectId(long projectId);
    Theme ReadThemeByName(string chosenTheme);
    Theme ReadThemeByProjectIdAndThemeName(long projectId, string themeName,bool isHeadTheme);
    Theme ReadThemeWithComments(long themeId);
    IEnumerable<Theme> ReadAllHeadThemesWithSubThemes();
    IEnumerable<Theme> ReadAllThemes();
    IEnumerable<string> ReadThemesByProjectId(long projectId);
    
    void UpdateTheme(Theme theme);
    void CreateTheme(Theme theme);
    void DeleteTheme(Theme theme);

}