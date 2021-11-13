using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using mesoBoard.Data;
using Microsoft.AspNetCore.Http;

namespace mesoBoard.Framework.Models
{
    public class PostEditorViewModel
    {
        public int PostID { get; set; }

        [Required]
        public string Message { get; set; }

        [Display(Name = "Subscribe to thread")]
        public bool SubscribeToThread { get; set; }

        [Display(Name = "Show signature")]
        public bool ShowSignature { get; set; }

        public IEnumerable<Attachment> Attachments { get; set; }

        public IFormFile[] Files { get; set; }

        public int[] Delete { get; set; }
    }
}