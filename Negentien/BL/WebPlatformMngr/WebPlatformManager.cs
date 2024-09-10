using System.Collections;
using NT.BL.Domain.flowpck;
using NT.BL.Domain.users;
using NT.BL.Domain.webplatformpck;
using NT.BL.ProjectMngr;
using NT.BL.UserMngr;
using NT.DAL.WebPlatformRep;

namespace NT.BL.WebPlatformMngr;

public class WebPlatformManager : IWebPlatformManager
{
    private readonly ICommentRepository _commentRepository;
    private readonly IProjectManager _projectManager;
    private readonly IGeneralUserManager _generalUserManager;
    
    public WebPlatformManager(ICommentRepository commentRepository, IProjectManager projectManager, IGeneralUserManager generalUserManager)
    {
        _commentRepository = commentRepository;
        _projectManager = projectManager;
        _generalUserManager = generalUserManager;
    }


    public ICollection<Comment> GetCommentsOfTheme(long themeId)
    {
        Theme theme = _projectManager.GetThemeWithComments(themeId);
        if (theme == null)
        {
            return null;
        }

        return theme.Comments;

    }

    public Comment AddComment(long themeId, string commentText, ApplicationUser user, string url)
    {
        Theme theme = _projectManager.GetThemeWithComments(themeId);
        if (theme == null)
        {
            return null;
        }
        
        Comment comment = new Comment
        {
            Text = commentText,
            User = user,
            SubComments = new List<Comment>(),
            ObjectName = url
        };

        theme.Comments.Add(comment);
        _commentRepository.CreateComment(comment);
        return comment;
    }

    public Comment AddReply(long commentId, string text, ApplicationUser user)
    {
        Comment comment = _commentRepository.ReadComment(commentId);
        if (comment == null)
        {
            throw new Exception("Comment not found");
        }

        Comment reply = new Comment
        {
            Text = text,
            User = user
        };
        
        comment.SubComments.Add(reply);
        _commentRepository.CreateComment(reply);
        return reply;
    }

    public Comment LikeComment(long commentId)
    {
        Comment comment = _commentRepository.ReadComment(commentId);
        if (comment == null)
        {
            throw new Exception("Comment not found");
        }

        comment.Likes++;
        return _commentRepository.UpdateComment(comment);
    }
    
    public Comment UnlikeComment(long commentId)
    {
        Comment comment = _commentRepository.ReadComment(commentId);
        if (comment == null)
        {
            throw new Exception("Comment not found");
        }

        comment.Likes--;
        return _commentRepository.UpdateComment(comment);
    }

    public void AddLikedComment(long commentId, ApplicationUser user)
    {
        LikedComment likedComment = new LikedComment
        {
            CommentId = commentId,
        };
        
        _commentRepository.CreateLikedComment(likedComment);

        _generalUserManager.AddLikedCommentToApplicationUser(likedComment, user);
    }

    public void RemoveLikedComment(long commentId, ApplicationUser user)
    {
        LikedComment likedComment = _commentRepository.ReadLikedComment(commentId);
        if (likedComment == null)
        {
            throw new Exception("LikedComment not found");
        }

        _commentRepository.DeleteLikedComment(likedComment);

        _generalUserManager.RemoveLikedCommentFromApplicationUser(likedComment, user);
    }
}