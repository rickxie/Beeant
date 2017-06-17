using System.Collections.Generic;

namespace Winner.Storage.Synchronization
{
    public interface IMaster
    {
        /// <summary>
        /// 存储文件,返回存储后的文件路径
        /// </summary>
        /// <param name="endpoints"></param>
        /// <param name="fileName"></param>
        /// <param name="fileByte"></param>
        /// <returns></returns>
        void Save(IList<string> endpoints, string fileName, byte[] fileByte);

        /// <summary>
        /// 删除文件或者目录
        /// </summary>
        /// <param name="endpoints"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        void Remove(IList<string> endpoints, string fileName);
        /// <summary>
        /// 开启异常处理
        /// </summary>
        void StartException();

    }
}
