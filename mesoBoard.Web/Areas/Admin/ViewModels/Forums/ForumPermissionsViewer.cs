using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using mesoBoard.Data;

namespace mesoBoard.Web.Areas.Admin.ViewModels
{
    public class ForumPermissionsViewer
    {
        public Forum Forum { get; set; }

        public IEnumerable<ForumPermission> ForumPermissions { get; set; }
    }
}