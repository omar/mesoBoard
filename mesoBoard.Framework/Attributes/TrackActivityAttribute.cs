using System;
using System.Web.Mvc;
using mesoBoard.Common;
using mesoBoard.Data;
using mesoBoard.Services;
using mesoBoard.Framework.Core;
using Ninject;

namespace mesoBoard.Framework
{
    public class TrackActivityAttribute : ActionFilterAttribute
    {

        public IRepository<User> UserRepository
        {
            get
            {
                return ServiceLocator.Kernel.Get<IRepository<User>>();
            }
        }


        public GlobalServices GlobalServices
        {
            get
            {
                return ServiceLocator.Kernel.Get<GlobalServices>();
            }
        }

        private void UpdateUser(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                var user = UserRepository.Get(int.Parse(filterContext.HttpContext.User.Identity.Name));
                GlobalServices.OnlineUserRoutine(user, filterContext.HttpContext.Request.UserHostAddress);
            }
            else
                GlobalServices.OnlineUserRoutine(null, filterContext.HttpContext.Request.UserHostAddress);
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            DateTime? date = (DateTime?)filterContext.HttpContext.Session[SessionKeys.LastActivityUpdate];

            if (date.HasValue)
            {
                var elapsedMinutes = (DateTime.UtcNow - date.Value).TotalMinutes;

                if (elapsedMinutes > 15)
                {
                    UpdateUser(filterContext);
                    filterContext.HttpContext.Session[SessionKeys.LastActivityUpdate] = DateTime.UtcNow;
                }
            }
            else
            {
                UpdateUser(filterContext);
                filterContext.HttpContext.Session[SessionKeys.LastActivityUpdate] = DateTime.UtcNow;
            }
        }
    }
}