using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Web;
using mesoBoard.Framework.Core;
using mesoBoard.Framework;
using System.Web.Mvc;

namespace mesoBoard.Web.Controllers
{
    public class CaptchaController : Controller
    {
        public void Render(string challengeGuid)
        {
            string solution = (string)Session[SessionKeys.CaptchaSessionPrefix + challengeGuid];

            using (Bitmap bmp = Captcha.RenderCaptcha(solution))
            {
                Response.ContentType = "image/jpeg";
                Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
                Response.Cache.SetValidUntilExpires(false);
                Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Cache.SetNoStore();
                bmp.Save(Response.OutputStream, ImageFormat.Jpeg);
            }
        }
    }
}
