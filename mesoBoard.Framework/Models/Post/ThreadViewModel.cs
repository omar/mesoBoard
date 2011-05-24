using mesoBoard.Common;
using mesoBoard.Data;
using mesoBoard.Services;
using mesoBoard.Framework.Core;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;
using System.ComponentModel;

namespace mesoBoard.Framework.Models
{
    public class ThreadViewModel : BaseViewModel, IThreadEditor, IPostEditor, IPollEditor, IEditor
    {
        public bool CanUploadAttachments { get; set; }
        public bool CanCreatePoll { get; set; }

        public bool? Preview { get; set; }
        public string PreviewText { get; set; }
        public string PreviewTitle { get; set; }

        public EditorType EditorType { get; set; }

        public Forum Forum { get; set; }
        public int ForumID { get; set; }

        public ThreadEditorViewModel ThreadEditor { get; set; }
        public PostEditorViewModel PostEditor { get; set; }
        public PollEditorViewModel PollEditor { get; set; }
    }
}