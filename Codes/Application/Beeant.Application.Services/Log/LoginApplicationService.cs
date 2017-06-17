using System;
using System.Collections.Generic;
using Beeant.Domain.Entities.Log;
using Beeant.Domain.Services.Utility;

namespace Beeant.Application.Services.Log
{
    public class LoginApplicationService : RealizeApplicationService<LoginEntity>
    {
      
      public IIpRepository IpRepository { get; set; }

        public override bool Save(IList<LoginEntity> infos)
        {
            Action<IList<LoginEntity>> func = AnscSave;
            func.BeginInvoke(infos, null, null);
            return true;
        }
        /// <summary>
        /// 异步存储登录
        /// </summary>
        /// <param name="infos"></param>
        protected virtual void AnscSave(IList<LoginEntity> infos)
        {
            foreach (var info in infos)
            {
                if (string.IsNullOrEmpty(info.City))
                {
                    var ipEntity = IpRepository.Get(info.Ip);
                    if (ipEntity != null)
                    {
                        info.City = ipEntity.City;
                    }
                }
            }
            base.Save(infos);
        }
    }
}
