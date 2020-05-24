using System.ComponentModel.DataAnnotations;

namespace Cover.Models
{
    public class CreatePostViewModel
    {
        [Required]
        public string CreateBy { get; set; }

        [Required]
        public string Category { get; set; }

        [Required]
        public string Body { get; set; }
    }
}
