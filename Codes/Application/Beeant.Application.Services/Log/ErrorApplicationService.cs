using System;
using System.Collections.Generic;
using Beeant.Domain.Entities.Log;
using Beeant.Domain.Services.Utility;

namespace Beeant.Application.Services.Log
{
    public class ErrorApplicationService : RealizeApplicationService<ErrorEntity>
    {
      
      public IIpRepository IpRepository { get; set; }

        public override bool Save(IList<ErrorEntity> infos)
        {
            Action<IList<ErrorEntity>> func = AnscSave;
            func.BeginInvoke(infos, null, null);
            return true;
        }
        /// <summary>
        /// 异步存储登录
        /// </summary>
        /// <param name="infos"></param>
        protected virtual void AnscSave(IList<ErrorEntity> infos)
        {
            base.Save(infos);
        }
    }
}
