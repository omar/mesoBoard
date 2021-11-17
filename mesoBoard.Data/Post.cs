using System;
using System.Collections.Generic;

#nullable disable

namespace mesoBoard.Data
{
    public partial class Post
    {
        public Post()
        {
            Attachments = new HashSet<Attachment>();
        }

        public int PostID { get; set; }
        public int ThreadID { get; set; }
        public int UserID { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public bool UseSignature { get; set; }
        public string ParsedText { get; set; }
        public string TextOnly { get; set; }

        public virtual Thread Thread { get; set; }
        public virtual User User { get; set; }
        public virtual ReportedPost ReportedPost { get; set; }
        public virtual ICollection<Attachment> Attachments { get; set; }
    }
}
