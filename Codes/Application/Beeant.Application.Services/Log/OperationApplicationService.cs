using System;
using System.Collections.Generic;
using Beeant.Domain.Entities.Log;

namespace Beeant.Application.Services.Log
{
    public class OperationApplicationService : RealizeApplicationService<OperationEntity>
    {

        public override bool Save(IList<OperationEntity> infos)
        {
            Action<IList<OperationEntity>> func = AnscSave;
            func.BeginInvoke(infos, null, null);
            return true;
        }
        /// <summary>
        /// 异步存储登录
        /// </summary>
        /// <param name="infos"></param>
        protected virtual void AnscSave(IList<OperationEntity> infos)
        {
           
            base.Save(infos);
        }
    }
}
