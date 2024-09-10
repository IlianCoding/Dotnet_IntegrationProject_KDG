using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using NT.BL.Domain.flowpck;
using NT.BL.Domain.projectpck;
using NT.BL.Domain.users;
using NT.BL.Domain.webplatformpck;
using NT.BL.services;
using NT.DAL.PlatformRep.SharingplatformPck;
using NT.DAL.ProjectRep.ProjectPck;
using NT.DAL.SessionRep.SessionPck;
using NT.DAL.User;
using NT.DAL.User.AnonymousUser;
using NT.DAL.User.ApplicationPck;
using NT.DAL.User.AttendentPck;
using NT.DAL.User.HeadOfPlatformPck;
using NT.DAL.User.OrganizationPck;

namespace NT.BL.UserMngr;

public class GeneralUserManager : IGeneralUserManager
{
    private readonly IApplicationUserRepository _applicationUserRepository;
    private readonly IAttendentUserRepository _attendentUserRepository;
    private readonly IOrganizationRepository _organizationRepository;
    private readonly IHeadOfPlatformRepository _headOfPlatformRepository;
    private readonly IIdentityUserRepository _identityUserRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly ISessionRepository _sessionRepository;
    private readonly IAnonymousUserRepository _anonymousUserRepository;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly EmailSender _emailSender;

    public GeneralUserManager(IApplicationUserRepository applicationUserRepository, IAttendentUserRepository attendentUserRepository, IOrganizationRepository organizationRepository, IHeadOfPlatformRepository headOfPlatformRepository, IIdentityUserRepository identityUserRepository, IProjectRepository projectRepository, ISessionRepository sessionRepository, UserManager<IdentityUser> userManager, EmailSender emailSender, IAnonymousUserRepository anonymousUserRepository)
    {
        _applicationUserRepository = applicationUserRepository;
        _attendentUserRepository = attendentUserRepository;
        _organizationRepository = organizationRepository;
        _headOfPlatformRepository = headOfPlatformRepository;
        _identityUserRepository = identityUserRepository;
        _projectRepository = projectRepository;
        _sessionRepository = sessionRepository;
        _userManager = userManager;
        _emailSender = emailSender;
        _anonymousUserRepository = anonymousUserRepository;
    }

    #region AddUsers

    public async Task AddApplicationUser(string firstName, string lastName, string userMail, string phoneNumber, 
        bool moreInfo, DateOnly birthDate, string password, RunningFlow runningFlow, Color color)
    {
        Session currentSession;
        if (runningFlow.IsKiosk)
        {
            currentSession = _sessionRepository.ReadSessionWithStateAndRunningFlow(State.Open, runningFlow.Id);
            _sessionRepository.UpdateSessionStateAndColor(currentSession.Id, runningFlow.Id);
        }
        else
        {
            currentSession = _sessionRepository.ReadSessionWithColorFromRunningFlow(color, runningFlow.Id);
            _sessionRepository.UpdateSessionStateAndColor(currentSession.Id, runningFlow.Id);
        }
        
        ApplicationUser applicationUser = new ApplicationUser()
        {
            FirstName = firstName,
            LastName = lastName,
            UserName = userMail,
            NormalizedUserName = userMail.ToUpper(),
            Email = userMail,
            NormalizedEmail = userMail.ToUpper(),
            EmailConfirmed = true,
            PhoneNumber = phoneNumber,
            PhoneNumberConfirmed = true,
            BirthDate = birthDate,
            TwoFactorEnabled = true,
            MoreInfo = moreInfo,
            Sessions = new List<Session>()
            {
                currentSession
            },
            LikedComments = new List<LikedComment>()
        };

        ValidationContext vc = new ValidationContext(applicationUser);
        List<ValidationResult> results = new List<ValidationResult>();
        bool isOk = Validator.TryValidateObject(applicationUser, vc, results, validateAllProperties: true);
        if (!isOk)
        {
            
            string errorMessage = "";
            foreach (ValidationResult vr in results)
            {
                errorMessage += vr.ErrorMessage + "\n";
            }

            throw new ValidationException(errorMessage);
        }
        
        await _userManager.CreateAsync(applicationUser, password);
        await _userManager.AddToRoleAsync(applicationUser, CustomIdentityConstants.Application);
        
        string emailBody = $"Your account has been created successfully by Phygital!\n" +
                           $"This is your login name: {userMail}\n" +
                           $"With password: ${password}\n" +
                           $"Please change your password after logging in for the first time.";
        await _emailSender.SendMailForKnowledgeOfUserCreation(userMail, "Phygital Account Created", emailBody,
            applicationUser.FirstName, applicationUser.LastName);
        
    }
    

