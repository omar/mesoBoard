using System;
using Microsoft.AspNetCore.Mvc.Filters;

namespace mesoBoard.Framework
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public class AllowOfflineAttribute : ActionFilterAttribute
    {
    }
}