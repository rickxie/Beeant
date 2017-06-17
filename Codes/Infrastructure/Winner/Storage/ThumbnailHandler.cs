using System.Drawing;
using System.IO;
using System.Web;

namespace Winner.Storage
{
    public class ThumbnailHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var fileName = context.Request.Url.AbsolutePath.ToLower();
           var rev= Output(context, fileName);
            if (!rev)
            {
                SetContentType(context, fileName);
                fileName = context.Server.MapPath(fileName);
                if (File.Exists(fileName))
                    context.Response.WriteFile(fileName);
            }

        }

        public bool IsReusable { get; }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="fileName"></param>
        protected virtual bool Output(HttpContext context,string fileName)
        {
            var index = fileName.IndexOf(".");
            var lastindex = fileName.LastIndexOf(".");
            if (index <= -1 || lastindex<=-1)
                return false;
            if (lastindex == index)
                return false;
            index = fileName.IndexOf(".", index + 1);
            var values = fileName.Substring(index + 1, lastindex - index - 1).Split('-');
            if (values.Length != 2)
                return false;
            int width, height;
            int.TryParse(values[0], out width);
            int.TryParse(values[1], out height);
            if(width==0 || height==0)
                return false;
            OutputFile(context, fileName.Substring(0, index), width,height);
            return true;
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="context"></param>
        /// <param name="fileName"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        protected virtual void OutputFile(HttpContext context, string fileName, int width, int height)
        {
            fileName = context.Server.MapPath(fileName);
            if (!File.Exists(fileName))
                return;
            using (var newImage = new Bitmap(width, height))
            {
                using (var img = (Bitmap) Bitmap.FromFile(fileName))
                {
                    Graphics gs = Graphics.FromImage(newImage);
                    gs.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Default;
                    gs.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    gs.DrawImage(img, new Rectangle(0, 0, width, height), new Rectangle(0, 0, img.Width, img.Height),
                        GraphicsUnit.Pixel);
                    gs.Dispose();

                }
                var format = SetContentType(context, fileName);
                newImage.Save(context.Response.OutputStream, format);
            }
        }

        /// <summary>
        /// 设置类型
        /// </summary>
        /// <param name="context"></param>
        /// <param name="fileName"></param>
        protected virtual System.Drawing.Imaging.ImageFormat SetContentType(HttpContext context, string fileName)
        {
            var contentType = "image/jpeg";
            string ext = System.IO.Path.GetExtension(fileName).ToLower();
            System.Drawing.Imaging.ImageFormat format = System.Drawing.Imaging.ImageFormat.MemoryBmp;
            switch (ext)
            {
                case ".gif": contentType = "image/gif"; format = System.Drawing.Imaging.ImageFormat.Gif; break;
                case ".jpg": contentType = "image/jpg"; format = System.Drawing.Imaging.ImageFormat.Jpeg; break;
                case ".png": contentType = "image/png"; format = System.Drawing.Imaging.ImageFormat.Png; break;
                case ".bmp": contentType = "image/bmp"; format = System.Drawing.Imaging.ImageFormat.Bmp; break;
                case ".jpeg": contentType = "image/jpeg"; format = System.Drawing.Imaging.ImageFormat.Jpeg; break;
                case ".emf": contentType = "image/emf"; format = System.Drawing.Imaging.ImageFormat.Emf; break;
                case ".exif": contentType = "image/exif"; format = System.Drawing.Imaging.ImageFormat.Exif; break;
                case ".icon": contentType = "image/icon"; format = System.Drawing.Imaging.ImageFormat.Icon; break;
                case ".tiff": contentType = "image/tiff"; format = System.Drawing.Imaging.ImageFormat.Tiff; break;
                case ".wmf": contentType = "image/wmf"; format = System.Drawing.Imaging.ImageFormat.Wmf; break;
            }
            context.Response.ContentType = contentType;
            return format;
        }

    }
}
