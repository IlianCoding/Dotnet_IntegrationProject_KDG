using Microsoft.EntityFrameworkCore;
using NT.BL.Domain.projectpck;
using NT.BL.Domain.users;
using NT.DAL.EF;

namespace NT.DAL.User.AttendentPck;

public class AttendentUserRepository : IAttendentUserRepository
{
    private readonly PhygitalDbContext _phygitalDbContext;

    public AttendentUserRepository(PhygitalDbContext phygitalDbContext)
    {
        _phygitalDbContext = phygitalDbContext ?? throw new ArgumentNullException(nameof(phygitalDbContext));
    }
    public void UpdateAttendentUserProject(AttendentUser attendant, Project project)
    {
        var attendentUser = _phygitalDbContext.AttendentUsers
            .Include(proj => proj.AssignedProject)
            .Single(use => use.Id == attendant.Id);
        
        if (attendentUser != null)
        {
            attendentUser.AssignedProject = project;
            _phygitalDbContext.SaveChanges();
        }
    }
    
    public AttendentUser ReadAttendentUserByEmail(string userMail)
    {
        return _phygitalDbContext.AttendentUsers
            .Include(proj => proj.AssignedProject)
            .ThenInclude(pro => pro.Flows)
            .ThenInclude(fl => fl.RunningFlows)
            .Include(proj => proj.AssignedProject)
            .ThenInclude(pro => pro.Theme)
            .Single(mail => mail.Email == userMail);
    }

    public ICollection<AttendentUser> ReadAllAttendentsFromOrganization(string organization)
    {
        return _phygitalDbContext.AttendentUsers
            .Include(proj => proj.AssignedProject)
            .Where(users =>
            users.Organization.ToLower().Equals(organization.ToLower())).ToList();
    }

    public ICollection<AttendentUser> ReadAllAttendentUsersByProjectId(long projectId)
    {
        return _phygitalDbContext.AttendentUsers
            .Include(proj => proj.AssignedProject)
            .Where(users => users.AssignedProject.Id == projectId).ToList();
    }
}