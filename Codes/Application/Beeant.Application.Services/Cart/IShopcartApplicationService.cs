using System.Collections.Generic;

namespace Beeant.Application.Services.Cart
{
    public interface IShopcartApplicationService
    {
        /// <summary>
        /// 删除购物车
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="productIds"></param>
        /// <returns></returns>
        bool Remove(long accountId, IList<long> productIds);
    }
}
