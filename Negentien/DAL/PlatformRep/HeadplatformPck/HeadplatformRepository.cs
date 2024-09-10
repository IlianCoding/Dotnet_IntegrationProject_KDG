using Microsoft.EntityFrameworkCore;
using NT.BL.Domain.platformpck;
using NT.BL.Domain.Util;
using NT.DAL.EF;

namespace NT.DAL.PlatformRep.HeadplatformPck;

public class HeadplatformRepository : IHeadplatformRepository
{
    private readonly PhygitalDbContext _phygitalDbContext;

    public HeadplatformRepository(PhygitalDbContext phygitalDbContext)
    {
        _phygitalDbContext = phygitalDbContext ?? throw new ArgumentNullException(nameof(phygitalDbContext));
    }
    public Platform ReadPlatformWithAllSharingPlatforms(bool isHead)
    {
        return _phygitalDbContext.Platforms
            .Include(sub => sub.SharingPlatforms)
            .ThenInclude(proj => proj.ProjectsAssigned)
            .Include(shar => shar.SharingPlatforms)
            .ThenInclude(us => us.OrganizationMaintainer)
            .SingleOrDefault(head => head.IsHead == isHead);
    }
    public void UpdateHeadPlatform(Platform sharingPlatform)
    {
        var headPlatform = _phygitalDbContext.Platforms
            .Include(platform => platform.SharingPlatforms)
            .SingleOrDefault(head => head.IsHead == true);

        if (headPlatform != null && !headPlatform.SharingPlatforms.Contains(sharingPlatform))
        {
            headPlatform.SharingPlatforms.Add(sharingPlatform);
            _phygitalDbContext.SaveChanges();
        }
    }
    public void UpdatePlatform(Platform platform)
    {
        _phygitalDbContext.Platforms.Update(platform);
    }
    public HeadPlatformNumbers ReadHeadPlatformNumbers()
    {
        HeadPlatformNumbers headPlatformNumbers = new HeadPlatformNumbers()
        {
            AmountOfProjects = _phygitalDbContext.Projects.Count(),
            AmountOfSharingPlatforms = _phygitalDbContext.Platforms.Count(p => !p.IsHead)
        };

        headPlatformNumbers.AvgAmountOfFlowsPerProject =
            ReadAvgFlowsPerProject(headPlatformNumbers.AmountOfProjects);

        return headPlatformNumbers;
    }

    private double ReadAvgFlowsPerProject(int amountOfProjects)
    {
        if (amountOfProjects == 0)
        {
            return 0;
        }

        double amountOfFlows = _phygitalDbContext.Flows.Count();

        return amountOfFlows / amountOfProjects;
    }

    public IEnumerable<ProjectDataPerSharingPlatform> ReadTotalAndActiveProjectCountPerSharingPlatform()
    {
        return _phygitalDbContext.Platforms
            .Where(p => !p.IsHead)
            .Select(p => new ProjectDataPerSharingPlatform()
            {
                SharingPlatformName = p.PlatformName,
                TotalProjectCount = p.ProjectsAssigned.Count(),
                ActiveProjectCount = p.ProjectsAssigned.Count(project => project.IsActive)
            });
    }

    public IEnumerable<AvgFlowPerProjectPerSharingPlatformData> ReadAvgFlowPerProjectPerSharingPlatform()
    {
        return _phygitalDbContext.Platforms
            .Where(p => !p.IsHead)
            .Select(p => new AvgFlowPerProjectPerSharingPlatformData()
            {
                SharingPlatformName = p.PlatformName,
                AvgFlowsPerProject = !p.ProjectsAssigned.Any()
                    ? 0.0
                    : p.ProjectsAssigned.Average(project => project.Flows.Count())
            });
    }
}