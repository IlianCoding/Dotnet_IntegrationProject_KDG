using Microsoft.AspNetCore.Identity;
using NT.BL.Domain.users;

namespace NT.BL.Domain.webplatformpck;

public class Comment
{
    public long Id { get; set; }
    public string Text { get; set; }
    public ApplicationUser User { get; set; }
    public int Likes { get; set; }
    public ICollection<Comment> SubComments { get; set; }
    public string ObjectName { get; set; }
    public string ContentType { get; set; }
}