using System;
using mesoBoard.Data;
using mesoBoard.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using System.Threading.Tasks;

namespace mesoBoard.Framework.Core.IoC
{
    public class UserFactory
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserServices _userServices;

        public UserFactory(
            IHttpContextAccessor httpContextAccessor,
            UserServices userServices)
        {
            _httpContextAccessor = httpContextAccessor;
            _userServices = userServices;
        }

        public async Task<User> GetUserAsync()
        {
            var httpContext = _httpContextAccessor.HttpContext;

            var user = new Data.User { UserID = 0 }; ;

            if (httpContext.User != null && httpContext.User.Identity.IsAuthenticated)
            {
                user = _userServices.GetUser(int.Parse(httpContext.User.Identity.Name));
                if (user == null)
                {
                    user = new Data.User { UserID = 0 };
                    await httpContext.SignOutAsync();
                }
            }

            httpContext.Items[HttpContextItemKeys.CurrentUser] = user;

            return user;
        }
    }
}
