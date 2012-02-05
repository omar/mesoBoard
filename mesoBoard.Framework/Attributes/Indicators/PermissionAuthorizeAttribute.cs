using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using mesoBoard.Common;
using Ninject.Web.Mvc;
using Ninject.Web.Mvc.FilterBindingSyntax;

namespace mesoBoard.Framework
{
    public class PermissionAuthorizeAttribute : ActionFilterAttribute
    {
        public SpecialPermissionValue[] Permissions { get; set; }

        public PermissionAuthorizeAttribute(params SpecialPermissionValue[] permissions)
        {
            Permissions = permissions;
        }

        public static void Bind(Ninject.IKernel kernel)
        {
            kernel
                .BindFilter<PermissionAuthorizeFilter>(FilterScope.Controller | FilterScope.Action, null)
                .WhenControllerHas<PermissionAuthorizeAttribute>()
                .WithConstructorArgumentFromControllerAttribute<PermissionAuthorizeAttribute>("Permissions", x => x.Permissions);
        }
    }
}
