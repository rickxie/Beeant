using System.Collections.Generic;
using System.Text;
using Beeant.Domain.Entities.Workflow;
using System.Linq;
using Beeant.Domain.Entities;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Domain.Services.Workflow
{
    public class WorkflowDomainService : IWorkflowDomainService
    {
        #region 属性
        /// <summary>
        /// 存储实例
        /// </summary>
        public virtual IRepository Repository{ get; set; }
        #endregion

        #region 接口的实现

        /// <summary>
        /// 验证修改
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public virtual IList<IUnitofwork> Handle(WorkflowArgsEntity args)
        {
            if (args == null)
                return null;
            if (args.AccountId == 0 || args.TaskId == 0 || args.LevelId == 0)
            {
                args.AddError("ParamterError");
                return null;
            }
            FillArgs(args);
            if (!Validate(args))
                return null;
            args.Entity = args.Entity.Id == 0 ? args.Entity : GetEntityByConditions(args);
            var nodeIds = args.Flow.Nodes == null
                ? null
                : args.Flow.Nodes.Where(it => it.Name == args.Node.Name).Select(it => it.Id).ToArray();
            if (nodeIds == null || nodeIds.Length == 0)
                return null;
            var currentTasks = GetCurrentTasks(args.Entity.Id, nodeIds);
            args.CreateTasks(currentTasks);
            if (args.NextTasks == null)
                return null;
            SetEntityStatus(args);
            return Repository.Save(args.NextTasks);
        }

        /// <summary>
        /// 填充实体
        /// </summary>
        protected virtual void FillArgs(WorkflowArgsEntity args)
        {
            if (args.TaskId > 0)
            {
                args.Task = Repository.Get<TaskEntity>(args.TaskId);
                return;
            }
            var auditors= args.Engine.GetAuditorsHandle(args.AccountId);
            if (auditors != null )
            {
                
                var node = args.Flow.Nodes.OrderByDescending(it => it.Sequence)
                    .FirstOrDefault(it => it.Auditor != null && auditors.Contains(it.Auditor.Id));
                if (node != null)
                {
                    var name = node.NodeType == NodeType.Any || node.NodeType == NodeType.Any
                        ? node.PassName
                        : node.Name;
                    args.Node = args.Flow.Nodes.FirstOrDefault(it => it.Name == name);
                }
            }
            else
            {
                args.Node = args.Flow.Nodes.FirstOrDefault(it => it.NodeType == NodeType.Start);
            }
        }

        /// <summary>
        /// 验证
        /// </summary>
        /// <returns></returns>
        protected virtual bool Validate(WorkflowArgsEntity args)
        {
            if (args.Task == null)
            {
                args.AddError("TaskNoExist");
                return false;
            }
            if (args.Flow == null)
            {
                args.AddError("FlowNoExist");
                return false;
            }
            if (args.Node == null)
            {
                args.AddError("NodeNoExist");
                return false;
            }
            if (args.Task.Status != TaskStatusType.Waiting)
            {
                args.AddError("TaskHandled");
                return false;
            }
            if (args.Task.Account==null || args.Task.Account.Id != args.AccountId)
            {
                args.AddError("NoAuthority");
                return false;
            }
            return ValidateCreateTask(args);
        }
        /// <summary>
        /// 得到流程
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        protected virtual bool ValidateCreateTask(WorkflowArgsEntity args)
        {
            if (args.Task != null)
                return true;
            if (args.Task == null && args.Entity.SaveType == SaveType.Add)
                return true;
            var query=new QueryInfo {IsReturnCount = false};
            query.SetPageSize(1)
                .Query<TaskEntity>()
                .Where(it => it.DataId == args.Entity.Id && it.Flow.Id == args.Flow.Id)
                .Select(it => it.Id);
            var infos = Repository.GetEntities<TaskEntity>(query);
            if (infos != null && infos.Count > 0)
            {
                args.AddError("TaskAlreadCreated");
                return false;
            }
            return true;
        }
      

        /// <summary>
        /// 设置业务状态
        /// </summary>
        protected virtual void SetEntityStatus(WorkflowArgsEntity args)
        {
            if(args.NextNodes == null || args.NextNodes.Count==0)
                return;
            foreach (var nextNode in args.NextNodes)
            {
                if(string.IsNullOrEmpty(nextNode.StatusName) || string.IsNullOrWhiteSpace(nextNode.StatusValue))
                    continue;
                Winner.Creator.Get<Winner.Base.IProperty>()
                    .SetValue(args.Entity, nextNode.StatusName, nextNode.StatusValue);
                if (args.Entity.SaveType == SaveType.None)
                {
                    args.Entity.SaveType=SaveType.Modify;
                    args.Entity.SetProperty(nextNode.StatusName);
                }
                else if (args.Entity.SaveType == SaveType.Modify && args.Entity.Properties != null)
                {
                    args.Entity.SetProperty(nextNode.StatusName);
                }
            }
        }
        #endregion


        #region 查询
        /// <summary>
        /// 得到当前任务
        /// </summary>
        /// <returns></returns>
        public virtual IList<TaskEntity> GetCurrentTasks(long dataId,long[] nodeIds)
        {
            var query = new QueryInfo();
            query.Query<TaskEntity>()
                .Where(it => it.DataId== dataId && nodeIds.Contains(it.Node.Id));
            var infos = Repository.GetEntities<TaskEntity>(query);
            return infos;
        } 
        /// <summary>
        /// 得到对象
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        protected virtual BaseEntity GetEntityByConditions(WorkflowArgsEntity args)
        {
            var query = new QueryInfo();
            var select = new StringBuilder();
            select.Append("*");
            if (args.Node.Conditions != null)
            {
                foreach (var condition in args.Node.Conditions.Where(condition => condition.SelectExpArray != null))
                {
                    @select.Append(",");
                    @select.Append(condition.SelectExp);
                }
            }
            var entityName = string.Format("{0},{1}", args.Entity.GetType().FullName,
                  args.Entity.GetType().Module.ToString().Replace(".dll", ""));
            query.Select(select.ToString()).Where("Id==@Id").From(entityName)
                .SetParameter("Id", args.Entity.Id);
            return Repository.GetEntities<BaseEntity>(query).FirstOrDefault();
        }
        #endregion

 
    }
}
