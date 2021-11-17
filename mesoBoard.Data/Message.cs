using System;
using System.Collections.Generic;

#nullable disable

namespace mesoBoard.Data
{
    public partial class Message
    {
        public int MessageID { get; set; }
        public int? FromUserID { get; set; }
        public int? ToUserID { get; set; }
        public string Subject { get; set; }
        public string Text { get; set; }
        public bool IsRead { get; set; }
        public DateTime DateSent { get; set; }
        public bool ToDelete { get; set; }
        public bool FromDelete { get; set; }

        public virtual User FromUser { get; set; }
        public virtual User ToUser { get; set; }
    }
}
