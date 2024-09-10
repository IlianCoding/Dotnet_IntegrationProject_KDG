using System.ComponentModel.DataAnnotations;

namespace NT.BL.Domain.flowpck
{
    public class InformationContent : Content
    {
        [Required]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Title should be between 1 and 100 characters long.")]
        public string Title { get; set; }
        [StringLength(1000, MinimumLength = 3, ErrorMessage = "Text should be between 3 and 1000 characters long.")]
        public string TextInformation { get; set; }
        public string ObjectName { get; set; }
        public string ContentType { get; set; }
    }
}