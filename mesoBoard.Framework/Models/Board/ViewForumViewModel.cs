using System.Collections.Generic;
using mesoBoard.Common;
using mesoBoard.Data;

namespace mesoBoard.Framework.Models
{
    public class ViewForumViewModel : BaseViewModel 
    {
        public Forum Forum { get; set; }
        public List<ThreadRow> GlobalAnnouncements { get; set; }
        public List<ThreadRow> ThreadRows { get; set; }
        public UserPermissions UserPermissions { get; set; }
        public Pagination Pagination { get; set; }
    }
}