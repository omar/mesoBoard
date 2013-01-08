using System;
using System.Reflection;
using System.Web.Mvc;

namespace mesoBoard.Framework
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class DefaultActionAttribute : ActionNameSelectorAttribute
    {
        private string _indexActionName = "Index";

        public override bool IsValidName(ControllerContext controllerContext, string actionName, MethodInfo methodInfo)
        {
            if (string.Equals(actionName, _indexActionName, StringComparison.OrdinalIgnoreCase))
            {
                controllerContext.RouteData.Values["action"] = methodInfo.Name;
                return true;
            }

            return string.Equals(actionName, methodInfo.Name, StringComparison.OrdinalIgnoreCase);
        }
    }
}