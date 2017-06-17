using System;
using System.Collections.Generic;
using Beeant.Domain.Entities.Log;

namespace Beeant.Application.Services.Log
{
    public class ApiTraceApplicationService : RealizeApplicationService<ApiTraceEntity>
    {

        public override bool Save(IList<ApiTraceEntity> infos)
        {
            Action<IList<ApiTraceEntity>> func = AnscSave;
            func.BeginInvoke(infos, null, null);
            return true;
        }
        /// <summary>
        /// 异步存储登录
        /// </summary>
        /// <param name="infos"></param>
        protected virtual void AnscSave(IList<ApiTraceEntity> infos)
        {
           
            base.Save(infos);
        }
    }
}
