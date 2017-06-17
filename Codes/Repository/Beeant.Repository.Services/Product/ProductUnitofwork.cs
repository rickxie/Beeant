using System;
using System.Collections.Generic;
using Component.Extension;
using Beeant.Domain.Entities.Product;
using Beeant.Domain.Services.Product;
using Winner;
using Winner.Log;
using Winner.Persistence;
using Winner.Wcf;

namespace Beeant.Repository.Services.Product
{


    public class ProductUnitofwork : IUnitofwork
    {
        public IWcfService WcfService { get; set; }
        protected ProductEntity Product { get; set; }
        public ProductUnitofwork(ProductEntity product, IWcfService wcfService)
        {
            Product = product;
            WcfService = wcfService;

        }

        #region 接口的实现
        public IList<object> Entities
        {
            get { return null; }
        }

        public bool IsExcute { get; set; }
        public bool IsDispose { get; set; }

        /// <summary>
        /// 执行
        /// </summary>
        public virtual void Execute()
        {
            if(Product==null)
                return;
            var rev = WcfService.Invoke<IProductContract>(SaveProduct, GetEndPoints, Product);
            if (rev == null)
               throw new Exception("ProductCountNotEnough");
            
        }

        /// <summary>
        /// 提交
        /// </summary>
        public virtual void Commit()
        {
   
        }

        /// <summary>
        /// 回滚
        /// </summary>
        public virtual void Rollback()
        {
            try
            {
                if (Product == null || Product.Id == 0) return;
                Product.Count = Product.Count - Product.ChangeCount;
                Product.SaveType = SaveType.Modify;
                Product.SetProperty(it => it.Count);
                var unitofworks= Creator.Get<IContext>().Save();
                Creator.Get<IContext>().Commit(unitofworks);
            }
            catch (Exception ex)
            {

                Creator.Get<ILog>().AddException(ex);
            }
        }
        /// <summary>
        /// 得到节点
        /// </summary>
        /// <param name="endPoints"></param>
        /// <param name="paramters"></param>
        /// <returns></returns>
        protected virtual IList<EndPointInfo> GetEndPoints(IList<EndPointInfo> endPoints,
            params object[] paramters)
        {
            if (endPoints == null || endPoints.Count==0)
                return null;
            var product = paramters[0] as ProductEntity;
            if (product == null)
                return null;
            var endPoint = endPoints[(int) (product.Id%endPoints.Count)];
            return new List<EndPointInfo> { endPoint }; 
        }
        /// <summary>
        /// 存储
        /// </summary>
        /// <param name="service"></param>
        /// <param name="paramters"></param>
        /// <returns></returns>
        public virtual object SaveProduct(IProductContract service, params object[] paramters)
        {
            var info = service.Save(paramters[0].SerializeJson());
            return info;
        }
        #endregion
    }
}
