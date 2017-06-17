using Beeant.Domain.Entities.Product;
using Beeant.Domain.Services.Product;
using Winner.Persistence;

namespace Beeant.Application.Services.Product
{
    public class PlatformApplicationService : ApplicationService, IPlatformApplicationService
    {
        /// <summary>
        /// 平台类型
        /// </summary>
        public IPlatformRepository PlatformRepository { get; set; }

        /// <summary>
        /// 同步接口
        /// </summary>
        /// <param name="goodsId"></param>
        /// <returns></returns>
        public virtual PlatformEntity Synchronize(long goodsId)
        {
            var info = PlatformRepository.Synchronize(goodsId);
            if (info != null && info.SaveType!=SaveType.None)
            {
                var unitofworks=DomainService.Handle(info);
                if (unitofworks != null)
                    Commit(unitofworks);
            }
            return info;
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual PlatformEntity Remove(long id)
        {
            var info = PlatformRepository.Remove(id);
            if (info != null && info.SaveType != SaveType.None)
            {
                var unitofworks = DomainService.Handle(info);
                if (unitofworks != null)
                    Commit(unitofworks);
            }
            return info;
        }
    }
}
