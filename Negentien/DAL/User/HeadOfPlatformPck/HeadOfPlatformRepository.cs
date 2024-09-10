using NT.BL.Domain.users;
using NT.DAL.EF;

namespace NT.DAL.User.HeadOfPlatformPck;

public class HeadOfPlatformRepository : IHeadOfPlatformRepository
{
    private readonly PhygitalDbContext _phygitalDbContext;

    public HeadOfPlatformRepository(PhygitalDbContext phygitalDbContext)
    {
        _phygitalDbContext = phygitalDbContext ?? throw new ArgumentNullException(nameof(phygitalDbContext));
    }

    public HeadOfPlatformUser ReadHeadOfPlatformUserByEmail(string userMail)
    {
        return _phygitalDbContext.HeadOfPlatformUsers
            .SingleOrDefault(mail => mail.Email == userMail);
    }
}