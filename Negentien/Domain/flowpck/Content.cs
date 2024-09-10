using System.ComponentModel.DataAnnotations;

namespace NT.BL.Domain.flowpck;

public abstract class Content
{
    [Key]
    public long Id { get; set; }
    
    
}