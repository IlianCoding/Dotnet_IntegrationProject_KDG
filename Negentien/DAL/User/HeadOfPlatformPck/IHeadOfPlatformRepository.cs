using NT.BL.Domain.users;

namespace NT.DAL.User.HeadOfPlatformPck;

public interface IHeadOfPlatformRepository
{
    HeadOfPlatformUser ReadHeadOfPlatformUserByEmail(string userMail);
}