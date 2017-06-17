using System.Collections.Generic;
using System.Linq;
using Beeant.Domain.Entities.Product;
using Beeant.Domain.Entities.Utility;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Domain.Services.Product
{
    public class GoodsDetailDomainService : RealizeDomainService<GoodsDetailEntity>
    {

        private IList<FileEntity> _fileProperties = new List<FileEntity>
            {
                new FileEntity {FilePropertyName = "Attachment", BytePropertyName = "AttachmentByte"}
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
        protected override bool ValidateAdd(GoodsDetailEntity info)
        {
            var rev = ValidateGoods(info,null) && ValidateProduct(info) && ValidateExist(info);
            return rev;
        }
        #endregion
        #region 修改验证
        /// <summary>
        /// 添加验证
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateModify(GoodsDetailEntity info)
        {
            var dataEntity = Repository.Get<GoodsDetailEntity>(info.Id);
            var rev = ValidateGoods(info, dataEntity) ;
            return rev;
        }
        #endregion
        /// <summary>
        /// 验证类目
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateExist(GoodsDetailEntity info)
        {
            var query = new QueryInfo();
            query.Query<GoodsDetailEntity>()
                 .Where(it => it.Goods.Id == info.Goods.Id && it.Product.Id == info.Product.Id);
            var infos = Repository.GetEntities<GoodsDetailEntity>(query);
            var dataEntity= infos == null ? null : infos.FirstOrDefault();
            if (dataEntity != null)
            {
                info.AddError("Exist");
                return false;
            }
            return true;
        }


        /// <summary>
        /// 验证类目
        /// </summary>
        /// <param name="info"></param>
        /// <param name="dataEntity"></param>
        /// <returns></returns>
        protected virtual bool ValidateGoods(GoodsDetailEntity info, GoodsDetailEntity dataEntity)
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
        protected virtual bool ValidateProduct(GoodsDetailEntity info)
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
