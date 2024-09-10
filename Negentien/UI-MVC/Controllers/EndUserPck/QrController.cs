using System.Drawing;
using System.Drawing.Imaging;
using Microsoft.AspNetCore.Mvc;
using NT.BL.services;
using QRCoder;

namespace NT.UI.MVC.Controllers.EndUserPck;

public class QrController : Controller
{
    private CloudStorageService _cloudStorageService;

    public QrController(CloudStorageService cloudStorageService)
    {
        _cloudStorageService = cloudStorageService;
    }

    public IActionResult GenerateQrCode(string url)
    {
        if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
        {
            url = $"{Request.Scheme}://{Request.Host}{url}";
        }

        using (MemoryStream ms = new MemoryStream())
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            using (Bitmap qrCodeImage = qrCode.GetGraphic(20))
            {
                qrCodeImage.Save(ms, ImageFormat.Png);
                string fileName = $"qr_{Guid.NewGuid()}.png";
                string contentType = "image/png";

                return File(ms.ToArray(), "image/png", fileName);
            }
        }
    }
    [HttpGet]
    public Task<IActionResult> GenerateQrCodeWithBucket(string url)
    {
        if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
        {
            url = $"{Request.Scheme}://{Request.Host}{url}";
        }

        using (MemoryStream ms = new MemoryStream())
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            using (Bitmap qrCodeImage = qrCode.GetGraphic(20))
            {
                qrCodeImage.Save(ms, ImageFormat.Png);
            }

            ms.Position = 0;

            string fileName = $"qr_{Guid.NewGuid()}.png";
            string contentType = "image/png";

            string objectName = _cloudStorageService.UploadFileToBucket(ms, fileName, contentType);

            Task.Run(() => RemoveQrCodeAfterDelay(objectName, TimeSpan.FromMinutes(1)));

            string fileUrl = _cloudStorageService.GetMedia(objectName);

            if (fileUrl == null)
            {
                return Task.FromResult<IActionResult>(NotFound());
            }

            return Task.FromResult<IActionResult>(Ok(new { qrCodeUrl = fileUrl }));
        }
    }
    private async Task RemoveQrCodeAfterDelay(string objectName, TimeSpan delay)
    {
        await Task.Delay(delay);
        _cloudStorageService.RemoveMedia(objectName);
    }
}