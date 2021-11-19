using mesoBoard.Services;
using SixLabors.ImageSharp;
using SixLaborsCaptcha.Core;

namespace mesoBoard.Framework
{
    public static class Captcha
    {
        public static byte[] RenderCaptcha(string solution)
        {
            var slc = new SixLaborsCaptchaModule(new SixLaborsCaptchaOptions
			{
				DrawLines = (byte)SiteConfig.CaptchaWarpFactor.ToInt(),
                Height = (ushort)SiteConfig.CaptchaHeight.ToInt(),
                Width = (ushort)SiteConfig.CaptchaWidth.ToInt(),
                FontFamilies = new string[] { SiteConfig.CaptchaFontFamily.Value },
				TextColor = new Color[] { Color.Parse(SiteConfig.CaptchaFontColor.Value) },
			});
			var result = slc.Generate(solution);
            return result;
        }
    }
}