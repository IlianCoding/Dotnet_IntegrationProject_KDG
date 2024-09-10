using Microsoft.EntityFrameworkCore;
using NT.DAL.EF;

namespace NT.DAL.User.AnonymousUser;

public class AnonymousUserRepository : IAnonymousUserRepository
{
    private readonly PhygitalDbContext _phygitalDbContext;

    public AnonymousUserRepository(PhygitalDbContext phygitalDbContext)
    {
        _phygitalDbContext = phygitalDbContext ?? throw new ArgumentNullException(nameof(phygitalDbContext));
    }

    public IEnumerable<BL.Domain.users.AnonymousUser> ReadAllAnonymousUserWithSession()
    {
        return _phygitalDbContext.AnonymousUsers
            .Include(an => an.Session);
    }

    public void DeleteAnonymousUsers(IEnumerable<BL.Domain.users.AnonymousUser> anonymousUsers)
    {
        _phygitalDbContext.AnonymousUsers.RemoveRange(anonymousUsers);
        _phygitalDbContext.SaveChanges();
    }
}