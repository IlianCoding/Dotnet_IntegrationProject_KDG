using NT.BL.Domain.flowpck;
using NT.BL.Domain.webplatformpck;

namespace NT.DAL.WebPlatformRep;

public interface ICommentRepository
{
    Comment ReadComment(long commentId);
    Comment CreateComment(Comment comment); 
    Comment UpdateComment(Comment comment);
    LikedComment CreateLikedComment(LikedComment likedComment);
    LikedComment ReadLikedComment(long commentId);
    void DeleteLikedComment(LikedComment likedComment);
    public void DeleteCommentsOfTheme(Theme theme);

}