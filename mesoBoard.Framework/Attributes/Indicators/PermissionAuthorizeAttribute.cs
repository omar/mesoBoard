using System;
using mesoBoard.Common;
using mesoBoard.Services;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace mesoBoard.Framework
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class PermissionAuthorizeAttribute : Attribute, IFilterFactory
    {
        public bool IsReusable => false;

        public SpecialPermissionValue[] Permissions { get; set; }

        public PermissionAuthorizeAttribute(params SpecialPermissionValue[] permissions)
        {
            Permissions = permissions;
        }

        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            var roleServices = serviceProvider.GetService<RoleServices>();
            return new PermissionAuthorizeFilter(roleServices, Permissions);
        }
    }
}