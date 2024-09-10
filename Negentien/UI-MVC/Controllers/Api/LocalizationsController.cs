using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace NT.UI.MVC.Controllers.Api;
[Route("api/[controller]")]
[ApiController]
public class LocalizationsController : ControllerBase
{
    [HttpGet("Nederlands")]
    public IActionResult Nederlands()
    {
        Response.Cookies.Append(
            CookieRequestCultureProvider.DefaultCookieName,
            CookieRequestCultureProvider.MakeCookieValue(new RequestCulture("nl-BE")),
            new CookieOptions(){Expires = DateTimeOffset.UtcNow.AddDays(31)});
        return RedirectToAction("Index", "Home");
    }
    
    [HttpGet("English")]
    public IActionResult English()
    {
        Response.Cookies.Append(
            CookieRequestCultureProvider.DefaultCookieName,
            CookieRequestCultureProvider.MakeCookieValue(new RequestCulture("en-GB")),
            new CookieOptions(){Expires = DateTimeOffset.UtcNow.AddDays(31)});
        return RedirectToAction("Index", "Home");
    }
    
    [HttpGet("Français")]
    public IActionResult Français()
    {
        Response.Cookies.Append(
            CookieRequestCultureProvider.DefaultCookieName,
            CookieRequestCultureProvider.MakeCookieValue(new RequestCulture("fr-Fr")),
            new CookieOptions(){Expires = DateTimeOffset.UtcNow.AddDays(31)});
        return RedirectToAction("Index", "Home");
    }
}