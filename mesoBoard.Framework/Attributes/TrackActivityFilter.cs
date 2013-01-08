using System;
using System.Web.Mvc;
using mesoBoard.Common;
using mesoBoard.Data;
using mesoBoard.Framework.Core;
using mesoBoard.Services;
using Ninject;

namespace mesoBoard.Framework
{
    public class TrackActivityFilter : IActionFilter
    {
        private IRepository<User> _userRepository;
        private GlobalServices _globalServices;

        public TrackActivityFilter(IRepository<User> userRepository, GlobalServices globalServices)
        {
            _userRepository = userRepository;
            _globalServices = globalServices;
        }

        private void UpdateUser(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                var user = _userRepository.Get(int.Parse(filterContext.HttpContext.User.Identity.Name));
                _globalServices.OnlineUserRoutine(user, filterContext.HttpContext.Request.UserHostAddress);
            }
            else
                _globalServices.OnlineUserRoutine(null, filterContext.HttpContext.Request.UserHostAddress);
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
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

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
        }
    }
}