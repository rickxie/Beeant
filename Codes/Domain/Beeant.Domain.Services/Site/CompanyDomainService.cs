using System;
using System.Collections.Generic;
using Beeant.Domain.Entities.Basedata;
using Beeant.Domain.Entities.Site;
using Beeant.Domain.Entities.Utility;
using Winner.Persistence;

namespace Beeant.Domain.Services.Site
{
    public class CompanyDomainService : RealizeDomainService<CompanyEntity>
    {
        private IList<FileEntity> _fileProperties = new List<FileEntity>
            {
               new FileEntity {FilePropertyName = "WeixinQrCodeFileName",BytePropertyName = "WeixinQrCodeFileByte"}
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
        protected override bool ValidateAdd(CompanyEntity info)
        {
            var rev = ValidateSite(info) && ValidateAlbum(info,null);
            return rev;
        }

        /// <summary>
        /// 添加验证
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateModify(CompanyEntity info)
        {
            var dataInfo = Repository.Get<CompanyEntity>(info.Id);
            var rev = ValidateAlbum(info, dataInfo);
            return rev;
        }


        #endregion

        /// <summary>
        /// 验证员工
        /// </summary>
        /// <param name="info"></param>

        /// <returns></returns>
        protected virtual bool ValidateSite(CompanyEntity info)
        {
            if (!info.HasSaveProperty(it => it.Site.Id) || info.Site == null)
                return true;
            if (info.Site != null && info.Site.SaveType == SaveType.Add)
                return true;
            var site = Repository.Get<SiteEntity>(info.Site.Id);
            if (site == null)
            {
                info.AddErrorByName(typeof(SiteEntity).FullName, "NoExist");
                return false;
            }
            if (site.ExpireDate<DateTime.Now)
            {
                info.AddErrorByName(typeof(SiteEntity).FullName, "ExpireDateOver");
                return false;
            }
            
            return true;
        }

        /// <summary>
        /// 验证员工
        /// </summary>
        /// <param name="info"></param>
        /// <param name="dataInfo"></param>
        /// <returns></returns>
        protected virtual bool ValidateAlbum(CompanyEntity info, CompanyEntity dataInfo)
        {
            if (!info.HasSaveProperty(it => it.Album.Id) || info.Album == null || info.Album.Id == 0)
                return true;
            if (info.Album != null && info.Album.SaveType == SaveType.Add)
                return true;
            if (dataInfo != null && dataInfo.Album != null && dataInfo.Album.Id == info.Album.Id)
                return true;
            var site = Repository.Get<AlbumEntity>(info.Album.Id);
            if (site == null)
            {
                info.AddErrorByName(typeof(AlbumEntity).FullName, "NoExist");
                return false;
            }
          
            return true;
        }
    }
}
