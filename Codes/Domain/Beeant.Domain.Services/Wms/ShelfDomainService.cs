using System.Collections.Generic;
using System.Linq;
using Beeant.Domain.Entities;
using Beeant.Domain.Entities.Product;
using Beeant.Domain.Entities.Wms;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Domain.Services.Wms
{
    public class ShelfDomainService : RealizeDomainService<ShelfEntity>
    {

  

        #region 重写验证



        #region 添加验证
        /// <summary>
        /// 删除添加
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateAdd(ShelfEntity info)
        {
            var rev = ValidateStorehouse(info, null);
            if (rev) rev = ValidateProduct(info, null);
            if (rev) rev = ValidateExist(info);
            return rev;
        }


        #endregion

        #region 修改验证
        /// <summary>
        /// 删除添加
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateRemove(ShelfEntity info)
        {
            var dataEntity = Repository.Get<ShelfEntity>(info.Id);
            var rev = ValidateCount(info, dataEntity);
            return rev;
        }
        /// <summary>
        /// 验证数量
        /// </summary>
        /// <param name="info"></param>
        /// <param name="dataEntity"></param>
        /// <returns></returns>
        protected virtual bool ValidateCount(ShelfEntity info, ShelfEntity dataEntity)
        {
            if (dataEntity.Count != 0)
            {
                info.AddError("HasCountNotAllowRemove");
                return false;
            }
            return true;
        }

        #endregion

        /// <summary>
        /// 验证商品
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateExist(ShelfEntity info)
        {
            var query = new QueryInfo();
            query.Query<ShelfEntity>().Where(it => it.Product.Id == info.Product.Id && it.Storehouse.Id == info.Storehouse.Id).Select(it => new object[] { it.Id });
            var infos = Repository.GetEntities<ShelfEntity>(query);
            if (infos == null || infos.Count == 0)
                return true;
            info.AddError("Exist");
            return false;
        }

        /// <summary>
        /// 验证商品
        /// </summary>
        /// <param name="info"></param>
        /// <param name="dataEntity"></param>
        /// <returns></returns>
        protected virtual bool ValidateProduct(ShelfEntity info, ShelfEntity dataEntity)
        {
            if (!info.HasSaveProperty(it => it.Product.Id))
                return true;
            if (info.Product != null && info.Product.SaveType == SaveType.Add)
                return true;
            if (info.Product != null && info.Product.Id != 0)
            {
                if (dataEntity != null && dataEntity.Product != null && dataEntity.Product.Id == info.Product.Id)
                    return true;
                if (Repository.Get<ProductEntity>(info.Product.Id) != null)
                    return true;
            }
            info.AddErrorByName(typeof(ProductEntity).FullName, "NoExist");
            return false;
        }
    

        /// <summary>
        /// 验证仓库
        /// </summary>
        /// <param name="info"></param>
        /// <param name="dataEntity"></param>
        /// <returns></returns>
        protected virtual bool ValidateStorehouse(ShelfEntity info, ShelfEntity dataEntity)
        {

            if (!info.HasSaveProperty(it => it.Storehouse.Id))
                return true;
            if (info.Storehouse != null && info.Storehouse.SaveType == SaveType.Add)
                return true;
            if (info.Storehouse != null && info.Storehouse.Id!=0)
            {
                if (dataEntity != null && dataEntity.Storehouse != null && dataEntity.Storehouse.Id == info.Storehouse.Id)
                    return true;
                var storehouse = Repository.Get<StorehouseEntity>(info.Storehouse.Id);
                if (storehouse == null)
                {
                    info.AddErrorByName(typeof(StorehouseEntity).FullName, "NoExist");
                    return false;
                }
                if (!storehouse.IsUsed)
                {
                    info.AddErrorByName(typeof(StorehouseEntity).FullName, "UnUsed");
                    return false;
                }
                return true;
            }
            info.AddErrorByName(typeof(StorehouseEntity).FullName, "NoExist");
            return false;
        }
        #endregion


    }
}
