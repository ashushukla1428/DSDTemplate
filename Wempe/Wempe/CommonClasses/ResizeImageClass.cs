using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Web;

namespace Wempe.CommonClasses
{
    //System.Drawing.Image _photo = System.Drawing.Image.FromStream(vm.Image.InputStream);
    //var _image = ResizeImageClass.ResizeImage(_photo, 200, 200);
    //Graphics graphics = Graphics.FromImage((Image)_image);
    //ResizeImageClass.writeWaterMark(graphics, 2, 180, new Font("Arial", 14, FontStyle.Bold), "asdasd");
    //_image.Save(Server.MapPath("~/Content/CompanyLogo/"+ vm.Image.FileName),System.Drawing.Imaging.ImageFormat.Jpeg);
    public class ResizeImageClass
    {
        /// <summary>
        /// Resize the image to the specified width and height.
        /// </summary>
        /// <param name="image">The image to resize.</param>
        /// <param name="width">The width to resize to.</param>
        /// <param name="height">The height to resize to.</param>
        /// <returns>The resized image.</returns>
        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                // choose font for text
                
                //Font font = new Font("Arial", 20, FontStyle.Bold, GraphicsUnit.Pixel);
                ////choose color and transparency
                //Color color = Color.FromArgb(100, 255, 0, 0);
                ////location of the watermark text in the parent image
                //Point pt = new Point(1, 1);
                //SolidBrush brush = new SolidBrush(color);

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);

                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                    // graphics.DrawString("Hello",font,brush, pt);
                }
            }

            return destImage;
        }
        // Takes a graphics object, a font object for our text, and the text we want to write.
        // Then writes it to the handle as a watermark
        public static void writeWaterMark(Graphics graphicsHandle, int x, int y, Font ourFont, String text)
        {
            StringFormat strFormatter = new StringFormat();
            SolidBrush transBrush = new SolidBrush(Color.FromArgb(113, 255, 255, 255));

            // Drawstring the transparent text to the Graphics context at x,y position.
            graphicsHandle.DrawString(text,
                 ourFont,
                 transBrush,
                 new PointF(x, y),
                 strFormatter);

        }
    }
}