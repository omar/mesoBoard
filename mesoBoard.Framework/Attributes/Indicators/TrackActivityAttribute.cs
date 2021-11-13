using System;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace mesoBoard.Framework
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class TrackActivityAttribute : Attribute, IFilterFactory
    {
        public bool IsReusable => false;

        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            return serviceProvider.GetService<TrackActivityFilter>();
        }
    }
}