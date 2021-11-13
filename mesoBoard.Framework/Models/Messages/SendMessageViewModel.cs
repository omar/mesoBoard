using System.ComponentModel.DataAnnotations;

namespace mesoBoard.Framework.Models
{
    public class SendMessageViewModel
    {
        [Required]
        [Display(Name = "To User")]
        public string Username { get; set; }

        public string Subject { get; set; }

        [Required]
        public string Message { get; set; }
    }
}