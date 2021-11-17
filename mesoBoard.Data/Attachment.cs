using System;
using System.Collections.Generic;

#nullable disable

namespace mesoBoard.Data
{
    public partial class Attachment
    {
        public int AttachmentID { get; set; }
        public int PostID { get; set; }
        public string DownloadName { get; set; }
        public string SavedName { get; set; }
        public string Type { get; set; }
        public int Size { get; set; }
        public int Downloaded { get; set; }

        public virtual Post Post { get; set; }
    }
}
