using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using mesoBoard.Common;
using System.Web.Mvc;

namespace mesoBoard.Framework.Models
{
    public class ThreadEditorViewModel
    {
        public int ThreadID { get; set; }
        public string[] ThreadImages { get; set; }
        public bool AllowThreadImages { get; set; }
        public string Image { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Title must be less than 50 characters")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Thread Type")]
        public int ThreadType { get; set; }

        public List<ThreadTypes> PermittedThreadTypes { get; set; }

        public SelectList ThreadTypeList
        {
            get
            {
                return new SelectList(PermittedThreadTypes, "Value", "Name");
            }
        }
    }
}