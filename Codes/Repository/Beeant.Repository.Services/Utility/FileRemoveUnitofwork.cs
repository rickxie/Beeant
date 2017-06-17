using System;
using System.Collections.Generic;
using Beeant.Domain.Entities.Utility;
using Winner.Log;
using Winner.Persistence;
using Winner.Storage;

namespace Beeant.Repository.Services.Utility
{
 

    public class FileRemoveUnitofwork : IUnitofwork
    {
        protected  IList<string> Files { get; set; }
        public FileRemoveUnitofwork(IList<string> files)
        {
            Files = files;
        }

        #region 接口的实现
        public IList<object> Entities
        {
            get { return null; }
        }

        public bool IsExcute { get; set; }
        public bool IsDispose { get; set; }
        protected IList<FileEntity> TempFiles { get; set; }
        /// <summary>
        /// 执行
        /// </summary>
        public virtual void Execute()
        {
            try
            {
                if (Files == null || Files.Count == 0) return;
                TempFiles = new List<FileEntity>();
                foreach (var fileName in Files)
                {
                    var fileByte = Winner.Creator.Get<IFile>().Download(fileName);
                    TempFiles.Add(new FileEntity { FileName = fileName, FileByte = fileByte });
                    Winner.Creator.Get<IFile>().Remove(fileName);
                }
            }
            catch (Exception ex)
            {

                Winner.Creator.Get<ILog>().AddException(ex);
            }
           
      
        }

        /// <summary>
        /// 提交
        /// </summary>
        public virtual void Commit()
        {
            try
            {
                if (Files == null || Files.Count == 0) return;
                TempFiles = null;

            }
            catch (Exception ex)
            {

                Winner.Creator.Get<ILog>().AddException(ex);
            }
           
        }

        /// <summary>
        /// 回滚
        /// </summary>
        public virtual void Rollback()
        {
            try
            {
                if (Files == null || Files.Count == 0 || TempFiles == null)
                    return;
                foreach (var file in TempFiles)
                {
                    Winner.Creator.Get<IFile>().Save(file.FileName, file.FileByte);
                }
            }
            catch (Exception ex)
            {

                Winner.Creator.Get<ILog>().AddException(ex);
            }
           
      
        }
        #endregion
    }
}
