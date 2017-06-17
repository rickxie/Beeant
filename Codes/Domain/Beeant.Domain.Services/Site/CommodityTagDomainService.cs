using System.Collections.Generic;
using System.Linq;
using Beeant.Domain.Entities.Site;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Domain.Services.Site
{
    public class CommodityTagDomainService : RealizeDomainService<CommodityTagEntity>
    {
      

        /// <summary>
        /// 是否有重复信息添加
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        protected override bool ValidateBatch(IList<CommodityTagEntity> infos)
        {
            bool rev = true;
            for (int i = 0; i < infos.Count; i++)
                for (int k = i + 1; k < infos.Count; k++)
                {
                    rev &= CompareEntities(infos[i], infos[k]);
                }
            return rev;
        }
        /// <summary>
        /// 比较
        /// </summary>
        /// <param name="info"></param>
        /// <param name="compare"></param>
        /// <returns></returns>
        protected virtual bool CompareEntities(CommodityTagEntity info, CommodityTagEntity compare)
        {
            //string errorName = null;
            //if (info.SaveType==SaveType.Add && info.Commodity.Id.Equals(compare.Commodity.Id) && info.Tag.Id.Equals(compare.Tag.Id))
            //    errorName = "RepeatInList";
            //if (string.IsNullOrEmpty(errorName)) return true;
            //info.AddErrorByName(typeof(BaseEntity).FullName, errorName);
            //return false;
            return true;
        }
   
        /// <summary>
        /// 验证添加
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateAdd(CommodityTagEntity info)
        {
            return ValidateCommodityExist(info) && ValidateTagExist(info) && ValidateTagCommodityExist(info);
        }



        /// <summary>
        /// 验证TagCommodity是否已经存在
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateTagCommodityExist(CommodityTagEntity info)
        {
            if (info.Tag != null && info.Tag.SaveType == SaveType.Add ||
                info.Commodity != null && info.Commodity.SaveType == SaveType.Add)
                return true;
            var query = new QueryInfo();
            query.Query<CommodityTagEntity>().Where(it => it.Commodity.Id == info.Commodity.Id
                                                      && it.Tag.Id == info.Tag.Id);
            var infos = Repository.GetEntities<CommodityTagEntity>(query);
            if (infos == null || infos.Count == 0) return true;
            info.AddError("ExistTagCommodity");
            return false;
        }

        /// <summary>
        /// 验证角色类型
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateTagExist(CommodityTagEntity info)
        {
            if (!info.HasSaveProperty(it => it.Tag.Id))
                return true;
            if (info.Tag != null && info.Tag.SaveType == SaveType.Add)
                return true;
            if (info.Tag != null && info.Tag.Id!=0)
            {
                if (Repository.Get<TagEntity>(info.Tag.Id) != null)
                    return true;
            }
            info.AddErrorByName(typeof(TagEntity).FullName, "NoExist");
            return false;
        }

        /// <summary>
        /// 验证功能类型
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateCommodityExist(CommodityTagEntity info)
        {
            if (!info.HasSaveProperty(it => it.Commodity.Id))
                return true;
            if (info.Commodity != null && info.Commodity.SaveType == SaveType.Add)
                return true;
            if (info.Commodity != null && info.Commodity.Id!=0)
            {
                if (Repository.Get<CommodityEntity>(info.Commodity.Id) != null)
                    return true;
            }
            info.AddErrorByName(typeof(CommodityEntity).FullName, "NoExist");
            return false;
        }
        /// <summary>
        /// 验证功能类型
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateCommodityTagSite(CommodityTagEntity info)
        {
            var tag = info.Tag.SaveType == SaveType.Add ? info.Tag : Repository.Get<TagEntity>(info.Tag.Id);
            var commodity = info.Commodity.SaveType == SaveType.Add ? info.Commodity : Repository.Get<CommodityEntity>(info.Commodity.Id);
            if (tag != null && tag.Site != null && commodity != null && commodity.Site != null && tag.Site.Id==commodity.Site.Id)
            {
                return true;
            }
            info.AddErrorByName(typeof(CommodityTagEntity).FullName, "SiteNotEqual");
            return false;
        }
    }
}
