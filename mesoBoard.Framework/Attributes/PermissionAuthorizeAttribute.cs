using System.Web;
using System.Web.Mvc;
using mesoBoard.Common;
using mesoBoard.Services;
using mesoBoard.Framework.Core;

namespace mesoBoard.Framework
{
    public class PermissionAuthorizeAttribute : AuthorizeAttribute
    {
        private SpecialPermissionValue[] _permission;

        public PermissionAuthorizeAttribute(params SpecialPermissionValue[] permissions)
        {
            _permission = permissions;
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            RoleServices roles = ServiceLocator.Get<RoleServices>();
            if (!httpContext.User.Identity.IsAuthenticated)
                return false;

            if (roles.UserHasSpecialPermissions(int.Parse(httpContext.User.Identity.Name), SpecialPermissionValue.Administrator))
                return true;

            bool hasPermission = false;

            foreach (SpecialPermissionValue permission in _permission)
            {
                if (roles.UserHasSpecialPermissions(int.Parse(httpContext.User.Identity.Name), permission))
                {
                    hasPermission = true;
                    break;
                }
            }

            return hasPermission;
                       
        }
    }
}