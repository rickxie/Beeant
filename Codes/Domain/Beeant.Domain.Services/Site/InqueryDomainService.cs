using System;
using Beeant.Domain.Entities.Site;
using Winner.Persistence;

namespace Beeant.Domain.Services.Site
{
    public class InqueryDomainService : RealizeDomainService<InqueryEntity>
    {
   

        #region 重写验证

        /// <summary>
        /// 添加验证
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateAdd(InqueryEntity info)
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
        protected virtual bool ValidateSite(InqueryEntity info)
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
