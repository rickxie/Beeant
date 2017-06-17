using System;
using System.Linq;
using Beeant.Domain.Entities.Site;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Domain.Services.Site
{
    public class TagDomainService : RealizeDomainService<TagEntity>
    {

      
        #region 重写验证

        /// <summary>
        /// 添加验证
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateAdd(TagEntity info)
        {
            var rev = ValidateSite(info) && ValidateName(info);
            return rev;
        }

        /// <summary>
        /// 添加验证
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateModify(TagEntity info)
        {
            var dataInfo = Repository.Get<TagEntity>(info.Id);
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
        protected virtual bool ValidateName(TagEntity info)
        {
            if (!info.HasSaveProperty(it => it.Name))
                return true;
            var query=new QueryInfo();
            query.Query<TagEntity>().Where(it => it.Name == info.Name.Trim() && it.Site.Id==info.Site.Id)
                .Select(it => it.Id);
            var infos = Repository.GetEntities<TagEntity>(query);
            var existInfo = infos?.FirstOrDefault();
            if (existInfo == null || info.Id == existInfo.Id)
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
        protected virtual bool ValidateSite(TagEntity info)
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
