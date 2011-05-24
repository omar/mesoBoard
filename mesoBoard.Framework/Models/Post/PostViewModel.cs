using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using mesoBoard.Data;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace mesoBoard.Framework.Models
{
    public class PostViewModel : IPostEditor
    {
        public bool CanUploadAttachments { get; set; }
        public bool? Preview { get; set; }
        public string PreviewText { get; set; }
        
        public int ThreadID { get; set; }
        public Thread Thread { get; set; }
        public EditorType EditorType { get; set; }
        public PostEditorViewModel PostEditor { get; set; }
    }
}