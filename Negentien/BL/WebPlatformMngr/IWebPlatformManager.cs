using NT.BL.Domain.users;
using NT.BL.Domain.webplatformpck;

namespace NT.BL.WebPlatformMngr;

public interface IWebPlatformManager
{
    ICollection<Comment> GetCommentsOfTheme(long themeId);
    Comment AddComment(long themeId, string commentText, ApplicationUser user, string url);
    Comment AddReply(long commentId, string text, ApplicationUser user);
    Comment LikeComment(long commentId);
    Comment UnlikeComment(long commentId);
    void AddLikedComment(long commentId, ApplicationUser user);
    void RemoveLikedComment(long commentId, ApplicationUser user);
}