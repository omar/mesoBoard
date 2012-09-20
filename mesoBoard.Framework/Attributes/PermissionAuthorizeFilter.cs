using System;
using System.Web;
using System.Web.Mvc;
using mesoBoard.Common;
using mesoBoard.Framework.Core;
using mesoBoard.Services;

namespace mesoBoard.Framework
{
    public class PermissionAuthorizeFilter : AuthorizeAttribute
    {
        private SpecialPermissionValue[] _permission;
        private RoleServices _roleServices;

        public PermissionAuthorizeFilter(RoleServices roleServices, params SpecialPermissionValue[] permissions)
        {
            _permission = permissions;
            _roleServices = roleServices;
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (!httpContext.User.Identity.IsAuthenticated)
                return false;

            if (_roleServices.UserHasSpecialPermissions(int.Parse(httpContext.User.Identity.Name), SpecialPermissionValue.Administrator))
                return true;

            foreach (SpecialPermissionValue permission in _permission)
            {
                if (_roleServices.UserHasSpecialPermissions(int.Parse(httpContext.User.Identity.Name), permission))
                    return true;
            }

            return false;
        }
    }
}