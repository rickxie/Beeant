using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using Beeant.Domain.Entities.Account;
using Winner;
using Winner.Filter;
using Winner.Persistence;

namespace Beeant.Domain.Entities.Workflow
{

    [Serializable]
    public class WorkflowArgsEntity
    {
        /// <summary>
        /// 流程名称
        /// </summary>
        public string FlowKey { get; set; }
        /// <summary>
        /// 是否通过
        /// </summary>
        public bool IsPass { get; set; }
        /// <summary>
        /// 业务实体
        /// </summary>
        public BaseEntity Entity { get; set; }
        /// <summary>
        /// 级别
        /// </summary>
        public long LevelId { get; set; }
        /// <summary>
        /// 操作任务
        /// </summary>
        public long TaskId { get; set; }
        /// <summary>
        /// 操作人 
        /// </summary>
        public long AccountId { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 错误信息
        /// </summary>
        public IList<ErrorInfo> Errors { get; set; }
        /// <summary>
        /// 引擎
        /// </summary>
        public WorkflowEngineEntity Engine { get; set; }
        /// <summary>
        /// 当前执行的任务
        /// </summary>
        public TaskEntity Task { get; set; }
       
        private FlowEntity _flow;
        /// <summary>
        /// 当前工作流
        /// </summary>
        public FlowEntity Flow
        {
            get
            {
                if (_flow != null)
                    return _flow;
                if (Engine.GetFlowHandle == null) return null;
                _flow = Engine.GetFlowHandle(FlowKey);
                return _flow;
            }
        }
        private NodeEntity _node;
        /// <summary>
        /// 当前节点
        /// </summary>
        public NodeEntity Node
        {
            get
            {
                if (_node != null)
                    return _node;
                if (Flow == null || Flow.Nodes == null || Task == null || Task.Node == null)
                    return null;
                _node = Flow.Nodes.FirstOrDefault(it => it.Id == Task.Node.Id);
                return _node;
            }
            set
            {
                _node = value;
            }
        }
        private LevelEntity _level;
        /// <summary>
        /// 当前节点
        /// </summary>
        public LevelEntity Level
        {
            get
            {
                if (_level != null)
                    return _level;
                _level = Engine.GetLevels()?.FirstOrDefault(it => it.Id == LevelId);
                return _level;
            }
        }
        /// <summary>
        /// 生产的任务
        /// </summary>
        public IList<TaskEntity> NextTasks { get; set; }
        /// <summary>
        /// 生产的节点
        /// </summary>
        public IList<NodeEntity> NextNodes { get; set; }
        /// <summary>
        /// 消息
        /// </summary>
        public IList<MessageEntity> Messages { get; set; }
    
        /// <summary>
        /// 添加错误信息
        /// </summary>
        public virtual void AddError(ErrorInfo error)
        {
            Errors = Errors ?? new List<ErrorInfo>();
            Errors.Add(error);
        }

        /// <summary>
        /// 添加错误信息
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="paramters"></param>
        public virtual void AddError(string propertyName, params object[] paramters)
        {
            Errors = Errors ?? new List<ErrorInfo>();
            var error = Creator.Get<IValidation>().GetErrorInfo(GetType().FullName, propertyName);
            error.Message = string.Format(error.Message, paramters);
            Errors.Add(error);
        }

        /// <summary>
        /// 添加错误信息
        /// </summary>
        /// <param name="name"></param>
        /// <param name="propertyName"></param>
        /// <param name="paramters"></param>
        public virtual void AddErrorByName(string name, string propertyName, params object[] paramters)
        {
            Errors = Errors ?? new List<ErrorInfo>();
            var error = Creator.Get<IValidation>().GetErrorInfo(name, propertyName);
            error.Message = string.Format(error.Message, paramters);
            Errors.Add(error);
        }

        /// <summary>
        /// 验证是否通过
        /// </summary>
        public virtual bool IsValidate
        {
            get { return Errors == null || Errors.Count == 0; }
        }
   

        #region 创建流程
        /// <summary>
        /// 生成流程
        /// </summary>
        public virtual void CreateTasks(IList<TaskEntity> currentNodeTasks)
        {
            SetTask();
            if (Flow == null || Flow.Nodes == null || Task == null || Task.Node == null)
                return;
            var node = Flow.Nodes.FirstOrDefault(it => it.Id == Task.Node.Id);
            if (node == null)
                return;
           
            NextNodes = GetNextNodes(node);
            if (NextNodes == null || NextNodes.Count == 0)
                return;
            if (!CheckCreateTask(node, currentNodeTasks))
                return;
            NextTasks = GetTasks(NextNodes);
        }
        /// <summary>
        /// 设置当前任务
        /// </summary>
        protected virtual void SetTask()
        {
            if ( Node==null)
                return;
            if (Task == null)
            {
                Task = new TaskEntity
                {
                    Data = Entity,
                    Flow = Flow,
                    Level = Level,
                    Node = Node,
                    OverTime = DateTime.Now,
                    HandleTime = DateTime.Now,
                    Account = new AccountEntity { Id = AccountId },
                    Remark = "",
                    Status = TaskStatusType.Created,
                    SaveType = SaveType.Add
                };
                Task.FillDataEntity(Node.NodeProperties);
            }
            else
            {
                Task.HandleTime = DateTime.Now;
                Task.Remark = Remark;
                Task.SaveType = SaveType.Modify;
                Task.SetProperty(it => it.Remark).SetProperty(it => it.HandleTime);
                Task.FillDataEntity(Node.NodeProperties);
            }

        }

        /// <summary>
        /// 生产任务
        /// </summary>
        /// <param name="nodes"></param>
        /// <returns></returns>
        protected virtual IList<TaskEntity> GetTasks(IList<NodeEntity> nodes)
        {
            var tasks = new List<TaskEntity>();
            foreach (var node in nodes)
            {

                var accountId = GetTaskAccountId(node);
                if (accountId == 0 || accountId == AccountId)
                   continue;
                var task = new TaskEntity
                {
                    Flow = new FlowEntity { Id = Flow.Id },
                    Data = Entity,
                    Node = node,
                    Level = new LevelEntity { Id = LevelId },
                    OverTime = DateTime.Now.AddMinutes(node.Timeout),
                    Account = new AccountEntity { Id = accountId },
                    SaveType = SaveType.Add
                };
                tasks.Add(task);
            }
            return tasks;
        }

        /// <summary>
        /// 得到任务用户编号
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        protected virtual long GetTaskAccountId(NodeEntity node)
        {
            if (node.Auditor == null || node.Auditor.AuditorAccounts == null || node.Auditor.AuditorAccounts.Count == 0)
                return 0;
            var userIds = GetTaskAccountIds(node);
            if (userIds == null)
                return 0;
            if (node.AssignType == NodeAssignType.Average)
            {
                int index = (int)(Entity.UpdateTime.Ticks % node.Auditor.AuditorAccounts.Count);
                return node.Auditor.AuditorAccounts[index].Account == null ? 0 : node.Auditor.AuditorAccounts[index].Account.Id;
            }
            if (node.AssignType == NodeAssignType.Random)
            {
                Random rd = new Random(Guid.NewGuid().GetHashCode());
                var index = rd.Next(0, node.Auditor.AuditorAccounts.Count - 1);
                return node.Auditor.AuditorAccounts[index].Account == null ? 0 : node.Auditor.AuditorAccounts[index].Account.Id;
            }
            return 0;
        }

        /// <summary>
        /// 得到用户编号
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        protected virtual IList<long> GetTaskAccountIds(NodeEntity node)
        {
            var groupIds = Engine.GetGroups(AccountId)?.ToDictionary(it=>it);
            var accounts = new Dictionary<long, long>();
            foreach (var id in node.Auditor.AuditorAccounts.Where(it => it.Account != null && it.Account.IsUsed).Select(it => it.Account.Id))
            {
                if(accounts.ContainsKey(id))
                    continue;
                if (node.IsGroup)
                {
                    var auditorGroupIds = Engine.GetGroups(id);
                    if(groupIds==null || auditorGroupIds == null || auditorGroupIds.Count(it=> groupIds.ContainsKey(it))==0)
                        continue;
                }
                accounts.Add(id, id);
            }
            return accounts.Values.ToList();
        }
        /// <summary>
        /// 确定是否生成
        /// </summary>
        /// <returns></returns>
        protected virtual bool CheckCreateTask(NodeEntity node, IList<TaskEntity> tasks)
        {
            if (node == null || tasks == null)
                return false;
            var nextName = IsPass ? node.PassName:  node.RejectName ;
            var nextNode = Flow.Nodes.FirstOrDefault(it => it.Name == nextName);
            if (nextNode == null || nextNode.NodeType == NodeType.End)
                return false;
            switch (node.NodeType)
            {
                case NodeType.Single:
                    {
                        return true;
                    }
                case NodeType.Any:
                    {
                        if (IsPass)
                        {
                            return tasks.Count(it => it.Status == TaskStatusType.Passed) == 0;
                        }
                        return tasks.All(it => it.Status == TaskStatusType.Rejected && it.Id != Task.Id);
                    }
                case NodeType.All:
                    {
                        if (IsPass)
                        {
                            return tasks.All(it => it.Status == TaskStatusType.Passed && it.Id == Task.Id);
                        }
                        return tasks.Count(it => it.Status == TaskStatusType.Rejected) == 0;
                    }
            }
            return false;
        }


        /// <summary>
        /// 得到下一个节点
        /// </summary>
        /// <returns></returns>
        protected virtual IList<NodeEntity> GetNextNodes(NodeEntity currentNode)
        {
            if (currentNode == null)
                return null;
            var nodes = new List<NodeEntity>();
            var name = IsPass ? currentNode.PassName : currentNode.RejectName;
            foreach (var node in Flow.Nodes.Where(it => it.Name == name))
            {
                if (CheckNodeCondition(node))
                {
                    switch (node.ConditionType)
                    {
                        case ConditionType.Create:
                            nodes.Add(node);
                            break;
                        case ConditionType.UnCreate:
                            break;
                        case ConditionType.Skip:
                            return GetNextNodes(Flow.Nodes.FirstOrDefault(it => it.PassName == currentNode.PassName));
                    }

                }
            }
            return nodes;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        protected virtual bool CheckNodeCondition(NodeEntity node)
        {
            if (node == null || node.Conditions == null || node.Conditions.Count == 0)
                return true;
            if (node.ConditionDelegate != null)
                return node.ConditionDelegate(this);
            var infos = new List<object> { Entity };
            foreach (var condition in node.Conditions)
            {
                if (string.IsNullOrEmpty(condition.InspectExp))
                    continue;
                if (!infos.AsQueryable().Where(condition.InspectExp, condition.ArgumentArray).Any())
                {
                    return false;
                }
            }
            return true;
        }

        #endregion


    }
    [Serializable]
    public class WorkflowEngineEntity
    {

     
        /// <summary>
        /// 得到组委托
        /// </summary>
        public Func<string, FlowEntity> GetFlowHandle { get; set; }
        /// <summary>
        /// 级别
        /// </summary>
        public Func<IList<LevelEntity>> GetLevelsHandle{ get; set; } 

        /// <summary>
        /// 得到组委托
        /// </summary>
        public Func<long,long[]> GetGroupsHandle { get; set; }
        /// <summary>
        /// 得到组委托
        /// </summary>
        public Func<long, long[]> GetAuditorsHandle { get; set; }


        /// <summary>
        /// 得到用户编号
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public virtual long[] GetGroups(long accountId)
        {
            if (GetGroupsHandle == null)
                return null;
            return GetGroupsHandle(accountId);
        }
        /// <summary>
        /// 得到用户编号
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public virtual long[] GetAuditors(long accountId)
        {
            if (GetAuditorsHandle == null)
                return null;
            return GetAuditorsHandle(accountId);
        }
        /// <summary>
        /// 得到用户编号
        /// </summary>
        /// <returns></returns>
        public virtual IList<LevelEntity> GetLevels()
        {
            if (GetLevelsHandle == null)
                return null;
            return GetLevelsHandle();
        }
    }
}
