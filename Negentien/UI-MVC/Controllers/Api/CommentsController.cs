using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NT.BL.Domain.webplatformpck;
using NT.BL.services;
using NT.BL.UserMngr;
using NT.BL.WebPlatformMngr;
using NT.UI.MVC.Models.Dto;

namespace NT.UI.MVC.Controllers.Api;

[ApiController]
[Route("api/[controller]")]
public class CommentsController : ControllerBase
{
    private readonly IWebPlatformManager _webPlatformManager;
    private readonly UserManager<IdentityUser> _identityUserManager;
    private readonly IGeneralUserManager _generalUserManager;
    private readonly ModerationService _moderationService;

    public CommentsController(IWebPlatformManager webPlatformManager, UserManager<IdentityUser> identityUserManager,
        IGeneralUserManager generalUserManager, ModerationService moderationService)
    {
        _webPlatformManager = webPlatformManager;
        _identityUserManager = identityUserManager;
        _generalUserManager = generalUserManager;
        _moderationService = moderationService;
    }


    [HttpGet("/api/Comments/GetComments/{themeId}")]
    public ActionResult GetComments(long themeId)
    {
        ICollection<Comment> comments = _webPlatformManager.GetCommentsOfTheme(themeId);
        if (comments == null)
        {
            return NotFound();
        }

        var userAsync = _identityUserManager.GetUserAsync(User).Result;
        var user = _generalUserManager.GetApplicationUserByEmail(userAsync?.Email);
        var likedComments = user?.LikedComments;

        var commentsWithLikes = comments.Select(comment => new
        {
            comment.Id,
            comment.Text,
            comment.User,
            comment.Likes,
            comment.ObjectName,
            SubComments = comment.SubComments.Select(subComment => new
            {
                subComment.Id,
                subComment.Text,
                subComment.User,
                subComment.Likes,
                IsLikedByCurrentUser = likedComments?.Any(lc => lc.CommentId == subComment.Id) ?? false
            }),
            IsLikedByCurrentUser = likedComments?.Any(lc => lc.CommentId == comment.Id) ?? false
        });

        return Ok(commentsWithLikes);
    }

    [HttpPost("/api/Comments/AddComment")]
    public ActionResult AddComment([FromBody] NewCommentDto commentDto)
    {
        var userAsync = _identityUserManager.GetUserAsync(User).Result;
        var user = _generalUserManager.GetApplicationUserByEmail(userAsync?.Email);
        var isGivenCommentMalicious = _moderationService.IsGivenUserInputMalicious(commentDto.Text);

        if (isGivenCommentMalicious)
        {
            return BadRequest(
                "Your comment was flagged for inappropriate content. Please ensure your comment adheres to our community guidelines.");
        }
        else
        {
            Comment comment = _webPlatformManager.AddComment(commentDto.ThemeId, commentDto.Text, user, commentDto.Url);
            return Ok(comment);
        }
    }

    [HttpPost("/api/Comments/AddReply")]
    public ActionResult AddReply([FromBody] NewReplyDto replyDto)
    {
        var userAsync = _identityUserManager.GetUserAsync(User).Result;
        var user = _generalUserManager.GetApplicationUserByEmail(userAsync?.Email);
        var isGivenCommentMalicious = _moderationService.IsGivenUserInputMalicious(replyDto.Text);

        if (isGivenCommentMalicious)
        {
            return BadRequest(
                "Your comment was flagged for inappropriate content. Please ensure your comment adheres to our community guidelines.");
        }
        else
        {
            Comment comment = _webPlatformManager.AddReply(replyDto.CommentId, replyDto.Text, user);
            return Ok(comment);
        }
    }

    [HttpPut("/api/Comments/LikeComment/{commentId}")]
    public ActionResult LikeComment(long commentId)
    {
        Comment comment = _webPlatformManager.LikeComment(commentId);
        
        var userAsync = _identityUserManager.GetUserAsync(User).Result;
        var user = _generalUserManager.GetApplicationUserByEmail(userAsync?.Email);

        if (user != null)
        {
            _webPlatformManager.AddLikedComment(commentId, user);
        }
        
        return Ok(comment);
    }

    [HttpPut("/api/Comments/UnlikeComment/{commentId}")]
    public ActionResult UnlikeComment(long commentId)
    {
        Comment comment = _webPlatformManager.UnlikeComment(commentId);
        
        var userAsync = _identityUserManager.GetUserAsync(User).Result;
        var user = _generalUserManager.GetApplicationUserByEmail(userAsync?.Email);
        
        if (user != null)
        {
            _webPlatformManager.RemoveLikedComment(commentId, user);
        }
        return Ok(comment);
    }
}