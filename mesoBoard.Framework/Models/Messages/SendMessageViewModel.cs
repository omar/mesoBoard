using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.ComponentModel;
using mesoBoard.Framework.Core;
using mesoBoard.Services;

namespace mesoBoard.Framework.Models
{
    public class SendMessageViewModel
    {
        [Required]
        [Display(Name = "To User")]
        public string Username { get; set; }

        public string Subject { get; set; }

        [AllowHtml]
        [Required]
        public string Message { get; set; }
    }
}