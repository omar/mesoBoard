using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using mesoBoard.Data;

namespace mesoBoard.Web.Areas.Admin.ViewModels
{
    public class RolesViewer
    {
        public IEnumerable<Role> Roles { get; set; }

        public RoleViewModel RoleViewModel { get; set; }
    }
}