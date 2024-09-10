using Microsoft.EntityFrameworkCore;
using NT.BL.Domain.flowpck;
using NT.BL.Domain.webplatformpck;
using NT.DAL.EF;

namespace NT.DAL.WebPlatformRep;

public class CommentRepository: ICommentRepository
{
    private readonly PhygitalDbContext _phygitalDbContext;
    
    public CommentRepository(PhygitalDbContext phygitalDbContext)
    {
        _phygitalDbContext = phygitalDbContext;
    }


    public Comment ReadComment(long commentId)
    {
        return _phygitalDbContext.Comments
            .Include(c => c.SubComments)
            .SingleOrDefault(comment => comment.Id == commentId);
    }

    public Comment CreateComment(Comment comment)
    {
        _phygitalDbContext.Comments.Add(comment);
        _phygitalDbContext.SaveChanges();
        return comment;
    }

    public Comment UpdateComment(Comment comment)
    {
        _phygitalDbContext.Comments.Update(comment);
        _phygitalDbContext.SaveChanges();
        return comment;
    }

    public LikedComment CreateLikedComment(LikedComment likedComment)
    {
        _phygitalDbContext.LikedComments.Add(likedComment);
        _phygitalDbContext.SaveChanges();
        return likedComment;
    }

    public LikedComment ReadLikedComment(long commentId)
    {
        return _phygitalDbContext.LikedComments
            .SingleOrDefault(lc => lc.CommentId == commentId);
    }

    public void DeleteLikedComment(LikedComment likedComment)
    {
        _phygitalDbContext.LikedComments.Remove(likedComment);
        _phygitalDbContext.SaveChanges();
    }

    public void DeleteCommentsOfTheme(Theme theme)
    {
        _phygitalDbContext.Comments.RemoveRange(theme.Comments);
    }
}