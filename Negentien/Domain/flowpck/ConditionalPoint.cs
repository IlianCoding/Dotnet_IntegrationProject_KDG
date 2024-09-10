using System.ComponentModel.DataAnnotations;

namespace NT.BL.Domain.flowpck
{
    public class ConditionalPoint
    {
        [Key]
        public long Id { get; set; }
        public string ConditionalPointName { get; set; }
        public Step ConditionalStep { get; set; }
    }
}