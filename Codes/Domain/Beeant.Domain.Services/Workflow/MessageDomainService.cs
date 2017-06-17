using System.Collections.Generic;
using Beeant.Domain.Entities.Workflow;
using System.Linq;
using Beeant.Domain.Entities.Account;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Domain.Services.Workflow
{
    public class MessageDomainService:RealizeDomainService<MessageEntity>,IMessageDomainService
    {


        #region 接口的实现

        /// <summary>
        /// 验证修改
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public virtual IList<IUnitofwork> GetUnitofworks(WorkflowArgsEntity args)
        {
            if (args == null || args.Entity == null 
                || args.Flow==null || args.Task == null || args.Node==null) 
                return null;
            var infos = GetMessageEntities(args);
            args.Messages = infos;
            if (infos != null && infos.Count > 0)
            {
                return Handle(infos);
            }
            return null;
        }
        #endregion

        /// <summary>
        /// 得到消息
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        protected virtual IList<MessageEntity> GetMessageEntities(WorkflowArgsEntity args)
        {
            if (string.IsNullOrWhiteSpace(args.Node.PassName))
            {
                var user = GetSumbitAccount(args);
                var mess = GetMessageEntity(args, user,0);
                if (mess == null)
                    return null;
                return new List<MessageEntity> {mess};
            }
            if (args.NextTasks == null)
                return null;
            var infos = new List<MessageEntity>();
            foreach (var nextTask in args.NextTasks)
            {
                infos.Add(GetMessageEntity(args, nextTask.Account, nextTask.Id));
            }
            return infos;
        }

        /// <summary>
        /// 得到提交人
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        protected virtual AccountEntity GetSumbitAccount(WorkflowArgsEntity args)
        {
            var query=new QueryInfo {IsReturnCount=false};
            query.SetPageSize(1)
                .Query<TaskEntity>()
                .Where(it => it.DataId == args.Task.DataId && it.Flow.Id == args.Flow.Id && it.Status==TaskStatusType.Created)
                .Select(it=>it.Account.Id);
            var entities = Repository.GetEntities<TaskEntity>(query);
            return entities?.FirstOrDefault()?.Account;
        }

        /// <summary>
        /// 得到消息
        /// </summary>
        /// <param name="args"></param>
        /// <param name="account"></param>
        /// <param name="taskId"></param>
        /// <returns></returns>
        protected virtual MessageEntity GetMessageEntity(WorkflowArgsEntity args, AccountEntity account,long taskId)
        {
            if (account == null)
                return null;
            var message = new MessageEntity
            {
                SaveType = SaveType.Add,
                Flow = args.Flow,
                Title = args.Node.MessageTitle,
                Data = args.Entity,
                IsRead = false,
                Level = args.Level,
                Task=new TaskEntity { Id=taskId},
                Account = account
            };
            return message;
        }
       

    }
}
