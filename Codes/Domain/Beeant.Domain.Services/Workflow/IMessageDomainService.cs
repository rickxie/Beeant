using System.Collections.Generic;
using Beeant.Domain.Entities.Workflow;
using Winner.Persistence;

namespace Beeant.Domain.Services.Workflow
{
    public interface IMessageDomainService
    {
        #region 接口的实现
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        IList<IUnitofwork> GetUnitofworks(WorkflowArgsEntity args);

        #endregion
    }
}
