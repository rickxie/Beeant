using System.Collections.Generic;
using Beeant.Domain.Entities.Workflow;
using Winner.Persistence;

namespace Beeant.Application.Services.Workflow
{
    public interface IWorkflowEngineApplicationService
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        bool Handle(WorkflowArgsEntity args);
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        IList<IUnitofwork> GetHandleUnitofworks(WorkflowArgsEntity args);
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="args"></param>
        void SendMessage(WorkflowArgsEntity args);
        /// <summary>
        /// 得到消息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        MessageEntity GetMessage(long userId);
        /// <summary>
        /// 得到消息
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        bool CheckSign(string url);
        /// <summary>
        /// 得到工作流
        /// </summary>
        /// <returns></returns>
        WorkflowArgsEntity GetWorkflowArgs();
    }
}
