using System;
using System.Collections.Generic;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;

namespace Winner.Storage.Image
{
    public class ImageStore:FileStore
    {

        #region 属性
        public IDictionary<string, IList<ImageThumbnailInfo>> ImageThumbnails{get;set;}
        #endregion

        #region 构造函数
        /// <summary>
        /// 无参数
        /// </summary>
        public ImageStore()
        { 
            ImageThumbnails=new Dictionary<string, IList<ImageThumbnailInfo>>();
        }

        /// <summary>
        /// 存储路径，缩略图信息
        /// </summary>
        /// <param name="imageThumbnails"></param>
        public ImageStore(IDictionary<string, IList<ImageThumbnailInfo>> imageThumbnails)
        {
            ImageThumbnails = imageThumbnails;
        }
        #endregion

        #region 重载方法

        /// <summary>
        /// 重载文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="fileByte"></param>
        protected override void SaveFile(string fileName, byte[] fileByte)
        {
            base.SaveFile(fileName, fileByte);
            CreateImages(fileName);
        }
       
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="fileName"></param>
        protected override void DeleteFileOrDirectory(string fileName)
        {
            base.DeleteFileOrDirectory(fileName);
            string dir = fileName.Substring(0, fileName.LastIndexOf("/") + 1);
            if (!ImageThumbnails.ContainsKey(dir.ToLower()))
                return;
            foreach (var imageThumbnail in ImageThumbnails[dir.ToLower()])
            {
                var imageName = CreateThumbnailName(fileName, imageThumbnail);
                if (File.Exists(imageName))
                    File.Delete(imageName);
            }
        }

        #endregion

        #region 创建缩略图
        /// <summary>
        /// 添加缩略图
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public virtual bool AddImageThumbnail(string key, IList<ImageThumbnailInfo> value)
        {
            if (ImageThumbnails.ContainsKey(key))
                return false;
            ImageThumbnails.Add(key,value);
            return true;
        }
        /// <summary>
        /// 生成缩略图
        /// </summary>
        /// <param name="fileName"></param>
        protected virtual void CreateImages(string fileName)
        {
            string dir = fileName.Substring(0, fileName.LastIndexOf("/")+1);
            var imageThumbnails = ImageThumbnails.FirstOrDefault(it => dir.ToLower().Contains(it.Key));
            if (imageThumbnails.Value==null) return;
            fileName = GetAbsoluteFileName(fileName);
            foreach (var imageThumbnail in imageThumbnails.Value)
            {
                CreateThumbnail(fileName, imageThumbnail.Width, imageThumbnail.Height,CreateThumbnailName(fileName,imageThumbnail), Color.White, 0);
            }
        }
        /// <summary>
        /// 创建缩略图名称
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="imageThumbnail"></param>
        /// <returns></returns>
        protected virtual string CreateThumbnailName(string fileName,ImageThumbnailInfo imageThumbnail)
        {
            return string.Format("{0}.{1}{2}", fileName, imageThumbnail.Flag, Path.GetExtension(fileName));
        }

        #endregion
     
