using System.Collections.Generic;
using Beeant.Domain.Entities.Product;
using Winner.Filter;
using Winner.Persistence;

namespace Beeant.Domain.Services.Product
{
    public interface IGoodsPropertyDomainService
    {
        /// <summary>
        /// 验证发布
        /// </summary>
        /// <param name="goodId"></param>
        /// <param name="saveType"></param>
        /// <param name="goodsProperties"></param>
        /// <returns></returns>
        IList<ErrorInfo> ValidateRuleType(long goodId,SaveType saveType, IList<GoodsPropertyEntity> goodsProperties);

    }
}
