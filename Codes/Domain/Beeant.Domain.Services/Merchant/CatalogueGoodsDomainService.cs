using System.Collections.Generic;
using System.Linq;
using Beeant.Domain.Entities;
using Beeant.Domain.Entities.Product;
using Beeant.Domain.Entities.Merchant;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Domain.Services.Merchant
{
    public class CatalogueGoodsDomainService : RealizeDomainService<CatalogueGoodsEntity>
    {
    

        /// <summary>
        /// 是否有重复信息添加
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        protected override bool ValidateBatch(IList<CatalogueGoodsEntity> infos)
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
        protected virtual bool CompareEntities(CatalogueGoodsEntity info, CatalogueGoodsEntity compare)
        {
            string errorName = null;
            if (info.Catalogue.Id.Equals(compare.Catalogue.Id) && info.Goods.Id.Equals(compare.Goods.Id))
                errorName = "RepeatInList";
            if (string.IsNullOrEmpty(errorName)) return true;
            info.AddErrorByName(typeof(BaseEntity).FullName, errorName);
            return false;
        }
  

        /// <summary>
        /// 验证添加
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateAdd(CatalogueGoodsEntity info)
        {
            return ValidateCatalogueExist(info) && ValidateGoodsExist(info)
                && ValidateCatalogueGoodsExist(info) && ValidateAccountEqual(info);
        }



        /// <summary>
        /// 验证RoleAbility是否已经存在
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateCatalogueGoodsExist(CatalogueGoodsEntity info)
        {
            if (info.Catalogue != null && info.Catalogue.SaveType == SaveType.Add ||
                info.Goods != null && info.Goods.SaveType == SaveType.Add)
                return true;
            var query = new QueryInfo();
            query.Query<CatalogueGoodsEntity>().Where(it => it.Catalogue.Id == info.Catalogue.Id
                                                      && it.Goods.Id == info.Goods.Id);
            var infos = Repository.GetEntities<CatalogueGoodsEntity>(query);
            if (infos == null || infos.Count == 0) return true;
            info.AddError("ExistCatalogueGoods");
            return false;
        }

        /// <summary>
        /// 验证角色类型
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateCatalogueExist(CatalogueGoodsEntity info)
        {
            if (!info.HasSaveProperty(it => it.Catalogue.Id))
                return true;
            if (info.Catalogue != null && info.Catalogue.SaveType == SaveType.Add)
                return true;
            if (info.Catalogue != null && info.Catalogue.Id!=0)
            {
                if (Repository.Get<CatalogueEntity>(info.Catalogue.Id) != null)
                    return true;
            }
            info.AddErrorByName(typeof(CatalogueEntity).FullName, "NoExist");
            return false;
        }

        /// <summary>
        /// 验证功能类型
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateGoodsExist(CatalogueGoodsEntity info)
        {
            if (!info.HasSaveProperty(it => it.Goods.Id))
                return true;
            if (info.Goods != null && info.Goods.SaveType == SaveType.Add)
                return true;
            if (info.Goods != null && info.Goods.Id!=0)
            {
                if (Repository.Get<GoodsEntity>(info.Goods.Id) != null)
                    return true;
            }
            info.AddErrorByName(typeof(GoodsEntity).FullName, "NoExist");
            return false;
        }
        /// <summary>
        /// 验证账户是否相等
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateAccountEqual(CatalogueGoodsEntity info)
        {
            var catalogue = info.Catalogue != null && info.Catalogue.SaveType == SaveType.Add
                                ? info.Catalogue
                                : Repository.Get<CatalogueEntity>(info.Catalogue.Id);
            var goods = info.Goods != null && info.Goods.SaveType == SaveType.Add
                             ? info.Goods
                             : Repository.Get<GoodsEntity>(info.Goods.Id);
            if (goods.Account == null && catalogue.Account == null)
                return true;
            if (goods.Account != null && catalogue.Account != null && goods.Account.Id == catalogue.Account.Id)
                return true;
            info.AddError("AccountNoEqual");
            return false;
        }
      
    }
}
