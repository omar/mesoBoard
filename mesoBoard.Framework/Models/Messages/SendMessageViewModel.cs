using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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