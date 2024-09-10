using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NT.BL.Domain.projectpck;
using NT.BL.Domain.users;
using NT.BL.Domain.Util;
using NT.BL.PlatformMngr;
using NT.BL.services;
using NT.BL.UserMngr;
using NT.UI.MVC.Models.UserViewModels;

namespace NT.UI.MVC.Controllers.OrganizationPck;

public class OrganizationController : Controller
{
    private readonly IGeneralUserManager _generalUserManager;
    private readonly IPlatformManager _platformManager;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly StatisticsService _statisticsService;

    public OrganizationController(IGeneralUserManager generalUserManager, IPlatformManager platformManager,
        UserManager<IdentityUser> userManager, StatisticsService statisticsService)
    {
        _generalUserManager = generalUserManager;
        _platformManager = platformManager;
        _userManager = userManager;
        _statisticsService = statisticsService;
    }

    [HttpGet]
    public IActionResult ProjectOversight()
    {
        var user = _userManager.GetUserAsync(User).Result;
        if (user != null && User.IsInRole(CustomIdentityConstants.Organization))
        {
            var organizationUser = _generalUserManager.GetOrganizationUserById(user.Id);
            var platform = _platformManager.GetSharingPlatformOfOrganizationUserWithAllProjects(organizationUser);
            return View(platform);
        }
        return Forbid();
    }

    [HttpGet]
    public IActionResult AttendentDetails()
    {
        var user = _userManager.GetUserAsync(User).Result;
        if (user != null)
        {
            if (User.IsInRole(CustomIdentityConstants.Organization))
            {
                var organizationUser = _generalUserManager.GetOrganizationUserById(user.Id);
                var attendentUsers = _generalUserManager.GetAllAttendentUsersFromOrganization(organizationUser.Organization);
                ViewBag.Projects = GetProjectsForReassignmentSelectItems();
                AttendentDetailsViewModel attendentViewModel = new AttendentDetailsViewModel()
                {
                    AttendentUsers = attendentUsers,
                    Projects = GetProjectsForReassignmentSelectItems()
                };
                return View(attendentViewModel);
            }
            return Forbid();
        }
        return NotFound();
    }

    [HttpGet]
    public IActionResult AddAttendentUser()
    {
        if (User.IsInRole(CustomIdentityConstants.Organization))
        {
            ViewBag.ProjectsSelectlistItems = GetProjectSelectListItems();
            return View();
        }
        return Forbid();
    }
    
    [HttpPost]
    public IActionResult AddAttendentUser(NewAttendentUserViewModel newAttendentUserViewModel)
    {
        if (User.IsInRole(CustomIdentityConstants.Organization))
        {
            var user = _userManager.GetUserAsync(User).Result;
            if (user != null)
            {
                var organization = _generalUserManager.GetOrganizationUserByEmail(user.Email).Organization;
                if (!ModelState.IsValid)
                {
                    ViewBag.ProjectsSelectlistItems = GetProjectSelectListItems();
                    return View(newAttendentUserViewModel);
                }

                _generalUserManager.AddAttendentUser(
                    newAttendentUserViewModel.FirstName,
                    newAttendentUserViewModel.LastName,
                    newAttendentUserViewModel.Email,
                    newAttendentUserViewModel.PhoneNumber,
                    newAttendentUserViewModel.BirthDate,
                    organization,
                    newAttendentUserViewModel.AssignedProjectName);
            }
            ModelState.Clear();
            return RedirectToAction("AttendentDetails", "Organization");
        }
        return Forbid();
    }
    
    [HttpGet]
    public IActionResult AddOrganizationMember()
    {
        if (User.IsInRole(CustomIdentityConstants.Organization))
        {
            return View();
        }
        return Forbid();
    }
    
    [HttpPost]
    public IActionResult AddOrganizationMember(NewOrganizationMemberViewModel newOrganizationMemberViewModel)
    {
        if (User.IsInRole(CustomIdentityConstants.Organization))
        {
            var user = _userManager.GetUserAsync(User).Result;
            if (user != null)
            {
                var organization = _generalUserManager.GetOrganizationUserByEmail(user.Email).Organization;
                if (!ModelState.IsValid)
                {
                    return View(newOrganizationMemberViewModel);
                }

                _generalUserManager.AddOrganizationUser(
                    newOrganizationMemberViewModel.FirstName,
                    newOrganizationMemberViewModel.LastName,
                    newOrganizationMemberViewModel.Email,
                    newOrganizationMemberViewModel.PhoneNumber,
                    newOrganizationMemberViewModel.Birthday,
                    organization);

                var organizationUser = _generalUserManager.GetOrganizationUserByEmail(newOrganizationMemberViewModel.Email);
                _platformManager.ChangeSharingPlatformMaintainers(organizationUser, organization);
                var projects =
                    _platformManager.GetSharingPlatformOfOrganizationWithAllProjects(organizationUser.Organization).ProjectsAssigned;
                if (projects != null)
                {
                    _generalUserManager.ChangeOrganizationUserProjects(organizationUser, projects);
                }
            }
            ModelState.Clear();
            return RedirectToAction("ProjectOversight", "Organization");
        }
        return Forbid();
    }
    
    private IEnumerable<SelectListItem> GetProjectSelectListItems()
    {
        var user = _userManager.GetUserAsync(User).Result;
        var organizationUser = _generalUserManager.GetOrganizationUserById(user.Id);
        var platformWithAllProjects = _platformManager.GetSharingPlatformOfOrganizationUserWithAllProjects(organizationUser);
        
        List<SelectListItem> projectList = new List<SelectListItem>();

        foreach (Project project in platformWithAllProjects.ProjectsAssigned)
        {
            projectList.Add(new SelectListItem(
                text: project.Name, value: project.Name));
        }
        
        return projectList;
    }
    
    private ICollection<Project> GetProjectsForReassignmentSelectItems()
    {
        ICollection<Project> projectItems = new List<Project>();
        var user = _userManager.GetUserAsync(User).Result;
        var organization = _generalUserManager.GetOrganizationUserByEmail(user.Email);
        var projects = _platformManager.GetSharingPlatformOfOrganizationUserWithAllProjects(organization).ProjectsAssigned;

        foreach (Project project in projects)
        {
            projectItems.Add(project);
        }

        return projectItems;
    }
    
    [HttpGet("Organization/OrganizationStatistics/{platformId}")]
    public IActionResult OrganizationStatistics(long platformId)
    {
        if (User.IsInRole(CustomIdentityConstants.Organization))
        {
            if (_platformManager.GetSharingPlatformOnly(platformId) == null)
            {
                return NotFound();
            }
        
            SharingPlatformNumbers sharingPlatformNumbers =
                _statisticsService.GetSharingPlatformStatistics(platformId);
      
            Console.WriteLine("shar == "+sharingPlatformNumbers);
            return View(sharingPlatformNumbers);
        }

        return Forbid();
    }
}