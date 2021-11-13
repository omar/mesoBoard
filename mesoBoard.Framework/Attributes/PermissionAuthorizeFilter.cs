using mesoBoard.Common;
using mesoBoard.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace mesoBoard.Framework
{

    public class PermissionAuthorizeFilter : IAuthorizationFilter
    {
        private SpecialPermissionValue[] _permission;
        private RoleServices _roleServices;

        public PermissionAuthorizeFilter(RoleServices roleServices, params SpecialPermissionValue[] permissions)
        {
            _permission = permissions;
            _roleServices = roleServices;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.User.Identity.IsAuthenticated) 
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            if (_roleServices.UserHasSpecialPermissions(int.Parse(context.HttpContext.User.Identity.Name), SpecialPermissionValue.Administrator)) 
            {
                return;
            }

            foreach (SpecialPermissionValue permission in _permission)
            {
                if (_roleServices.UserHasSpecialPermissions(int.Parse(context.HttpContext.User.Identity.Name), permission))
                    return;
            }

            context.Result = new UnauthorizedResult();
        }
    }
}