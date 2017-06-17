using System;
using System.Collections.Generic;
using System.Linq;
using Beeant.Domain.Entities.Utility;
using Winner.Log;
using Winner.Persistence;
using Winner.Storage;

namespace Beeant.Repository.Services.Utility
{
 

    public class FileSaveUnitofwork : IUnitofwork
    {
        protected  IList<FileEntity> Files { get; set; }
        public FileSaveUnitofwork(IEnumerable<FileEntity> files)
        {
            if (files != null)
                Files =
                    files.Where(
                        it =>
                        it != null && !string.IsNullOrEmpty(it.FileName) && it.FileByte != null &&
                        it.FileByte.Length > 0).ToList();
        }

        #region 接口的实现
        public IList<object> Entities
        {
            get { return null; }
        }

        public bool IsExcute { get; set; }
        public bool IsDispose { get; set; }

        /// <summary>
        /// 执行
        /// </summary>
        public virtual void Execute()
        {
            try
            {
                if (Files == null || Files.Count == 0) return;
                foreach (var file in Files)
                {
                    Winner.Creator.Get<IFile>().Save(file.FileName, file.FileByte);
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
   
        }

        /// <summary>
        /// 回滚
        /// </summary>
        public virtual void Rollback()
        {
            try
            {
                if (Files == null || Files.Count == 0) return;
                foreach (var file in Files)
                {
                    Winner.Creator.Get<IFile>().Remove(file.FileName);
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
