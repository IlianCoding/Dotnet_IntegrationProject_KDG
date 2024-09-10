using Microsoft.AspNetCore.Identity;
using NT.BL.Domain.flowpck;
using NT.BL.Domain.projectpck;
using NT.BL.Domain.users;
using NT.BL.Domain.webplatformpck;

namespace NT.BL.UserMngr;

public interface IGeneralUserManager
{
    Task AddApplicationUser(string firstName, string lastName, string userMail, string phoneNumber, 
        bool moreInfo, DateOnly birthDate, string password, RunningFlow runningFlow, Color color);
    Task AddAttendentUser(string firstName, string lastName, string userMail, string phoneNumber, DateOnly birthDate,
        string organization, string assignedProjectName);
    Task AddOrganizationUser(string firstName, string lastName, string userMail, string phoneNumber,
        DateOnly birthDate, string organization);
    void ChangeAttendentUserProject(AttendentUser attendant, Project project);
    void ChangeOrganizationUserProjects(OrganizationUser organizationUser, ICollection<Project> projects);
    OrganizationUser GetOrganizationUserByEmail(string userMail);
    AttendentUser GetAttendentUserByEmail(string userMail);
    IEnumerable<AttendentUser> GetAttendantUsersByProjectId(long projectId);
    ApplicationUser GetApplicationUserByEmail(string userMail);
    OrganizationUser GetOrganizationUserById(string organizationUserId);
    ICollection<AttendentUser> GetAllAttendentUsersFromOrganization(string organization);
    ICollection<AttendentUser> GetAttendentUsersByProjectId(long projectId);
    void AddLikedCommentToApplicationUser(LikedComment likedComment, ApplicationUser user);
    void RemoveLikedCommentFromApplicationUser(LikedComment likedComment, ApplicationUser user);
}