namespace NT.DAL.User.AnonymousUser;

public interface IAnonymousUserRepository
{
    IEnumerable<BL.Domain.users.AnonymousUser> ReadAllAnonymousUserWithSession();
    void DeleteAnonymousUsers(IEnumerable<BL.Domain.users.AnonymousUser> anonymousUsers);
}