        #region 生成缩略图
        /// <summary>
        /// 创建突破
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="newFileName"></param>
        /// <param name="col"></param>
        /// <param name="padding"></param>
        /// <returns></returns>
        protected string CreateThumbnail(string fileName, int width, int height, string newFileName, Color col, int padding)
        {
            var cloneImage = (Bitmap)System.Drawing.Image.FromFile(fileName);
            int x, y, newHeight, newWidth;
            GetImageSize(cloneImage, width, height, padding, out x, out y, out newWidth, out newHeight);
            var newImage = new Bitmap(width, height);
            DrawImage(newImage, cloneImage, col, x, y, newWidth, newHeight);
            SaveImageFile(newFileName, newImage);
            newImage.Dispose();
            cloneImage.Dispose();
            return newFileName;
        }
        /// <summary>
        /// 画图
        /// </summary>
        /// <param name="newImage"></param>
        /// <param name="cloneImage"></param>
        /// <param name="col"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="newWidth"></param>
        /// <param name="newHeight"></param>
        protected virtual void DrawImage(Bitmap newImage, Bitmap cloneImage, Color col,int x,int y,int newWidth,int newHeight)
        {
            
            Graphics gs = Graphics.FromImage(newImage);
            gs.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Default;
            gs.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            gs.Clear(col);
            gs.DrawImage(cloneImage, new Rectangle(x, y, newWidth, newHeight), new Rectangle(0, 0, cloneImage.Width, cloneImage.Height), GraphicsUnit.Pixel);
            gs.Dispose();
        }
        /// <summary>
        /// 得到大小
        /// </summary>
        /// <param name="cloneImage"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="padding"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="newHeight"></param>
        /// <param name="newWidth"></param>
        protected virtual void GetImageSize(Bitmap cloneImage, int width, int height, int padding, out int x, out int y, out int newWidth, out int newHeight)
        {
            int imgWidth = cloneImage.Width, imgHeight = cloneImage.Height;
            float before=imgWidth / (float)imgHeight;
            float after=width / (float)height;
            SetImageSize(before, after, imgWidth, imgHeight, width, height, out x, out y, out newWidth, out newHeight);
            SetImageSizePadding(padding,ref x, ref y, ref newWidth, ref newHeight);
        }
        /// <summary>
        /// 设置图片大小
        /// </summary>
        /// <param name="before"></param>
        /// <param name="after"></param>
        /// <param name="imgWidth"></param>
        /// <param name="imgHeight"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="newWidth"></param>
        /// <param name="newHeight"></param>
        protected virtual void SetImageSize(float before, float after, int imgWidth, int imgHeight, int width, int height, out int x, out int y, out int newWidth, out int newHeight)
        {
            if (before > after)
                SetImageSizeByWidth(imgWidth, imgHeight, width, height, out x, out y, out newWidth, out newHeight);
            else if (before < after)
                SetImageSizeByHeight(imgWidth, imgHeight, width, height, out x, out y, out newWidth, out newHeight);
            else
                SetImageSizeByEqual(width, height, out x, out y, out newWidth, out newHeight);
        }
        /// <summary>
        /// 根据宽度生成
        /// </summary>
        /// <param name="imgWidth"></param>
        /// <param name="imgHeight"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="newWidth"></param>
        /// <param name="newHeight"></param>
        protected virtual void SetImageSizeByWidth(int imgWidth, int imgHeight, int width, int height, out int x, out int y, out int newWidth, out int newHeight)
        {
            x = 0;
            newWidth = width;
            newHeight = Convert.ToInt32((float)imgHeight / imgWidth * width);
            y = (height - newHeight) / 2;
        }
        /// <summary>
        /// 根据高度设置
        /// </summary>
        /// <param name="imgWidth"></param>
        /// <param name="imgHeight"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="newWidth"></param>
        /// <param name="newHeight"></param>
        protected virtual void SetImageSizeByHeight(int imgWidth, int imgHeight, int width, int height, out int x, out int y, out int newWidth, out int newHeight)
        {
            y = 0;
            newHeight = height;
            newWidth = Convert.ToInt32((float)imgWidth / imgHeight * height);
            x = (width - newWidth) / 2;
        }
        /// <summary>
        /// 比例相等
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="newWidth"></param>
        /// <param name="newHeight"></param>
        protected virtual void SetImageSizeByEqual(int width, int height, out int x, out int y, out int newWidth, out int newHeight)
        {
            newHeight = height;
            newWidth = width;
            x = y = 0;
        }
        /// <summary>
        /// 设置padding
        /// </summary>
        /// <param name="padding"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="newWidth"></param>
        /// <param name="newHeight"></param>
        protected virtual void SetImageSizePadding(int padding,ref int x, ref int y, ref int newWidth, ref int newHeight)
        {
            x += padding;
            y += padding;
            newHeight -= padding * 2;
            newWidth -= padding * 2;
        }

        /// <summary>
        /// 第一个参数为文件名称，第二个参数为原文件件图像的副本，这个储存是为了储存的文件名和原文件名相同
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="newImage"></param>
        protected virtual void SaveImageFile(string filename, Bitmap newImage)
        {
            var extension = Path.GetExtension(filename);
            if (extension != null)
            {
                string ext = extension.ToLower();
                ImageFormat format = GetImageFormat(ext);
                newImage.Save(filename, format);
            }
            newImage.Dispose();
        }
        /// <summary>
        /// 得到ImageFormat
        /// </summary>
        /// <param name="ext"></param>
        /// <returns></returns>
        protected virtual ImageFormat GetImageFormat(string ext)
        {
            IDictionary<string, ImageFormat> formats = new Dictionary<string, ImageFormat>
                {
               {".gif",ImageFormat.Gif},{".jpg",ImageFormat.Jpeg},{".png",ImageFormat.Png},
               {".bmp",ImageFormat.Bmp},{".emf",ImageFormat.Emf},{".exif",ImageFormat.Exif},
               {".icon",ImageFormat.Icon},{".tiff",ImageFormat.Tiff},{".wmf",ImageFormat.Wmf}
            };
            return  formats.ContainsKey(ext) ? formats[ext] : ImageFormat.MemoryBmp;
        }
        #endregion
    }
}
