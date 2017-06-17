using System.Collections.Generic;
using Winner.Storage.Synchronization;

namespace Winner.Storage
{
    public class FileService :  IFileContract
    {
        #region 属性
        /// <summary>
        /// 文件存储实例
        /// </summary>
        public IFile File { get; set; }
        /// <summary>
        /// 服务实例
        /// </summary>
        public IMaster Master { get; set; }
        /// <summary>
        /// 存储文件,返回存储后的文件路径
        /// </summary>
        /// <param name="dataServices"></param>
        /// <param name="fileName"></param>
        /// <param name="fileByte"></param>
        /// <returns></returns>
        delegate void SaveDeletge(IList<string> dataServices, string fileName, byte[] fileByte);

        /// <summary>
        /// 删除文件或者目录
        /// </summary>
        /// <param name="dataServices"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
       delegate void RemoveDelete(IList<string> dataServices, string fileName);
        #endregion

        #region 构造函数
        /// <summary>
        /// 无参数
        /// </summary>
        public FileService()
        { 
        }

        /// <summary>
        /// WCF服务端配置文件路径，文件存储实例，错误日志实例
        /// </summary>
        /// <param name="file"></param>
        public FileService(IFile file)
        {
            File = file;
        }
        #endregion

        #region 接口的实现

        /// <summary>
        /// 存储
        /// </summary>
        /// <param name="endpoints"></param>
        /// <param name="fileName"></param>
        /// <param name="fileByte"></param>
        /// <returns></returns>
        public virtual void Save(IList<string> endpoints, string fileName, byte[] fileByte)
        {
            File.Save(fileName,fileByte);
            if (endpoints != null && endpoints.Count > 0)
            {
                var saveHandle = new SaveDeletge(Master.Save);
                saveHandle.BeginInvoke(endpoints, fileName, fileByte, null, null); 
            }
        }

      
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="endpoints"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public virtual void Remove(IList<string> endpoints, string fileName)
        {
            File.Remove(fileName);
            if (endpoints != null && endpoints.Count > 0)
            {
                var removeHandle = new RemoveDelete(Master.Remove);
                removeHandle.BeginInvoke(endpoints, fileName, null, null);
            }
        }
        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public virtual byte[] Download(string fileName)
        {
            return File.Download(fileName);
        }
 

        #endregion
 




    }
}
