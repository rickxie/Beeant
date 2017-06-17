
using System.Collections.Generic;
using Beeant.Domain.Entities;
using Beeant.Domain.Entities.Utility;
using Winner.Persistence;

namespace Beeant.Domain.Services.Utility
{
    public interface IFileRepository
    {
        ///  <summary>
        /// 得到文件存储事务对象
        ///  </summary>
        /// <param name="info"></param>
        /// <param name="files"></param>
        /// <returns></returns>
        IUnitofwork GetSaveUnitofwork<T>(T info, IList<FileEntity> files) where T : BaseEntity;
        ///  <summary>
        /// 得到文件存储事务对象
        ///  </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        IUnitofwork GetSaveUnitofwork(IList<FileEntity> files);

        ///  <summary>
        /// 得到文件删除事务对象
        ///  </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        IUnitofwork GetRemoveUnitofwork(IList<string> files);

        ///  <summary>
        /// 存储文件,返回存储后的文件路径
        ///  </summary>
        /// <param name="fileName"></param>
        /// <param name="fileByte"></param>
        /// <returns></returns>
        void Save(string fileName, byte[] fileByte);

        /// <summary>
        /// 删除文件或者目录
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        void Remove(string fileName);
        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        byte[] Download(string fileName);
        ///  <summary>
        /// 存储文件,返回存储后的文件路径
        ///  </summary>
        /// <param name="fileName"></param>
        /// <param name="fileByte"></param>
        /// <returns></returns>
        void SaveLocal(string fileName, byte[] fileByte);

        /// <summary>
        /// 删除文件或者目录
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        void RemoveLocal(string fileName);
        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        byte[] DownloadLocal(string fileName);
    }
}
