using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NT.BL.Domain.projectpck;
using NT.BL.Domain.users;
using NT.DAL.EF;

namespace NT.DAL.User.OrganizationPck;

public class OrganizationRepository : IOrganizationRepository
{
    private readonly PhygitalDbContext _phygitalDbContext;

    public OrganizationRepository(PhygitalDbContext phygitalDbContext)
    {
        _phygitalDbContext = phygitalDbContext ?? throw new ArgumentNullException(nameof(phygitalDbContext));
    }

    public OrganizationUser ReadOrganizationUserById(string organizationUserId)
    {
        return _phygitalDbContext.OrganisationUsers
            .SingleOrDefault(user => user.Id == organizationUserId);
    }

    public OrganizationUser ReadOrganizationUserByEmail(string userMail)
    {
        return _phygitalDbContext.OrganisationUsers
            .SingleOrDefault(mail => mail.Email == userMail);
    }

    public ICollection<OrganizationUser> ReadAllOrganizationUsers()
    {
        return _phygitalDbContext.OrganisationUsers.ToList();
    }

    public void UpdateOrganizationUserProjects(OrganizationUser organizationUser, ICollection<Project> projects)
    {
        var organisationUserChange = _phygitalDbContext.OrganisationUsers
            .Include(lst => lst.OwnedProjects)
            .SingleOrDefault(org => org.Email.ToUpper() == organizationUser.Email.ToUpper());

        if (organisationUserChange != null)
        {
            foreach (var project in projects)
            { 
                organisationUserChange.OwnedProjects.Add(project);
            }
            _phygitalDbContext.SaveChanges();
        }
    }
}