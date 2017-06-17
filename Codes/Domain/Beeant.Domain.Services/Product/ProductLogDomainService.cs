using System.Collections.Generic;
using System.Linq;
using Component.Extension;
using Beeant.Domain.Entities;
using Beeant.Domain.Entities.Authority;
using Beeant.Domain.Entities.Management;
using Beeant.Domain.Entities.Product;
using Winner.Persistence;
using Winner.Persistence.Linq;
using ProductEntity = Beeant.Domain.Entities.Product.ProductEntity;

namespace Beeant.Domain.Services.Product
{
    public class ProductLogDomainService : RealizeDomainService<ProductLogEntity>
    {

        /// <summary>
        /// 商品
        /// </summary>
        public IDomainService ProductDomainService { get; set; }


        private IDictionary<string, IUnitofworkHandle> _itemHandles;
        /// <summary>
        /// 处理
        /// </summary>
        protected override IDictionary<string, IUnitofworkHandle> ItemHandles
        {
            get
            {
                return _itemHandles ?? (_itemHandles = new Dictionary<string, IUnitofworkHandle>
                    {
                        {"Product", new UnitofworkHandle<ProductEntity>{DomainService= ProductDomainService}}
                    });
            }
            set
            {
                base.ItemHandles = value;
            }
        }
        private IDictionary<string, ItemLoader<ProductLogEntity>> _itemLoaders;
        /// <summary>
        /// 处理
        /// </summary>
        protected override IDictionary<string, ItemLoader<ProductLogEntity>> ItemLoaders
        {
            get
            {
                return _itemLoaders ?? (_itemLoaders = new Dictionary<string, ItemLoader<ProductLogEntity>>
                    {
                        {"WaitProductLog", LoadWaitProductLog}
                    });
            }
            set
            {
                base.ItemLoaders = value;
            }
        }

   
        /// <summary>
        /// 加载用户
        /// </summary>
        /// <param name="info"></param>
        protected virtual void LoadProduct(ProductLogEntity info)
        {
            info.Product = Repository.Get<ProductEntity>(info.Product.Id);
            if(info.Product!=null && info.Product.Goods!=null)
                info.Product.Goods = Repository.Get<GoodsEntity>(info.Product.Goods.Id);
            if (info.Product != null && info.Product.Goods!=null && info.Product.Goods.Category!=null)
                info.Product.Goods.Category = Repository.Get<CategoryEntity>(info.Product.Goods.Category.Id);
        }
        /// <summary>
        /// 加载用户
        /// </summary>
        /// <param name="info"></param>
        protected virtual void LoadWaitProductLog(ProductLogEntity info)
        {
            if(info.WaitProductLog==null || info.WaitProductLog.Id==0)
                return;
            info.WaitProductLog = Repository.Get<ProductLogEntity>(info.WaitProductLog.Id);
            if (info.WaitProductLog != null)
            {
                info.Product = info.WaitProductLog.Product;
                LoadProduct(info);
            }
        }
        /// <summary>
        /// 设置业务
        /// </summary>
        /// <param name="unitofworks"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool SetBusiness(IList<IUnitofwork> unitofworks, ProductLogEntity info)
        {
            var rev = base.SetBusiness(unitofworks, info);
            if (info.WaitProductLog != null && info.WaitProductLog.SaveType != SaveType.None)
                unitofworks.AddList(Repository.Save(info.WaitProductLog));
            return rev;
        }

        #region 重写验证


        #region 添加验证
        /// <summary>
        /// 删除添加
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateAdd(ProductLogEntity info)
        {
            return ValidateProduct(info) && ValidateUser(info);
        }


        /// <summary>
        /// 验证类目
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateProduct(ProductLogEntity info)
        {
            if (!info.HasSaveProperty(it => it.Product.Id))
                return true;
            if (info.Product.SaveType == SaveType.Add)
                return true;
            var product = Repository.Get<ProductEntity>(info.Product.Id);
            if (product == null)
            {
                info.AddErrorByName(typeof(ProductEntity).FullName, "NoExist");
                return false;
            }

            return true;
        }
       

        /// <summary>
        /// 验证类目
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateUser(ProductLogEntity info)
        {
            if (!info.HasSaveProperty(it => it.User.Id))
                return true;
            var user =info.User==null?null: Repository.Get<UserEntity>(info.User.Id);
            if (user == null)
            {
                info.AddErrorByName(typeof(UserEntity).FullName, "NoExist");
                return false;
            }
            if (!user.IsUsed)
            {
                info.AddErrorByName(typeof(UserEntity).FullName, "UnUsed");
                return false;
            }
            return true;
        }
        #endregion

    

        #endregion
    }
}
