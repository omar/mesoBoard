using System;
using mesoBoard.Common;
using mesoBoard.Data;
using mesoBoard.Services;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;
using System.Text;

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
                _globalServices.OnlineUserRoutine(user, filterContext.HttpContext.Connection.RemoteIpAddress.ToString());
            }
            else
                _globalServices.OnlineUserRoutine(null, filterContext.HttpContext.Connection.RemoteIpAddress.ToString());
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            byte[] dateValue = null;
            if (filterContext.HttpContext.Session.TryGetValue(SessionKeys.LastActivityUpdate, out dateValue))
            {
                var date = DateTime.Parse(Encoding.ASCII.GetString(dateValue));
                var elapsedMinutes = (DateTime.UtcNow - date).TotalMinutes;

                if (elapsedMinutes > 15)
                {
                    UpdateUser(filterContext);
                    filterContext.HttpContext.Session.SetString(SessionKeys.LastActivityUpdate, DateTime.UtcNow.ToString());
                }
            }
            else
            {
                UpdateUser(filterContext);
                filterContext.HttpContext.Session.SetString(SessionKeys.LastActivityUpdate, DateTime.UtcNow.ToString());
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}