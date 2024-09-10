using Microsoft.Extensions.Logging;
using NT.BL.Domain.flowpck;
using NT.BL.Domain.webplatformpck;

namespace NT.BL.Domain.users;

public class ApplicationUser : GeneralUser
{
    public bool MoreInfo { get; set; }
    public ICollection<Session> Sessions { get; set; }
    public ICollection<LikedComment> LikedComments { get; set; } = new List<LikedComment>();
}