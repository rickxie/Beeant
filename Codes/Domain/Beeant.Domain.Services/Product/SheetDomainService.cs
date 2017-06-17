using System.Linq;
using Beeant.Domain.Entities.Product;
using Winner.Persistence;
using Winner.Persistence.Linq;
using ProductEntity = Beeant.Domain.Entities.Product.ProductEntity;

namespace Beeant.Domain.Services.Product
{
    public class SheetDomainService : RealizeDomainService<SheetEntity>
    {
    

        #region 重写验证
             

        /// <summary>
        /// 验证添加操作
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateAdd(SheetEntity info)
        {
            var rev = ValidateScmProduct(info) && ValidateProduct(info) && ValidateSheetExist(info);
            return rev;
        }

 

        /// <summary>
        /// 验证数据完整性
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateSheetExist(SheetEntity info)
        {
            if (info.FactProduct.SaveType == SaveType.Add || info.Product.SaveType == SaveType.Add)
                return true;
            var query = new QueryInfo();
            query.Query<SheetEntity>().Where(it => it.Product.Id == info.Product.Id && it.FactProduct.Id == info.FactProduct.Id);
            var sheetinfos = Repository.GetEntities<SheetEntity>(query);
            if (sheetinfos != null && sheetinfos.Count > 0)
            {
                info.AddError("ExistSheet");
                return false;
            }
            return true;
        }


        /// <summary>
        /// 验证采购订单信息
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateScmProduct(SheetEntity info)
        {
            if (!info.HasSaveProperty(it => it.FactProduct.Id) || info.FactProduct.SaveType == SaveType.Add)
                return true;

            var goodsProduct = Repository.Get<ProductEntity>(info.FactProduct.Id);
            if (goodsProduct == null)
            {
                info.AddErrorByName(typeof (ProductEntity).FullName, "NoExist");
                return false;
            }
            return true;
        }

        /// <summary>
        /// 验证入库单信息
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateProduct(SheetEntity info)
        {
            if (info.HasSaveProperty(it => it.Product.Id) || info.Product.SaveType == SaveType.Add)
                return true;
            var product = Repository.Get<ProductEntity>(info.Product.Id);
            if (product == null)
            {
                info.AddErrorByName(typeof(ProductEntity).FullName, "NoExist");
                return false;
            }           
            return true;
        }

      
        #endregion
    }
}
