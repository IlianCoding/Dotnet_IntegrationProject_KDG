using NT.BL.Domain.flowpck;
using NT.BL.Domain.users;

namespace NT.DAL.User.ApplicationPck;

public interface IApplicationUserRepository
{
    ApplicationUser ReadApplicationUserByEmail(string userMail);
    IEnumerable<ApplicationUser> ReadAllApplicationUserWithCommentWithSession();
    void UpdateApplicationUser(ApplicationUser user);
}