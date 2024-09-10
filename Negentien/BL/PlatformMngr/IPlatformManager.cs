using NT.BL.Domain.platformpck;
using NT.BL.Domain.users;
using NT.BL.Domain.Util;

namespace NT.BL.PlatformMngr;

public interface IPlatformManager
{
    Platform GetPlatformWithAllSharingPlatforms(bool isHead);
    Platform GetSharingPlatform(long id);
    Platform GetSharingPlatformOfOrganizationWithAllProjects(string organizationName);
    Platform GetSharingPlatformOfOrganizationUserWithAllProjects(OrganizationUser organizationUser);
    void AddSharingPlatform(string platformName, OrganizationUser organizationMaintainer);
    void ChangeSharingPlatformMaintainers(OrganizationUser organizationUser, string organization);
    void ChangeLogo(string objectName, Platform platformToBeChanged);
    string GetLogoByProjectId(long projectId);
    string GetLogoByStepId(long stepId);
    public Platform GetSharingPlatformOnly(long platformId);
}