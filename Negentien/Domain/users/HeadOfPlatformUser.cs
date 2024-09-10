using NT.BL.Domain.platformpck;

namespace NT.BL.Domain.users;

public class HeadOfPlatformUser : GeneralUser
{
    public ICollection<Platform> ControlledPlatforms { get; set; }
}