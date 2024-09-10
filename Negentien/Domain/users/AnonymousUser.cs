using System.ComponentModel.DataAnnotations;
using NT.BL.Domain.flowpck;

namespace NT.BL.Domain.users;

public class AnonymousUser
{
    [Key]
    public long Id { get; set; }
    public DateOnly ActiveDate { get; set; }
    public Session Session { get; set; }
}