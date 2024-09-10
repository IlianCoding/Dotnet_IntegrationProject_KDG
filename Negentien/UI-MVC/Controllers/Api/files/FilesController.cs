using Microsoft.AspNetCore.Mvc;
using NT.BL.services;

namespace NT.UI.MVC.Controllers.Api.files;

[ApiController]
[Route("/api/files")]
public class FilesController : ControllerBase
{
    private readonly CloudStorageService _cloudStorageService;
    private readonly ModerationService _moderationService;

    public FilesController(CloudStorageService cloudStorageService, ModerationService moderationService)
    {
        this._cloudStorageService = cloudStorageService;
        _moderationService = moderationService;
    }

    [HttpPost]
    public ActionResult UploadFile(IFormFile file)
    {
        using var memoryStream = new MemoryStream();
        file.CopyTo(memoryStream);
        var imageData = memoryStream.ToArray();
        var isGivenCommentMalicious = _moderationService.IsGivenUserImageMalicious(imageData);

        if (isGivenCommentMalicious.Result)
        {
            return BadRequest(
                "Your comment was flagged for inappropriate content. Please ensure your comment adheres to our community guidelines.");
        }
        else
        {
            var objectName = _cloudStorageService.UploadFileToBucket(memoryStream, file.FileName, file.ContentType);
            return Ok(new { objectName , file.ContentType });
        }
    }
    
    [HttpGet]
    public ActionResult GetMedia(string objectName)
    {
        var url = _cloudStorageService.GetMedia(objectName);

        if (url == null)
        {
            return BadRequest();
        }

        return Ok(new {url = url});
    }
    
}