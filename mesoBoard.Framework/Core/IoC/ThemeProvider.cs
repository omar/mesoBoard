using System.Threading.Tasks;
using mesoBoard.Data;
using mesoBoard.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace mesoBoard.Framework.Core.IoC
{

    public class ThemeFactory
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ThemeServices _themeServices;

        public ThemeFactory(
            IHttpContextAccessor httpContextAccessor,
            ThemeServices themeServices)
        {
            _httpContextAccessor = httpContextAccessor;
            _themeServices = themeServices;
        }

        public Theme GetTheme()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var areaName = httpContext.GetRouteValue("area") as string;

            if (areaName == "Admin")
                return _themeServices.GetAdminTheme();

            return _themeServices.GetDefaultTheme();
        }
    }
}
