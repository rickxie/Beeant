using Winner.Cache;

namespace Beeant.Distributed.Service.Host.Service
{
    public class CacheService : Winner.Cache.CacheService
    {

        public CacheService()
        {
            Cache = new LocalCache();

        }
    }
}
