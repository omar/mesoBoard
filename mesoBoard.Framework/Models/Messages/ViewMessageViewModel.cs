using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using mesoBoard.Data;

namespace mesoBoard.Framework.Models
{
    public class ViewMessageViewModel
    {
        public User CurrentUser { get; set; }
        public string ParsedText { get; set; }
        public Message Message { get; set; }

        public SendMessageViewModel SendMessageViewModel { get; set; }
    }
}