using System;
using System.Collections.Generic;
using Beeant.Domain.Entities.Management;
using Beeant.Domain.Services.Utility;

namespace Beeant.Application.Services.Management
{
    public class UserApplicationService : RealizeApplicationService<UserEntity> 
    {
        static protected readonly object Locker=new object();
        /// <summary>
        /// 缓存锁
        /// </summary>

        private static readonly object CacheLocker = new object();
        /// <summary>
        /// 得到缓存值
        /// </summary>

        private static readonly string CacheKey = string.Format("用户{0}", Guid.NewGuid());
        /// <summary>
        /// 
        /// </summary>
        public ICacheRepository CacheRepository { get; set; }
        /// <summary>
        /// 重写Save
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        public override bool Save(IList<UserEntity> infos)
        {
            lock (Locker)
            {
                return base.Save(infos);
            }
        }
 
    }
}
