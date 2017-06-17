using System.Collections.Generic;
using Beeant.Domain.Entities.Product;
using Beeant.Domain.Entities.Utility;
using Winner.Persistence;

namespace Beeant.Domain.Services.Product
{
    public class GoodsImageDomainService : RealizeDomainService<GoodsImageEntity>
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

        #region 添加验证
        /// <summary>
        /// 添加验证
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateAdd(GoodsImageEntity info)
        {
            var rev = ValidateGoods(info,null) && ValidateProduct(info);
            return rev;
        }
        #endregion
        #region 修改验证
        /// <summary>
        /// 添加验证
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateModify(GoodsImageEntity info)
        {
            var dataEntity = Repository.Get<GoodsImageEntity>(info.Id);
            var rev = ValidateGoods(info, dataEntity);
            return rev;
        }
        #endregion
        /// <summary>
        /// 验证类目
        /// </summary>
        /// <param name="info"></param>
        /// <param name="dataEntity"></param>
        /// <returns></returns>
        protected virtual bool ValidateGoods(GoodsImageEntity info, GoodsImageEntity dataEntity)
        {
            if (!info.HasSaveProperty(it => it.Goods.Id))
                return true;
            if (dataEntity != null && dataEntity.Goods != null && info.Goods != null && info.Goods.Id == dataEntity.Goods.Id)
                return true;
            if (info.Goods != null && info.Goods.SaveType == SaveType.Add)
                return true;
            if (info.Goods != null && info.Goods.Id!=0)
            {
                var goods = Repository.Get<GoodsEntity>(info.Goods.Id);
                if (goods == null)
                {
                    info.AddErrorByName(typeof(GoodsEntity).FullName, "NoExist");
                    return false;
                }
                return true;
            }
            info.AddErrorByName(typeof(GoodsEntity).FullName, "NoExist");
            return false;
        }

        /// <summary>
        /// 验证商品
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateProduct(GoodsImageEntity info)
        {
            if (!info.HasSaveProperty(it => it.Product.Id))
                return true;
            if (info.Product != null && info.Product.Id == 0)
                return true;
            if (info.Product != null && info.Product.SaveType == SaveType.Add)
                return true;
            if (info.Product != null && info.Product.Id != 0)
            {
                var product = Repository.Get<ProductEntity>(info.Product.Id);
                if (product == null)
                {
                    info.AddErrorByName(typeof(ProductEntity).FullName, "NoExist");
                    return false;
                }
                return true;
            }
            info.AddErrorByName(typeof(ProductEntity).FullName, "NoExist");
            return false;
        }
        #endregion
      
    }
}
