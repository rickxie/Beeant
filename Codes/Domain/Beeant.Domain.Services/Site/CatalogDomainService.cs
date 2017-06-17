using System;
using System.Collections.Generic;
using System.Linq;
using Beeant.Domain.Entities.Site;
using Beeant.Domain.Entities.Utility;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Domain.Services.Site
{
    public class CatalogDomainService : RealizeDomainService<CatalogEntity>
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
        protected override bool ValidateAdd(CatalogEntity info)
        {
            var rev = ValidateSite(info) && ValidateName(info);
            return rev;
        }


        /// <summary>
        /// 添加验证
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateModify(CatalogEntity info)
        {
            var dataInfo = Repository.Get<CatalogEntity>(info.Id);
            if (dataInfo == null)
                return false;
            info.Site = dataInfo.Site;
            var rev = ValidateName(info);
            return rev;
        }


        #endregion

        /// <summary>
        /// 验证员工
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateName(CatalogEntity info)
        {
            if (!info.HasSaveProperty(it => it.Name))
                return true;
            var query=new QueryInfo();
            query.Query<CatalogEntity>().Where(it => it.Name == info.Name.Trim() && it.Site.Id==info.Site.Id)
                .Select(it => it.Id);
            var infos = Repository.GetEntities<CatalogEntity>(query);
            var existInfo = infos?.FirstOrDefault();
            if (existInfo == null ||  info.Id == existInfo.Id)
            {
                return true;
            }
            info.AddError("NameExist");
            return false;
        }
        /// <summary>
        /// 验证员工
        /// </summary>
        /// <param name="info"></param>

        /// <returns></returns>
        protected virtual bool ValidateSite(CatalogEntity info)
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
            if (site.ExpireDate<DateTime.Now)
            {
                info.AddErrorByName(typeof(SiteEntity).FullName, "ExpireDateOver");
                return false;
            }
            
            return true;
        }
    
    }
}
