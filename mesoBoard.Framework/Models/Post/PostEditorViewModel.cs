using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using mesoBoard.Common;
using mesoBoard.Data;

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
        public HttpPostedFileBase[] Files { get; set; }
        public int[] Delete { get; set; }


    }
}