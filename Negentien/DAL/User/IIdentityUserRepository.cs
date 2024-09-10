using Microsoft.AspNetCore.Identity;

namespace NT.DAL.User;

public interface IIdentityUserRepository
{
    IdentityUser ReadIdentityUserByEmail(string userMail);
}