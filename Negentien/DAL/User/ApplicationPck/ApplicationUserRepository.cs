using Microsoft.EntityFrameworkCore;
using NT.BL.Domain.flowpck;
using NT.BL.Domain.users;
using NT.DAL.EF;

namespace NT.DAL.User.ApplicationPck;

public class ApplicationUserRepository : IApplicationUserRepository
{
    private readonly PhygitalDbContext _phygitalDbContext;

    public ApplicationUserRepository(PhygitalDbContext phygitalDbContext)
    {
        _phygitalDbContext = phygitalDbContext ?? throw new ArgumentNullException(nameof(phygitalDbContext));
    }
    public ApplicationUser ReadApplicationUserByEmail(string userMail)
    {
        return _phygitalDbContext.ApplicationUsers
            .Include(u => u.LikedComments)
            .SingleOrDefault(email => email.Email == userMail);
    }

    public IEnumerable<ApplicationUser> ReadAllApplicationUserWithCommentWithSession()
    {
        return _phygitalDbContext.ApplicationUsers
            .Include(app => app.LikedComments)
            .Include(app => app.Sessions);
    }

    public void UpdateApplicationUser(ApplicationUser user)
    {
        _phygitalDbContext.ApplicationUsers.Update(user);
        _phygitalDbContext.SaveChanges();
    }

}