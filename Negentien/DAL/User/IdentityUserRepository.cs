using Microsoft.AspNetCore.Identity;
using NT.DAL.EF;

namespace NT.DAL.User;

public class IdentityUserRepository : IIdentityUserRepository
{
    private readonly PhygitalDbContext _phygitalDbContext;

    public IdentityUserRepository(PhygitalDbContext phygitalDbContext)
    {
        _phygitalDbContext = phygitalDbContext;
    }

    public IdentityUser ReadIdentityUserByEmail(string userMail)
    {
        return _phygitalDbContext.Users
            .SingleOrDefault(mail => mail.Email == userMail);
    }
}