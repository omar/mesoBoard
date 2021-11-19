using System;
using mesoBoard.Framework;
using mesoBoard.Framework.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SixLaborsCaptcha.Core;

namespace mesoBoard.Web.Core.Controllers
{
    public class CaptchaController : BaseController
    {
        [ResponseCache(NoStore=true, Location=ResponseCacheLocation.None)]
        public FileResult Render(string challengeGuid)
        {
            var solution = Extentions.GetUniqueKey(6);
            HttpContext.Session.SetString(SessionKeys.CaptchaSessionPrefix + challengeGuid, solution);

            var captcha = Captcha.RenderCaptcha(solution);
            return File(captcha, "image/png");
        }
    }
}