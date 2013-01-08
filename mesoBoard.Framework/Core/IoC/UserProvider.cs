using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using mesoBoard.Data;
using mesoBoard.Services;
using Ninject;
using Ninject.Activation;

namespace mesoBoard.Framework.Core.IoC
{
    public class UserProvider : Provider<User>
    {
        protected override User CreateInstance(IContext context)
        {
            var httpContext = HttpContext.Current;

            User user = new Data.User { UserID = 0 }; ;

            if (httpContext.User != null && httpContext.User.Identity.IsAuthenticated)
            {
                var userServices = context.Kernel.Get<UserServices>();
                user = userServices.GetUser(int.Parse(httpContext.User.Identity.Name));
                if (user == null)
                {
                    user = new Data.User { UserID = 0 };
                    FormsAuthentication.SignOut();
                }
            }

            httpContext.Items[HttpContextItemKeys.CurrentUser] = user;

            return user;
        }
    }
}
