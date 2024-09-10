using Microsoft.EntityFrameworkCore;
using NT.BL.Domain.flowpck;
using NT.BL.Domain.platformpck;
using NT.BL.Domain.users;
using NT.BL.Domain.Util;
using NT.DAL.EF;

namespace NT.DAL.PlatformRep.SharingplatformPck;

public class SharingplatformRepository : ISharingplatformRepository
{
    private readonly PhygitalDbContext _phygitalDbContext;

    public SharingplatformRepository(PhygitalDbContext phygitalDbContext)
    {
        _phygitalDbContext = phygitalDbContext ?? throw new ArgumentNullException(nameof(phygitalDbContext));
    }
    public void CreateSharingPlatform(Platform platform)
    {
        _phygitalDbContext.Platforms.Add(platform);
        _phygitalDbContext.SaveChanges();
    }

    public void UpdateSharingPlatform(Platform platform)
    {
        var oldPlatform = _phygitalDbContext.Platforms
            .SingleOrDefault(plt => plt.Id == platform.Id);
        if (oldPlatform != null) oldPlatform.ProjectsAssigned = platform.ProjectsAssigned;
        _phygitalDbContext.SaveChanges();
    }
    public Platform ReadSharingPlatform(long id)
    {
        return _phygitalDbContext.Platforms
            .Include(user => user.OrganizationMaintainer)
            .ThenInclude(user => user.OwnedProjects)
            .SingleOrDefault(platform => platform.Id == id);
    }

    public Platform ReadSharingPlatformOfOrganizationWithAllProjects(string organizationName)
    {
        return _phygitalDbContext.Platforms
            .Include(them => them.ProjectsAssigned)
            .SingleOrDefault(plt => plt.PlatformName.ToUpper() == organizationName.ToUpper());
    }

    public Platform ReadSharingPlatformOfOrganizationUserWithAllProjects(OrganizationUser organizationUser)
    {
        return _phygitalDbContext.Platforms
            .Include(user => user.OrganizationMaintainer)
            .Include(them => them.ProjectsAssigned)
            .ThenInclude(them => them.Theme)
            .Include(plat => plat.ProjectsAssigned)
            .ThenInclude(flo => flo.Flows)
            .SingleOrDefault(plt => plt.OrganizationMaintainer.Contains(organizationUser));
    }
    public void UpdateSharingPlatformMaintainers(OrganizationUser organizationUser, string organizationName)
    {
        var platform = _phygitalDbContext.Platforms
            .Include(maintainer => maintainer.OrganizationMaintainer)
            .SingleOrDefault(plt => plt.PlatformName.ToUpper() == organizationName.ToUpper());
        
        if (platform != null)
        {
            platform.OrganizationMaintainer.Add(organizationUser);
            _phygitalDbContext.SaveChanges();
        }
    }

    public string ReadLogoByProjectId(long projectId)
    {
        return _phygitalDbContext.Platforms
            .SingleOrDefault(platform => platform.ProjectsAssigned.Any(project => project.Id == projectId))
            ?.LogoObjectName;
    }

    public string ReadLogoByStepId(long stepId)
    {
        return _phygitalDbContext.Platforms
            .SingleOrDefault(platform => platform.ProjectsAssigned
                .Any(p => p.Flows
                    .Any(f =>f.Steps
                        .Any(s => s.Id == stepId))))
            ?.LogoObjectName;
    }

    public SharingPlatformNumbers ReadSharingPlatformStatistics(long platformId)
    {
        SharingPlatformNumbers platformNumbers =
            _phygitalDbContext.Platforms
                .Where(p => p.Id == platformId)
                .Select(platform => new SharingPlatformNumbers()
                    {
                        TotalProjects = platform.ProjectsAssigned.Count,

                        TotalFlows = platform.ProjectsAssigned.Sum(project => project.Flows.Count),

                        AvgAmountOfStepsPerFlow = platform.ProjectsAssigned
                            .Sum(project => project.Flows
                                .Sum(f => f.Steps.Count)),

                        AvgParticipantsPerFlow =
                            platform.ProjectsAssigned
                                .Sum(project => project.Flows
                                    .Sum(flow => flow.RunningFlows
                                        .Sum(rf => rf.Sessions.Count)))
                    }
                ).SingleOrDefault();

        if (platformNumbers == null)
        {
            return null;
        }

        if (platformNumbers.TotalFlows == 0)
        {
            return platformNumbers;
        }

        platformNumbers.AvgAmountOfStepsPerFlow /= platformNumbers.TotalFlows;
        platformNumbers.AvgParticipantsPerFlow /= platformNumbers.TotalFlows;

        return platformNumbers;
    }

    public IEnumerable<Flow> ReadAllFlowsFromPlatform(long platformId)
    {
        return _phygitalDbContext.Platforms
            .Where(pl => pl.Id == platformId)
            .SelectMany(pl => pl.ProjectsAssigned.SelectMany(pr => pr.Flows)
            ).Include(flow => flow.RunningFlows)
            .ThenInclude(rf => rf.Sessions);
    }

    public Platform ReadSharingPlatformOnly(long platformId)
    {
        return _phygitalDbContext.Platforms
            .SingleOrDefault(platform => platform.Id == platformId && !platform.IsHead);
    }

    public IEnumerable<Step> ReadStepsWithTheme(long projectId)
    {
        return _phygitalDbContext.Projects
            .Where(p => p.Id == projectId)
            .SelectMany(p => p.Flows.SelectMany(f => f.Steps))
            .Include(st => st.Theme);
    }

    public IEnumerable<ProjectNameAndId> ReadProjectIds(long platformId)
    {
        return _phygitalDbContext.Platforms.Where(p => p.Id == platformId)
            .Select(pl => pl.ProjectsAssigned.Select(p => new ProjectNameAndId()
            {
                Name = p.Name,
                Id = p.Id
            }))
            .SingleOrDefault();
    }

    public IEnumerable<FlowIdAndName> ReadFlowIds(long platformId)
    {
      
        return _phygitalDbContext.Platforms
            .Where(p => p.Id == platformId)
            .SelectMany(pl => pl.ProjectsAssigned
                .SelectMany(p => p.Flows
                    .Select(fl => new FlowIdAndName()
                    {
                        Name = fl.FlowName,
                        Id = fl.Id
                    })
                )
            );
        ;
    }
}