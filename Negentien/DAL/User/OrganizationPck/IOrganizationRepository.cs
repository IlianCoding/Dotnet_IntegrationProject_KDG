using Microsoft.AspNetCore.Identity;
using NT.BL.Domain.projectpck;
using NT.BL.Domain.users;

namespace NT.DAL.User.OrganizationPck;

public interface IOrganizationRepository
{
    OrganizationUser ReadOrganizationUserById(string organizationUserId);
    OrganizationUser ReadOrganizationUserByEmail(string userMail);
    ICollection<OrganizationUser> ReadAllOrganizationUsers();
    void UpdateOrganizationUserProjects(OrganizationUser organizationUser, ICollection<Project> projects);
}