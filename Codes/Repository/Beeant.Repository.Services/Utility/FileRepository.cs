using System;
using System.Collections.Generic;
using Beeant.Domain.Entities;
using Beeant.Domain.Entities.Utility;
using Beeant.Domain.Services.Utility;
using Winner.Base;
using Winner.Log;
using Winner.Persistence;
using Winner.Storage;

namespace Beeant.Repository.Services.Utility
{
    public class FileRepository : IFileRepository
    {
        #region 接口实现

        ///  <summary>
        /// 得到文件存储事务对象
        ///  </summary>
        /// <param name="info"></param>
        /// <param name="files"></param>
        /// <returns></returns>
        public virtual IUnitofwork GetSaveUnitofwork<T>(T info, IList<FileEntity> files) where T : BaseEntity
        {
            if (files == null || files.Count == 0) 
                return null;
            var saveFiles = new List<FileEntity>();
            AddSaveFiles(info, files, saveFiles);
            if (saveFiles.Count == 0) 
                return null;
            return GetSaveUnitofwork(saveFiles);
        }

        /// <summary>
        /// 得到存储文件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="info"></param>
        /// <param name="files"></param>
        /// <param name="saveFiles"></param>
        /// <returns></returns>
        protected virtual void AddSaveFiles<T>(T info, IList<FileEntity> files, IList<FileEntity> saveFiles) where T : BaseEntity
        {
            if (info != null && (info.SaveType == SaveType.Add || info.SaveType == SaveType.Modify))
            {
                foreach (var file in files)
                {
                    var saveFile = new FileEntity
                        {
                            FilePropertyName = file.FilePropertyName,
                            BytePropertyName = file.BytePropertyName,
                            FileName = Winner.Creator.Get<IProperty>().GetValue<string>(info, file.FilePropertyName),
                            FileByte = Winner.Creator.Get<IProperty>().GetValue<byte[]>(info, file.BytePropertyName),
                        };
                    if (saveFile.FileByte == null || saveFile.FileByte.Length == 0 || string.IsNullOrEmpty(saveFile.FileName)) 
                        continue;
                    saveFiles.Add(saveFile);
                }
            }
        }
        #endregion
       

        /// <summary>
        /// 得到文件存储事务对象
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        public virtual IUnitofwork GetSaveUnitofwork(IList<FileEntity> files)
        {
            return new FileSaveUnitofwork(files);
        }

        /// <summary>
        /// 得到文件删除事务对象
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        public virtual IUnitofwork GetRemoveUnitofwork(IList<string> files)
        {
            if (files == null || files.Count == 0) return null;
            return new FileRemoveUnitofwork(files);
        }

       

        ///  <summary>
        /// 存储文件,返回存储后的文件路径
        ///  </summary>
        /// <param name="fileName"></param>
        /// <param name="fileByte"></param>
        /// <returns></returns>
        public virtual void Save(string fileName, byte[] fileByte)
        {
            try
            {
                Winner.Creator.Get<IFile>().Save(fileName, fileByte);
            }
            catch (Exception ex)
            {
                Winner.Creator.Get<ILog>().AddException(ex);
            }
          
        }

      
       
        /// <summary>
        /// 删除文件或者目录
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public virtual void Remove(string fileName)
        {
            try
            {
                Winner.Creator.Get<IFile>().Remove(fileName);
            }
            catch (Exception ex)
            {
                Winner.Creator.Get<ILog>().AddException(ex);
            }
           
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public virtual byte[] Download(string fileName)
        {
            try
            {
                return Winner.Creator.Get<IFile>().Download(fileName);
            }
            catch (Exception ex)
            {
                Winner.Creator.Get<ILog>().AddException(ex);
            }
            return null;
        }
        /// <summary>
        /// 本地存储
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="fileByte"></param>
        public virtual void SaveLocal(string fileName, byte[] fileByte)
        {
            try
            {
                Winner.Creator.Get<IFile>("Winner.Storage.ILocalFile").Save(fileName, fileByte);
            }
            catch (Exception ex)
            {
                Winner.Creator.Get<ILog>().AddException(ex);
            }
           
        }
        /// <summary>
        /// 移除本地
        /// </summary>
        /// <param name="fileName"></param>
        public virtual void RemoveLocal(string fileName)
        {
            try
            {
                Winner.Creator.Get<IFile>("Winner.Storage.ILocalFile").Remove(fileName);
            }
            catch (Exception ex)
            {
                Winner.Creator.Get<ILog>().AddException(ex);
            }
          
        }
        /// <summary>
        /// 本地读取
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public virtual byte[] DownloadLocal(string fileName)
        {
            try
            {
                return Winner.Creator.Get<IFile>("Winner.Storage.ILocalFile").Download(fileName);
            }
            catch (Exception ex)
            {
                Winner.Creator.Get<ILog>().AddException(ex);
            }
            return null;
        }
     
    }


}
