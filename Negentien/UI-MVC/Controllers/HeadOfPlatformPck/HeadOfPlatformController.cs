using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NT.BL.Domain.platformpck;
using NT.BL.Domain.users;
using NT.BL.Domain.Util;
using NT.BL.PlatformMngr;
using NT.BL.services;
using NT.BL.UserMngr;
using NT.UI.MVC.Models;
using NT.UI.MVC.Models.UserViewModels;

namespace NT.UI.MVC.Controllers.HeadOfPlatformPck;

public class HeadOfPlatformController : Controller
{
    private readonly IPlatformManager _platformManager;
    private readonly IGeneralUserManager _generalUserManager;
    private readonly StatisticsService _statisticsService;

    public HeadOfPlatformController(IPlatformManager platformManager, 
        IGeneralUserManager generalUserManager,
        StatisticsService statisticsService)
    {
        _platformManager = platformManager;
        _generalUserManager = generalUserManager;
        _statisticsService = statisticsService;
    }
    
    [HttpGet]
    public IActionResult Oversight()
    {
        if (User.IsInRole(CustomIdentityConstants.HeadOfPlatform))
        {
            ICollection<Platform> platformOversight = _platformManager.GetPlatformWithAllSharingPlatforms(true).SharingPlatforms;
            return View(platformOversight);
        }

        return Forbid();
    }
    
    [HttpGet]
    public IActionResult Details(long id)
    {
        if (User.IsInRole(CustomIdentityConstants.HeadOfPlatform))
        {
            Platform platformDetails = _platformManager.GetSharingPlatform(id);
            return View(platformDetails);
        }

        return Forbid();
    }
    
    [HttpGet]
    public IActionResult Add()
    {
        if (User.IsInRole(CustomIdentityConstants.HeadOfPlatform))
        {
            return View();
        }

        return Forbid();
    }

    [HttpPost]
    public async Task<IActionResult> Add(NewSharingPlatformViewModel newSharingPlatformViewModel)
    {
        if (User.IsInRole(CustomIdentityConstants.HeadOfPlatform))
        {
            if (!ModelState.IsValid)
            {
                return View(newSharingPlatformViewModel);
            }

            await _generalUserManager.AddOrganizationUser(newSharingPlatformViewModel.FirstName,
                newSharingPlatformViewModel.LastName,
                newSharingPlatformViewModel.Email, newSharingPlatformViewModel.PhoneNumber,
                newSharingPlatformViewModel.Birthday,
                newSharingPlatformViewModel.Organization);

            var organizationMaintainer =
                _generalUserManager.GetOrganizationUserByEmail(newSharingPlatformViewModel.Email);
            _platformManager.AddSharingPlatform(newSharingPlatformViewModel.Organization, organizationMaintainer);
            ModelState.Clear();
            return RedirectToAction("Oversight", "HeadOfPlatform");
        }

        return Forbid();
    }

    [HttpGet]
    public IActionResult HeadPlatformStatistics()
    {
        if (User.IsInRole(CustomIdentityConstants.HeadOfPlatform))
        {
            HeadPlatformNumbers headPlatformNumbers =
                _statisticsService.GetHeadPlatformStatistics();
            return View(headPlatformNumbers);
        }

        return Forbid();
    }

    public IActionResult PrivacyDeclaration()
    {
        return View();
    }
}