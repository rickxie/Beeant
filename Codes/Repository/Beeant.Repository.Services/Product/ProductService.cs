using Component.Extension;
using Beeant.Domain.Entities.Product;
using Beeant.Domain.Services.Product;
using Winner.Persistence;

namespace Beeant.Repository.Services.Product
{
    public class ProductService : Repository, IProductContract
    {
        public virtual string Save(string info)
        {
            var product = info.DeserializeJson<ProductEntity>();
            var dataInfo = product.SaveType == SaveType.Add ? null : Get<ProductEntity>(product.Id);
            if (ValidateCount(product, dataInfo))
            {
                var unitofworks = base.Save(product);
                Winner.Creator.Get<IContext>().Commit(unitofworks);
            }
            return product.SerializeJson();
        }
        /// <summary>
        /// 验证手机号码
        /// </summary>
        /// <param name="info"></param>
        /// <param name="dataEntity"></param>
        /// <returns></returns>
        protected virtual bool ValidateCount(ProductEntity info, ProductEntity dataEntity)
        {
            if (!info.HasSaveProperty(it => it.ChangeCount)) return true;
            if (info.ChangeCount+dataEntity.Count<0)
            {
                info.AddError("Count");
                return false;
            }
            return true;
        }

    }
}
