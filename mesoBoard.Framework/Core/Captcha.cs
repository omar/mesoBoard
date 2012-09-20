using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using mesoBoard.Services;

namespace mesoBoard.Framework
{
    public static class Captcha
    {
        //
        // CAPTCHA code taken from "Pro ASP.NET MVC Framework" by Steven Sanderson
        // Apress publishing
        //
        // Modified for mesoBoard

        private static int ImageWidth
        {
            get
            {
                return SiteConfig.CaptchaWidth.ToInt();
            }
        }

        private static int ImageHeight
        {
            get
            {
                return SiteConfig.CaptchaHeight.ToInt();
            }
        }

        private static string FontFamily
        {
            get
            {
                return SiteConfig.CaptchaFontFamily.Value;
            }
        }

        private static Brush Foreground
        {
            get
            {
                return new SolidBrush(ColorTranslator.FromHtml(SiteConfig.CaptchaFontColor.Value));
            }
        }

        private static Color Background
        {
            get
            {
                return ColorTranslator.FromHtml(SiteConfig.CaptchaBackgroundColor.Value);
            }
        }

        private static int WarpFactor
        {
            get
            {
                return SiteConfig.CaptchaWarpFactor.ToInt();
            }
        }

        private static Double xAmp = WarpFactor * ImageWidth / 100;
        private static Double yAmp = WarpFactor * ImageHeight / 85;
        private static Double xFreq = 2 * Math.PI / ImageWidth;
        private static Double yFreq = 2 * Math.PI / ImageHeight;

        private static GraphicsPath DeformPath(GraphicsPath path)
        {
            PointF[] deformed = new PointF[path.PathPoints.Length];
            Random rng = new Random();
            Double xSeed = rng.NextDouble() * 2 * Math.PI;
            Double ySeed = rng.NextDouble() * 2 * Math.PI;
            for (int i = 0; i < path.PathPoints.Length; i++)
            {
                PointF original = path.PathPoints[i];
                Double val = xFreq * original.X + yFreq * original.Y;
                int xOffset = (int)(xAmp * Math.Sin(val + xSeed));
                int yOffset = (int)(yAmp * Math.Sin(val + ySeed));
                deformed[i] = new PointF(original.X + xOffset, original.Y + yOffset);
            }
            return new GraphicsPath(deformed, path.PathTypes);
        }

        public static Bitmap RenderCaptcha(string solution)
        {
            // Make a blank canvas to render the CAPTCHA on
            Bitmap bmp = new Bitmap(ImageWidth, ImageHeight);

            using (Graphics g = Graphics.FromImage(bmp))
            using (Font font = new Font(FontFamily, 1f))
            {
                g.Clear(Background);

                // Perform trial rendering to determine best font size
                SizeF finalSize;
                SizeF testSize = g.MeasureString(solution, font);
                float bestFontSize = Math.Min(ImageWidth / testSize.Width,
                                        ImageHeight / testSize.Height) * 0.95f;

                using (Font finalFont = new Font(FontFamily, bestFontSize))
                {
                    finalSize = g.MeasureString(solution, finalFont);
                }

                // Get a path representing the text centered on the canvas
                g.PageUnit = GraphicsUnit.Point;
                PointF textTopLeft = new PointF((ImageWidth - finalSize.Width) / 2,
                                              (ImageHeight - finalSize.Height) / 2);
                using (GraphicsPath path = new GraphicsPath())
                {
                    path.AddString(solution, new FontFamily(FontFamily), 0,
                        bestFontSize, textTopLeft, StringFormat.GenericDefault);

                    // Render the path to the bitmap
                    g.SmoothingMode = SmoothingMode.HighQuality;

                    g.FillPath(Foreground, DeformPath(path));
                    g.Flush();

                    return bmp;
                }
            }
        }
    }
}