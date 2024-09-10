using NT.BL.Domain.projectpck;
using NT.BL.Domain.users;

namespace NT.UI.MVC.Models.UserViewModels;

public class AttendentDetailsViewModel
{
    public ICollection<AttendentUser> AttendentUsers { get; set; }
    public ICollection<Project> Projects { get; set; }
}