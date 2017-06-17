using System.Collections.Generic;
using Beeant.Domain.Entities.Product;
using Winner.Filter;

namespace Beeant.Application.Services.Product
{
    public interface IPlatformApplicationService
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="goodsId"></param>
        /// <returns></returns>
        PlatformEntity Synchronize(long goodsId);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        PlatformEntity Remove(long id);
    }
}
