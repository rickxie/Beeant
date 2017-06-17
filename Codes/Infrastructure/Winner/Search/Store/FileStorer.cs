using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Winner.Search.Store
{
    public class FileStorer : IStorer
    {
        #region 属性
        /// <summary>
        /// 路径
        /// </summary>
        public string Path{get;set;}
        static readonly object CheckLoker=new object();
        static readonly IDictionary<string ,object> Lockers=new Dictionary<string, object>(); 
        #endregion

        #region 构造函数
        /// <summary>
        /// 无参数
        /// </summary>
        public FileStorer()
        { 
        }
        /// <summary>
        /// 路径
        /// </summary>
        /// <param name="path"></param>
        public FileStorer(string path)
        {
            Path = path;
        }
        #endregion

        #region 接口实现
        /// <summary>
        /// 存储信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual bool Save<T>(string name, T value)
        {
            var formatter = new BinaryFormatter();
            var fileName = GetFileName(name);
            lock (GetLocker(fileName))
            {
                CheckDirectory(fileName);
                using (Stream st = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
                {
                    formatter.Serialize(st, value);
                    st.Flush();
                } 
            }
            return true;
        }
        /// <summary>
        /// 删除信息
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public virtual bool Delete(string name)
        {
            string fileName = GetFileName(name);
            lock (GetLocker(fileName))
            {
                if (File.Exists(fileName))
                    File.Delete(fileName);
                else if (Directory.Exists(fileName))
                    Directory.Delete(fileName);
                
            }
            return true;
        }
        /// <summary>
        /// 读取信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public virtual T Read<T>(string name)
        {
            string fileName = GetFileName(name);
            lock (GetLocker(fileName))
            {
                if (!File.Exists(fileName))
                    return default(T);
                using (var file = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    var formatter = new BinaryFormatter();
                    return (T) formatter.Deserialize(file);
                }
            }
        }
        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="name"></param>
        public virtual void Flush(string name)
        {
            
        }
        /// <summary>
        /// 根据根目录读取所有存储名称
        /// </summary>
        /// <param name="rootName"></param>
        /// <returns></returns>
        public virtual IList<string> GetNames(string rootName)
        {
            var dir = GetFileName(rootName);
            if (!Directory.Exists(dir)) return null;
            var dirctory = new DirectoryInfo(dir);
            return dirctory.GetFiles().Select(file => string.Format("{0}{1}",rootName,file.Name)).ToList();
        }

        /// <summary>
        /// 得到文件名
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected virtual string GetFileName(string name)
        {
          return  System.IO.Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase,
                                   string.Format(@"{0}{1}", Path, name));
        }
        /// <summary>
        /// 检查目录是否存在
        /// </summary>
        /// <param name="fileName"></param>
        protected virtual void CheckDirectory(string fileName)
        {
            string dir = fileName.Substring(0, fileName.LastIndexOf(@"\"));
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
        }
        #endregion
        /// <summary>
        /// 得到锁
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        protected object GetLocker(string fileName)
        {
            lock (CheckLoker)
            {
                if(!Lockers.ContainsKey(fileName))
                    Lockers.Add(fileName,new object());
                return Lockers[fileName];
            }
        }

    }
}
