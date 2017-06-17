using System;
using System.IO;
using Winner.Storage.Route;

namespace Winner.Storage
{
    public class FileStore : IFile 
    {
        #region 属性
      
        /// <summary>
        /// 缓存
        /// </summary>
        public Cache.ICache Cache { get; set; }
        /// <summary>
        /// 文件路由
        /// </summary>
        public IFileRoute FileRoute { get; set; }
        #endregion

        #region 接口的实现
        /// <summary>
        /// 得到全路径
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public virtual string GetFullFileName(string fileName)
        {
            return fileName;
        }
        /// <summary>
        /// 创建文件名
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public virtual string CreateFileName(string fileName)
        {
            return FileRoute.CreateFileName(fileName);
        }

        /// <summary>
        /// 存储
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="fileByte"></param>
        /// <returns></returns>
        public virtual void Save(string fileName, byte[] fileByte)
        {
            if (string.IsNullOrEmpty(fileName) || fileByte == null || fileByte.Length==0) return;
            SaveFile(fileName, fileByte);
        }
        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public virtual void Remove(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return;
            DeleteFileOrDirectory(GetAbsoluteFileName(fileName));
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public byte[] Download(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return null;
            var fileByte = Cache.Get<byte[]>(fileName) ?? GetFileByte(fileName);
            if (fileByte != null)
                Cache.Set(fileName, fileByte);
            return fileByte;
        }

        #endregion

        #region 方法
 

        /// <summary>
        /// 得到文件二进制
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        protected virtual byte[] GetFileByte(string fileName)
        {
            fileName = GetAbsoluteFileName(fileName);
            if (!File.Exists(fileName)) return null;
            using (var s = new FileStream(fileName, FileMode.Open))
            {
                var buffer = new byte[s.Length];
                s.Read(buffer, 0, buffer.Length);
                return buffer;
            }
        }

        /// <summary>
        /// 删除文件或者路径
        /// </summary>
        /// <param name="fileName"></param>
        protected virtual void DeleteFileOrDirectory(string fileName)
        {
            if (File.Exists(fileName))
            {
                var index = fileName.LastIndexOf(@"\");
                string dir = fileName.Substring(0, index);
                var directory = new DirectoryInfo(dir);
                var pattern = fileName.Substring(index + 1, fileName.Length - index - 1);
                var extension = Path.GetExtension(pattern);
                if (!string.IsNullOrEmpty(extension))
                    pattern = pattern.Replace(extension, "");
                var files = directory.GetFiles(string.Format("{0}.*", pattern));
                foreach (var file in files)
                {
                    if (file.Exists)
                        file.Delete();
                }
            }
            DeleteDirectory(fileName);
        }

        /// <summary>
        /// 删除目录
        /// </summary>
        /// <param name="path"></param>
        protected virtual void DeleteDirectory(string path)
        {
            if (!Directory.Exists(path)) return;
            var fNames = Directory.GetFiles(path);
            foreach (var fName in fNames)
            {
                File.Delete(fName);
            }
            var dNames = Directory.GetDirectories(path);
            foreach (var dName in dNames)
            {
                DeleteDirectory(dName);
            }
            Directory.Delete(path);
        }


        /// <summary>
        /// 存储文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="fileByte"></param>
        protected virtual void SaveFile(string fileName,byte[] fileByte)
        {
            if (string.IsNullOrEmpty(fileName) || fileByte == null)return;
            fileName = GetAbsoluteFileName(fileName);
            CheckDirectoryAndDeleteFile(fileName);
            SaveByteToFile(fileName, fileByte);
        }
        /// <summary>
        /// 存储文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="fileByte"></param>
        protected virtual void SaveByteToFile(string fileName, byte[] fileByte)
        {
            using (var s = new FileStream(fileName, FileMode.Create))
            {
                using (var bw = new BinaryWriter(s))
                {
                    bw.Write(fileByte);
                }
            }
        }
        /// <summary>
        /// 得到文件绝对路径
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        protected virtual string GetAbsoluteFileName(string file)
        {
            string dir = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            return string.Format("{0}{1}",dir, file.Replace("/", "\\"));
        }
        /// <summary>
        /// 检查目录是否存在
        /// </summary>
        /// <param name="fileName"></param>
        protected virtual void CheckDirectoryAndDeleteFile(string fileName)
        {
            var index = fileName.LastIndexOf(@"\");
            string dir = fileName.Substring(0, index);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            else
            {
                var directory = new DirectoryInfo(dir);
                var pattern = fileName.Substring(index + 1, fileName.Length - index - 1);
                var extension = Path.GetExtension(pattern);
                if(!string.IsNullOrEmpty(extension))
                    pattern = pattern.Replace(extension, "");
                var files = directory.GetFiles(string.Format("{0}.*", pattern));
                foreach (var file in files)
                {
                    if(file.Exists)
                        file.Delete();
                }
            }
        }

        #endregion

    }
}
