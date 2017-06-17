using System.Collections.Generic;
using System.ServiceModel;

namespace Winner.Storage
{
    /// <summary>
    /// 文件存储接口
    /// </summary>
    [ServiceContract(Namespace = "http://Winner.Storage", ConfigurationName = "Winner.Storage.IFileContract")]
    public interface IFileContract
    {
        /// <summary>
        /// 存储文件,返回存储后的文件路径
        /// </summary>
        /// <param name="endpoints"></param>
        /// <param name="fileName"></param>
        /// <param name="fileByte"></param>
        /// <returns></returns>
        [OperationContract(Action = "http://Winner.Storage.IFileContract/Save",
            ReplyAction = "http://Winner.Storage.IFileContract/SaveResponse")]
        void Save(IList<string> endpoints, string fileName, byte[] fileByte);

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="endpoints"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [OperationContract(Action = "http://Winner.Storage.IFileContract/Remove",
            ReplyAction = "http://Winner.Storage.IFileContract/RemoveResponse")]
        void Remove(IList<string> endpoints, string fileName);

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [OperationContract(Action = "http://Winner.Storage.IFileContract/Download",
            ReplyAction = "http://Winner.Storage.IFileContract/DownloadResponse")]
        byte[] Download(string fileName);
    }
}
