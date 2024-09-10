using NT.BL.Domain.flowpck;
using NT.BL.Domain.platformpck;
using NT.BL.Domain.users;
using NT.BL.Domain.Util;

namespace NT.DAL.PlatformRep.SharingplatformPck;

public interface ISharingplatformRepository
{
    void CreateSharingPlatform(Platform platform);
    void UpdateSharingPlatform(Platform platform);
    Platform ReadSharingPlatform(long id);
    Platform ReadSharingPlatformOfOrganizationWithAllProjects(string organizationName);
    Platform ReadSharingPlatformOfOrganizationUserWithAllProjects(OrganizationUser organizationUser);
    void UpdateSharingPlatformMaintainers(OrganizationUser organizationUser, string organizationName);
    string ReadLogoByProjectId(long projectId);
    string ReadLogoByStepId(long stepId);
    SharingPlatformNumbers ReadSharingPlatformStatistics(long platformId);
    IEnumerable<Flow> ReadAllFlowsFromPlatform(long platformId);
    Platform ReadSharingPlatformOnly(long platformId);
    IEnumerable<Step> ReadStepsWithTheme( long projectId);
    IEnumerable<ProjectNameAndId> ReadProjectIds(long platformId);
    IEnumerable<FlowIdAndName> ReadFlowIds(long platformId);
}