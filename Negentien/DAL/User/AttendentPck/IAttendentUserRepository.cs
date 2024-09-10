using NT.BL.Domain.projectpck;
using NT.BL.Domain.users;

namespace NT.DAL.User.AttendentPck;

public interface IAttendentUserRepository
{
    void UpdateAttendentUserProject(AttendentUser attendant, Project project);
    AttendentUser ReadAttendentUserByEmail(string userMail);
    ICollection<AttendentUser> ReadAllAttendentsFromOrganization(string organization);
    ICollection<AttendentUser> ReadAllAttendentUsersByProjectId(long projectId);
}