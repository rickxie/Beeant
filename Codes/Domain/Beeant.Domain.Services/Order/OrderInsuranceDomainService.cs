using System.Collections.Generic;
using Beeant.Domain.Entities.Order;
using Beeant.Domain.Entities.Product;
using Winner.Persistence;
using FileEntity = Beeant.Domain.Entities.Utility.FileEntity;

namespace Beeant.Domain.Services.Order
{
    public class OrderInsuranceDomainService : RealizeDomainService<OrderInsuranceEntity>
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


        #region 验证添加

        /// <summary>
        /// 删除添加
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateAdd(OrderInsuranceEntity info)
        {
            OrderEntity order;
            var rev = ValidateOrder(info, out order);
            if (!rev) return false;
            return ValidateProduct(info) ;
        }

        /// <summary>
        /// 验证订单
        /// </summary>
        /// <param name="info"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        protected virtual bool ValidateOrder(OrderInsuranceEntity info, out OrderEntity order)
        {
            order = info.Order == null ? null
                           : info.Order.SaveType == SaveType.Add
                                 ? info.Order
                                 : Repository.Get<OrderEntity>(info.Order.Id);
            if (order == null)
            {
                info.AddErrorByName(typeof(OrderEntity).FullName, "NoExist");
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
        protected virtual bool ValidateProduct(OrderInsuranceEntity info)
        {
            if (!info.HasSaveProperty(it => it.Product.Id) || info.Product == null || info.Product.Id == 0)
            {
                return true;
            }
            if (info.Product != null && info.Product.SaveType == SaveType.Add)
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