    public async Task AddAttendentUser(string firstName, string lastName, string userMail, string phoneNumber,
        DateOnly birthDate, string organization, string assignedProjectName)
    {
        AttendentUser attendentUser = new AttendentUser()
        {
            FirstName = firstName,
            LastName = lastName,
            UserName = userMail,
            NormalizedUserName = userMail.ToUpper(),
            Email = userMail,
            NormalizedEmail = userMail.ToUpper(),
            EmailConfirmed = true,
            PhoneNumber = phoneNumber,
            PhoneNumberConfirmed = true,
            BirthDate = birthDate,
            TwoFactorEnabled = true,
            Organization = organization,
            AssignedProject = _projectRepository.ReadProjectByName(assignedProjectName)
        };

        ValidationContext vc = new ValidationContext(attendentUser);
        List<ValidationResult> results = new List<ValidationResult>();
        bool isOk = Validator.TryValidateObject(attendentUser, vc, results, validateAllProperties: true);
        if (!isOk)
        {
            string errorMessage = "";
            foreach (ValidationResult vr in results)
            {
                errorMessage += vr.ErrorMessage + "\n";
            }

            throw new ValidationException(errorMessage);
        }

        await _userManager.CreateAsync(attendentUser, "Password1!");
        await _userManager.AddToRoleAsync(attendentUser, CustomIdentityConstants.Attendent);

        string emailBody = $"Your account has been created successfully by {organization}!\n" +
                           $"This is your login name: {userMail}\n" +
                           $"With password: Password1!\n" +
                           $"Please change your password after logging in for the first time.";
        await _emailSender.SendMailForKnowledgeOfUserCreation(userMail, "Phygital Account Created", emailBody,
            attendentUser.FirstName, attendentUser.LastName);
    }

    public async Task AddOrganizationUser(string firstName, string lastName, string userMail, string phoneNumber,
        DateOnly birthDate, string organization)
    {
        OrganizationUser organizationUser = new OrganizationUser()
        {
            FirstName = firstName,
            LastName = lastName,
            UserName = userMail,
            NormalizedUserName = userMail.ToUpper(),
            Email = userMail,
            NormalizedEmail = userMail.ToUpper(),
            EmailConfirmed = true,
            PhoneNumber = phoneNumber,
            PhoneNumberConfirmed = true,
            BirthDate = birthDate,
            TwoFactorEnabled = true,
            Organization = organization,
            OwnedProjects = new List<Project>()
        };
            
        ValidationContext vc = new ValidationContext(organizationUser);
        List<ValidationResult> results = new List<ValidationResult>();
        bool isOk = Validator.TryValidateObject(organizationUser, vc, results, validateAllProperties: true);
        if (!isOk)
        {
            string errorMessage = "";
            foreach (ValidationResult vr in results)
            {
                errorMessage += vr.ErrorMessage + "\n";
            }
        
            throw new ValidationException(errorMessage);
        }
        
        await _userManager.CreateAsync(organizationUser, "Password1!");
        await _userManager.AddToRoleAsync(organizationUser, CustomIdentityConstants.Organization);

        string emailBody = $"Your account has been created successfully!\n" +
                           $"This is your login name: {userMail}\n" +
                           $"With password: Password1!\n" +
                           $"Please change your password after logging in for the first time.";
        await _emailSender.SendMailForKnowledgeOfUserCreation(userMail, "Phygital Account Created", emailBody,
            organizationUser.FirstName, organizationUser.LastName);
    }

    #endregion
    public void ChangeAttendentUserProject(AttendentUser attendant, Project project)
    {
        _attendentUserRepository.UpdateAttendentUserProject(attendant, project);
    }

    public void ChangeOrganizationUserProjects(OrganizationUser organizationUser, ICollection<Project> projects)
    {
        _organizationRepository.UpdateOrganizationUserProjects(organizationUser, projects);
    }

    #region GetUsers

    public IdentityUser GetUserByEmail(string userMail)
    {
        return _identityUserRepository.ReadIdentityUserByEmail(userMail);
    }
    public HeadOfPlatformUser GetHeadOfPlatformUserByEmail(string userMail)
    {
        return _headOfPlatformRepository.ReadHeadOfPlatformUserByEmail(userMail);
    }

    public OrganizationUser GetOrganizationUserByEmail(string userMail)
    {
        return _organizationRepository.ReadOrganizationUserByEmail(userMail);
    }

    public AttendentUser GetAttendentUserByEmail(string userMail)
    {
        return _attendentUserRepository.ReadAttendentUserByEmail(userMail);
    }

    public IEnumerable<AttendentUser> GetAttendantUsersByProjectId(long projectId)
    {
        return _attendentUserRepository.ReadAllAttendentUsersByProjectId(projectId);
    }

    public ApplicationUser GetApplicationUserByEmail(string userMail)
    {
        return _applicationUserRepository.ReadApplicationUserByEmail(userMail);
    }

    public OrganizationUser GetOrganizationUserById(string organizationUserId)
    {
        return _organizationRepository.ReadOrganizationUserById(organizationUserId);
    }
    public ICollection<OrganizationUser> GetAllOrganizationUsers()
    {
        return _organizationRepository.ReadAllOrganizationUsers();
    }

    public ICollection<AttendentUser> GetAllAttendentUsersFromOrganization(string organization)
    {
        return _attendentUserRepository.ReadAllAttendentsFromOrganization(organization);
    }

    public ICollection<AttendentUser> GetAttendentUsersByProjectId(long projectId)
    {
        return _attendentUserRepository.ReadAllAttendentUsersByProjectId(projectId);
    }

    public void AddLikedCommentToApplicationUser(LikedComment likedComment, ApplicationUser user)
    {
        user.LikedComments.Add(likedComment);
        _applicationUserRepository.UpdateApplicationUser(user);
    }

    public void RemoveLikedCommentFromApplicationUser(LikedComment likedComment, ApplicationUser user)
    {
        user.LikedComments.Remove(likedComment);
        _applicationUserRepository.UpdateApplicationUser(user);
    }

    #endregion
}
