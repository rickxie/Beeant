using System.Collections.Generic;
using Beeant.Domain.Entities.Product;

namespace Beeant.Application.Services.Product
{
    public interface IGoodsApplicationService
    {
    
        /// <summary>
        /// 得到产品
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        IList<GoodsEntity> GetGoodsByCache(long[] ids);

    }
}
