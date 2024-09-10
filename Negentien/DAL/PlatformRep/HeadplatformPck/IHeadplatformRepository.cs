using NT.BL.Domain.platformpck;
using NT.BL.Domain.Util;

namespace NT.DAL.PlatformRep.HeadplatformPck;

public interface IHeadplatformRepository
{

    Platform ReadPlatformWithAllSharingPlatforms(bool isHead);
    void UpdateHeadPlatform(Platform headPlatform);
    void UpdatePlatform(Platform platform);
    public HeadPlatformNumbers ReadHeadPlatformNumbers();
    IEnumerable<ProjectDataPerSharingPlatform> ReadTotalAndActiveProjectCountPerSharingPlatform();
    IEnumerable<AvgFlowPerProjectPerSharingPlatformData> ReadAvgFlowPerProjectPerSharingPlatform();
}