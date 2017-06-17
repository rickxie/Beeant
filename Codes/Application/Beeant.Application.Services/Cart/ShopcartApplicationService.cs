using System.Collections.Generic;
using Beeant.Domain.Entities.Cart;
using Winner.Persistence;

namespace Beeant.Application.Services.Cart
{
    public class ShopcartApplicationService : RealizeApplicationService<ShopcartEntity>, IShopcartApplicationService
    {
        /// <summary>
        /// 删除购物车
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="productIds"></param>
        /// <returns></returns>
        public bool Remove(long accountId,IList<long> productIds)
        {
            var info = new ShopcartEntity {SaveType = SaveType.Remove};
            if (productIds != null && productIds.Count > 0 )
            {
                info.Where(
                    it =>
                    it.Account.Id == accountId &&
                    (productIds.Contains(it.Product.Id)));
            }
            else if (productIds != null && productIds.Count > 0 )
            {
                info.Where(
                    it =>it.Account.Id == accountId &&productIds.Contains(it.Product.Id));
            }
            else
            {
                return false;
            }
            var unitofworks = Repository.Save(info);
            return Commit(unitofworks);
        }
    }
}
