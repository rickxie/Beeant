using System.Drawing;
using System.IO;

namespace Component.Extension
{
    /// <summary>
    ///ImageHelper 的摘要说明
    /// </summary>
    public class ImageHelper
    {
        /// <summary>
        /// 图片的宽度，高度，新文件的路劲,填充的背景颜色,边距大小，是否加水印
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="col"></param>
        /// <param name="padding"></param>
        /// <returns></returns>
        public static Bitmap Create(string fileName,int width, int height,Color col, int padding)
        {
            Bitmap newImage;
            using (var img = (Bitmap)Bitmap.FromFile(fileName))
            {
                int x, y, newHeight, newWidth;
                float foWidth = img.Width, foHeight = img.Height, fWidth = width, fHeight = height;
                if ((foWidth / foHeight) > (fWidth / fHeight))
                {
                    x = 0;
                    newWidth = width;
                    newHeight = (int)(foHeight / foWidth * fWidth);
                    y = (int)((fHeight - newHeight) / 2);
                }
                else if ((foWidth / foHeight) < (fWidth / fHeight))
                {
                    y = 0;
                    newHeight = height;
                    newWidth = (int)(foWidth / foHeight * fHeight);
                    x = (int)((fWidth - newWidth) / 2);
                }
                else
                {
                    newHeight = height;
                    newWidth = width;
                    x = y = 0;
                }
                x += padding;
                y += padding;
                newHeight -= padding * 2;
                newWidth -= padding * 2;
                 newImage = new Bitmap(width, height);
                Graphics gs = Graphics.FromImage(newImage);
                gs.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Default;
                gs.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                gs.Clear(col);
                gs.DrawImage(img, new Rectangle(x, y, newWidth, newHeight), new Rectangle(0, 0, img.Width, img.Height), GraphicsUnit.Pixel);
                gs.Dispose();
 
            }
           return  newImage;

        }
      
 

        #region  储存图片
 
        /// <summary>
        /// 第一个参数为文件名称，第二个参数为原文件件图像的副本，这个储存是为了储存的文件名和原文件名相同
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="newImage"></param>
        static public void Save(string filename, Bitmap newImage)
        {

            string ext = Path.GetExtension(filename).ToLower();
            System.Drawing.Imaging.ImageFormat format = System.Drawing.Imaging.ImageFormat.MemoryBmp;
            switch (ext)
            {
                case ".gif": format = System.Drawing.Imaging.ImageFormat.Gif; break;
                case ".jpg": format = System.Drawing.Imaging.ImageFormat.Jpeg; break;
                case ".png": format = System.Drawing.Imaging.ImageFormat.Png; break;
                case ".bmp": format = System.Drawing.Imaging.ImageFormat.Bmp; break;
                case ".emf": format = System.Drawing.Imaging.ImageFormat.Emf; break;
                case ".exif": format = System.Drawing.Imaging.ImageFormat.Exif; break;
                case ".icon": format = System.Drawing.Imaging.ImageFormat.Icon; break;
                case ".tiff": format = System.Drawing.Imaging.ImageFormat.Tiff; break;
                case ".wmf": format = System.Drawing.Imaging.ImageFormat.Wmf; break;
            }
            newImage.Save(filename, format);
            newImage.Dispose();
  
        }

 
        #endregion


    }
}
