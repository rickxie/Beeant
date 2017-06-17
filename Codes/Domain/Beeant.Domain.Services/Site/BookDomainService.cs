using System;
using System.Collections.Generic;
using Beeant.Domain.Entities.Site;
using Beeant.Domain.Entities.Utility;
using Winner.Persistence;

namespace Beeant.Domain.Services.Site
{
    public class BookDomainService : RealizeDomainService<BookEntity>
    {
        private IList<FileEntity> _fileProperties = new List<FileEntity>
            {
               new FileEntity {FilePropertyName = "FileName",BytePropertyName = "FileByte"}
            };

        public override IList<FileEntity> FileProperties
        {
            get { return _fileProperties; }
            set { _fileProperties = value; }
        }

        #region 重写验证

        /// <summary>
        /// 添加验证
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateAdd(BookEntity info)
        {
            var rev = ValidateSite(info);
            return rev;
        }



        #endregion

        /// <summary>
        /// 验证员工
        /// </summary>
        /// <param name="info"></param>

        /// <returns></returns>
        protected virtual bool ValidateSite(BookEntity info)
        {
            if (!info.HasSaveProperty(it => it.Site.Id) || info.Site == null || info.Site.Id == 0)
                return true;
            if (info.Site != null && info.Site.SaveType == SaveType.Add)
                return true;
            var site = Repository.Get<SiteEntity>(info.Site.Id);
            if (site == null)
            {
                info.AddErrorByName(typeof(SiteEntity).FullName, "NoExist");
                return false;
            }
            if (site.ExpireDate < DateTime.Now)
            {
                info.AddErrorByName(typeof(SiteEntity).FullName, "ExpireDateOver");
                return false;
            }

            return true;
        }


    }
}
