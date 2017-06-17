using System.Linq;
using Beeant.Domain.Entities.Product;
using Beeant.Domain.Entities.Promotion;
using Winner.Persistence;
using Winner.Persistence.Linq;


namespace Beeant.Domain.Services.Promotion
{
    public class PromotionDomainService : RealizeDomainService<PromotionEntity>
    {
 
        #region 添加验证
        /// <summary>
        /// 添加验证
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateAdd(PromotionEntity info)
        {
            return ValidateDeliveryDate(info,null) && ValidateProduct(info);
        }
        #endregion

        #region 修改验证
        /// <summary>
        /// 修改验证
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateModify(PromotionEntity info)
        {
            var dataEntity = Repository.Get<PromotionEntity>(info.Id);

            return ValidateDeliveryDate(info, dataEntity) ;
        }

        #endregion

        /// <summary>
        /// 验证产品
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateProduct(PromotionEntity info)
        {
            var query = new QueryInfo();
            query.Query<ProductEntity>()
                 .Where(it => it.Id == info.Product.Id);
            var infos = Repository.GetEntities<ProductEntity>(query);
            if (infos == null || infos.Count == 0)
            {
                info.AddErrorByName(typeof(ProductEntity).FullName, "NoExist");
                return false;
            }
            return true;
        }
        /// <summary>
        /// 验证日期
        /// </summary>
        /// <param name="info"></param>
        /// <param name="dataEntity"></param>
        /// <returns></returns>
        protected virtual bool ValidateDeliveryDate(PromotionEntity info, PromotionEntity dataEntity)
        {
            if (!info.HasSaveProperty(it => it.BeginDate) && !info.HasSaveProperty(it => it.EndDate))
                return true;
            var orderData = dataEntity != null && !info.HasSaveProperty(it => it.BeginDate)
                                ? dataEntity.BeginDate
                                : info.BeginDate;
            var deliveryDate = dataEntity != null && !info.HasSaveProperty(it => it.EndDate)
                           ? dataEntity.EndDate
                           : info.EndDate;
            if (orderData > deliveryDate)
            {
                info.AddError("DeliveryDateLessPurchaseDate");
                return false;
            }
            return true;
        }
     
    }
}
