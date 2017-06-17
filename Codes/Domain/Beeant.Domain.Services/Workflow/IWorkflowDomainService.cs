using System.Collections.Generic;
using Beeant.Domain.Entities.Workflow;
using Winner.Persistence;

namespace Beeant.Domain.Services.Workflow
{
    public interface IWorkflowDomainService
    {
        /// <summary>
        /// 处理
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        IList<IUnitofwork> Handle(WorkflowArgsEntity args);
    }
}
