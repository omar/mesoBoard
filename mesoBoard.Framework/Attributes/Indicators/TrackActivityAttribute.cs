using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Ninject;
using Ninject.Web.Mvc;
using Ninject.Web.Mvc.FilterBindingSyntax;

namespace mesoBoard.Framework
{
    public class TrackActivityAttribute : ActionFilterAttribute
    {
        public static void Bind(IKernel kernel)
        {
            kernel
                .BindFilter<TrackActivityFilter>(FilterScope.Controller, null)
                .WhenControllerHas(typeof(TrackActivityAttribute));
        }
    }
}