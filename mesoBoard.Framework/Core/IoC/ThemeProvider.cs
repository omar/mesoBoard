using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Routing;
using mesoBoard.Data;
using mesoBoard.Services;
using Ninject;
using Ninject.Activation;

namespace mesoBoard.Framework.Core.IoC
{
    public class ThemeProvider : Provider<Theme>
    {
        protected override Theme CreateInstance(Ninject.Activation.IContext context)
        {
            var themeServices = context.Kernel.Get<ThemeServices>();

            RouteData routeData = RouteTable.Routes.GetRouteData(new HttpContextWrapper(HttpContext.Current));

            Theme theme;
            if (routeData.GetAreaName() == "Admin")
                theme = themeServices.GetAdminTheme();
            else
                theme = themeServices.GetDefaultTheme();

            return theme;
        }
    }
}
