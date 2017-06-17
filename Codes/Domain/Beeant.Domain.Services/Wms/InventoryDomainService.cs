using System.Linq;
using Beeant.Domain.Entities.Product;
using Beeant.Domain.Entities.Wms;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Domain.Services.Wms
{
    public class InventoryDomainService : RealizeDomainService<InventoryEntity>
    {
      
        #region 重写验证

    

        #region 添加验证
        /// <summary>
        /// 删除添加
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateAdd(InventoryEntity info)
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
        protected override bool ValidateRemove(InventoryEntity info)
        {
            var dataEntity = Repository.Get<InventoryEntity>(info.Id);
            var rev = ValidateCount(info, dataEntity);
            return rev;
        }
        /// <summary>
        /// 验证数量
        /// </summary>
        /// <param name="info"></param>
        /// <param name="dataEntity"></param>
        /// <returns></returns>
        protected virtual bool ValidateCount(InventoryEntity info, InventoryEntity dataEntity)
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
        protected virtual bool ValidateExist(InventoryEntity info)
        {
            var query = new QueryInfo();
            query.Query<InventoryEntity>().Where(it => it.Product.Id == info.Product.Id && it.Storehouse.Id == info.Storehouse.Id).Select(it => new object[] { it.Id });
            var infos = Repository.GetEntities<InventoryEntity>(query);
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
        protected virtual bool ValidateProduct(InventoryEntity info, InventoryEntity dataEntity)
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
        protected virtual bool ValidateStorehouse(InventoryEntity info, InventoryEntity dataEntity)
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
