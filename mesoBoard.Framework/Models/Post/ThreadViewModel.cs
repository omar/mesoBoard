using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using mesoBoard.Common;
using mesoBoard.Data;
using mesoBoard.Framework.Core;
using mesoBoard.Services;

namespace mesoBoard.Framework.Models
{
    public class ThreadViewModel : IThreadEditor, IPostEditor, IPollEditor, IEditor
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