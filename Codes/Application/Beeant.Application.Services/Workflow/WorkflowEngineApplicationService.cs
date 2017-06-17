using System;
using System.Collections.Generic;
using Component.Extension;
using Beeant.Domain.Entities.Workflow;
using Beeant.Domain.Services;
using Beeant.Domain.Services.Utility;
using Beeant.Domain.Services.Workflow;
using Winner.Filter;
using Winner.Persistence;
using System.Linq;
using System.Web;
using Beeant.Application.Services.Sys;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Utility;
using Configuration;
using Winner.Mail;
using Winner.Persistence.Linq;

namespace Beeant.Application.Services.Workflow
{
    public class WorkflowEngineApplicationService : MarshalByRefObject, IWorkflowEngineApplicationService,IEventApplicationService
    {
        /// <summary>
        /// 状态实例
        /// </summary>
        public IWorkflowDomainService WorkflowDomainService { get; set; }
 
        /// <summary>
        /// 消息记录
        /// </summary>
        public IMessageDomainService MessageDomainService { get; set; }
        /// <summary>
        /// 存储实例
        /// </summary>
        public virtual IRepository Repository { get; set; }
        /// <summary>
        /// 消息队列
        /// </summary>
        public virtual IQueueRepository QueueRepository { get; set; }
        /// <summary>
        /// 消息队列
        /// </summary>
        public virtual IMobileRepository MobileRepository { get; set; }
        /// <summary>
        /// 消息队列
        /// </summary>
        public virtual IEmailRepository EmailRepository { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public ICacheRepository CacheRepository { get; set; }
        /// <summary>
        /// 服务领域
        /// </summary>
        public IDictionary<long,IDomainService> DomainServices { get; set; }=new Dictionary<long, IDomainService>();
 
        /// <summary>
        /// 处理工作流
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public virtual bool Handle(WorkflowArgsEntity args)
        {
            var unitofworks = GetHandleUnitofworks(args);
            if (unitofworks == null)
                return false;
            var rev = Winner.Creator.Get<IContext>().Commit(unitofworks);
            if (rev)
            {
                SendMessage(args);
                return true;
            }
            return false;
        }


        /// <summary>
        /// 得到添加Untiofwork
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public virtual IList<IUnitofwork> GetHandleUnitofworks(WorkflowArgsEntity args)
        {
            if (args == null)
                return null;
            args.Engine = GetWorkflowEngineEntity();
            var unitofworks = WorkflowDomainService.Handle(args);
            if (!args.IsValidate)
                return null;
            unitofworks = unitofworks ?? new List<IUnitofwork>();
            var domainService = DomainServices.ContainsKey(args.Flow.Id)? DomainServices[args.Flow.Id]:null;
            if (domainService != null)
            {
                var domainUnitofworks = domainService.Handle(args.Entity);
                if (domainUnitofworks == null)
                {
                    args.Errors = args.Errors ?? new List<ErrorInfo>();
                    args.Errors.AddList(args.Entity.Errors);
                    return null;
                }
                unitofworks.AddList(domainUnitofworks);
            }

            unitofworks.AddList(MessageDomainService.GetUnitofworks(args));
            return unitofworks;
        }
        #region 发送消息

        private const string MessageFlag = "WorkflowMessage{0}";
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="args"></param>
        public virtual void SendMessage(WorkflowArgsEntity args)
        {
            Action<WorkflowArgsEntity> action = SendAsyncMessage;
            action.BeginInvoke(args, null, null);
        }
        /// <summary>
        /// 异步发送消息
        /// </summary>
        /// <param name="args"></param>
        public virtual  void SendAsyncMessage(WorkflowArgsEntity args)
        {

            if (args.Messages == null) return;
            foreach (var message in args.Messages)
            {
                if(message.Account==null)
                    continue;
                SendDefaultMesssage(args, message);
                SendEmailMesssage(args,message);
                SendMobileMesssage(args,message);
            }
        }

        /// <summary>
        /// 发送系统消息
        /// </summary>
        /// <param name="args"></param>
        /// <param name="message"></param>
        protected virtual void SendDefaultMesssage(WorkflowArgsEntity args, MessageEntity message)
        {
            if ((args.Node.MessageType & (int) MessageType.Default) == 0 ||
                string.IsNullOrWhiteSpace(args.Node.DefaultMessage))
                return;
            var url = ConfigurationManager.GetSetting<string>(args.Flow.Url);
            url = string.Format("{0}{1}?taskid={2}", string.IsNullOrWhiteSpace(url) ? args.Flow.Url : url,
                args.Flow.DefaultUrl, message.Task?.Id);
            var detail = args.Node.DefaultMessage.Replace("【Remark】", args.Remark).Replace("【Url】", url);
            var name = string.Format(MessageFlag, message.Account.Id);
            QueueRepository.Open(name, 20);
            QueueRepository.Push(name, detail);
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="args"></param>
        /// <param name="message"></param>
        protected virtual void SendEmailMesssage(WorkflowArgsEntity args, MessageEntity message)
        {
            if ((args.Node.MessageType & (int) MessageType.Email) == 0 ||
                string.IsNullOrWhiteSpace(args.Node.EmailMessage))
                return;
            var account = Repository.Get<AccountEntity>(message.Account.Id);
            if (account == null || string.IsNullOrEmpty(account.Email))
                return;
            var url = ConfigurationManager.GetSetting<string>(args.Flow.Url);
            url = string.Format("{0}{1}", string.IsNullOrWhiteSpace(url) ? args.Flow.Url : url, args.Flow.EmailUrl);
            var detail = args.Node.EmailMessage.Replace("【Remark】", args.Remark).Replace("【Url】", url);
            EmailRepository.Send(new EmailEntity
            {
                IsLog = true,
                Mail = new MailInfo {Subject = message.Title, Body = detail, ToMails = new[] { account.Email}}
            });
        }

        /// <summary>
        /// 发送手机号码
        /// </summary>
        /// <param name="args"></param>
        /// <param name="message"></param>
        protected virtual void SendMobileMesssage(WorkflowArgsEntity args, MessageEntity message)
        {
            if ((args.Node.MessageType & (int) MessageType.Email) == 0 ||
                string.IsNullOrWhiteSpace(args.Node.MobileMessage))
                return;
            var account = Repository.Get<AccountEntity>(message.Account.Id);
            if (account == null || string.IsNullOrEmpty(account.Mobile))
                return;
            var url = ConfigurationManager.GetSetting<string>(args.Flow.Url);
            url = string.Format("{0}{1}", string.IsNullOrWhiteSpace(url) ? args.Flow.Url : url, args.Flow.MobileUrl);
            url = SignUrl(message, url);
            var detail = args.Node.MobileMessage.Replace("【Remark】", args.Remark).Replace("【Url】", url);
            MobileRepository.Send(new MobileEntity {Body = detail, ToMobiles = new[] {account.Mobile}});
        }

        /// <summary>
        /// 签名Url
        /// </summary>
        /// <param name="message"></param>
        /// <param name="url"></param>
        protected virtual string SignUrl(MessageEntity message, string url)
        {
            var timespan = DateTime.Now.Ticks.ToString();
            var mark = Winner.Creator.Get<Winner.Base.ISecurity>().EncryptSign(timespan);
            url = string.Format("{0}?taskId={1}&userid={2}&timespan={3}&mark={4}", url, message.Task?.Id,message.Account?.Id, timespan, mark);
            return url;
        }

        /// <summary>
        /// 得到消息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public virtual MessageEntity GetMessage(long userId)
        {
          return  QueueRepository.Pop<MessageEntity>(string.Format(MessageFlag, userId));
        }
        /// <summary>
        /// 得到消息
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public virtual bool CheckSign(string url)
        {
            var timespan = HttpUtility.ParseQueryString(url).Get("timespan");
            var mark = HttpUtility.ParseQueryString(url).Get("mark");
            if (string.IsNullOrEmpty(mark) || string.IsNullOrEmpty(timespan))
                return false;
            var mk = Winner.Creator.Get<Winner.Base.ISecurity>().EncryptSign(timespan);
            if (mark.ToLower() == mk.ToLower())
                return false;
            return true;
        }
        #endregion

        #region 得到工作流
        /// <summary>
        /// 得到工作流
        /// </summary>
        /// <returns></returns>
        public virtual WorkflowArgsEntity GetWorkflowArgs() 
        {
            var workflowArgs = new WorkflowArgsEntity
            {
                Engine = GetWorkflowEngineEntity()
            };
            return workflowArgs;
        }
        /// <summary>
        /// 缓存锁
        /// </summary>

        private static object CacheLocker = new object();
        /// <summary>
        /// 得到缓存值
        /// </summary>

        private static string CacheKey = "WorkflowEngineArgs";
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual WorkflowEngineEntity GetWorkflowEngineEntity()
        {
            var engine = CacheRepository.Get<WorkflowEngineEntity>(CacheKey);
            if (engine == null)
            {
                lock (CacheLocker)
                {
                    engine = CacheRepository.Get<WorkflowEngineEntity>(CacheKey);
                    if (engine == null)
                    {
                        engine = SetArgsCache();
                    }
                }
            }
            return engine;

        }
        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <returns></returns>
        protected virtual WorkflowEngineEntity SetArgsCache()
        {
            var args = new WorkflowEngineEntity();
            args.GetFlowHandle = GetFlow;
            args.GetLevelsHandle = GetLevels;
            args.GetGroupsHandle = GetGroups;
            args.GetAuditorsHandle = GetAuditors;
            CacheRepository.Set(CacheKey, args, DateTime.MaxValue);
            return args;
        }

        #region 工作组账户
        /// <summary>
        /// 得到缓存值
        /// </summary>

        private static string GroupAccountCacheKey = "WorkflowEngineGroupAccount";
        /// <summary>
        /// 得到缓存值
        /// </summary>
        /// <returns></returns>
        protected virtual string GetGroupAccountCacheKey(long id)
        {
            return string.Format("{0}{1}", GroupAccountCacheKey, id);
        }
      
        /// <summary>
        /// 得到角色
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        protected virtual long[] GetGroups(long accountId)
        {
            var key = GetGroupAccountCacheKey(accountId);
            var value = CacheRepository.Get<long[]>(key);
            if (value == null)
            {
                var query = new QueryInfo();
                query.Query<GroupAccountEntity>().Where(it => it.Account.Id == accountId).Select(it => it.Group.Id);
                var infos = Repository.GetEntities<GroupAccountEntity>(query);
                value = infos == null
                    ? new long[0]
                    : infos.Where(it => it.Group != null).Select(it => it.Group.Id).ToArray();
                CacheRepository.Set(key, value, 3600 * 15);
            }
            return value;
        }
        #endregion

        #region 操作人账户
        /// <summary>
        /// 得到缓存值
        /// </summary>

        private static string AuditorAccountCacheKey = "WorkflowEngineAuditorAccount";
        /// <summary>
        /// 得到缓存值
        /// </summary>
        /// <returns></returns>
        protected virtual string GetAuditorAccountCacheKey(long id)
        {
            return string.Format("{0}{1}", AuditorAccountCacheKey, id);
        }
       
        /// <summary>
        /// 得到角色
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        protected virtual long[] GetAuditors(long accountId)
        {
            var key = GetAuditorAccountCacheKey(accountId);
            var value = CacheRepository.Get<long[]>(key);
            if (value == null)
            {
                var query = new QueryInfo();
                query.Query<AuditorAccountEntity>().Where(it => it.Account.Id == accountId).Select(it => it.Auditor.Id);
                var infos = Repository.GetEntities<AuditorAccountEntity>(query);
                value = infos == null
                    ? new long[0]
                    : infos.Where(it => it.Auditor != null).Select(it => it.Auditor.Id).ToArray();
                CacheRepository.Set(key, value, 3600 * 15);
            }
            return value;
        }
        #endregion

        #region 操作人账户

        /// <summary>
        /// 得到缓存值
        /// </summary>

        private static string FlowCacheKey = "WorkflowEngineFlow";
        /// <summary>
        /// 得到缓存值
        /// </summary>
        /// <returns></returns>
        protected virtual string GetFlowCacheKey(string key)
        {
            return string.Format("{0}{1}", FlowCacheKey, key);
        }
      
        /// <summary>
        /// 得到角色
        /// </summary>
        /// <param name="flowKey"></param>
        /// <returns></returns>
        protected virtual FlowEntity GetFlow(string flowKey)
        {
            var key = GetFlowCacheKey(flowKey);
            var value = CacheRepository.Get<FlowEntity>(key);
            if (value == null)
            {
                var query = new QueryInfo();
                query.Query<FlowEntity>()
                    .Select(
                        it => new object[]
                        {
                        it,
                        it.Nodes.Select(s =>new object[] {s,s.Auditor.AuditorAccounts.Select(n=>n),s.Conditions.Select(n=>n),s.NodeProperties.Select(n=>n)}),
                        });
                value = Repository.GetEntities<FlowEntity>(query)?.FirstOrDefault();
                CacheRepository.Set(key, value, DateTime.MaxValue);
            }
            CreateArgsService(value);
            return value;
        }
        private static object DomainServiceLocker=new object();

        /// <summary>
        /// 生成
        /// </summary>
        /// <param name="flow"></param>
        protected virtual void CreateArgsService(FlowEntity flow)
        {
            lock (DomainServiceLocker)
            {
                if (flow == null)
                    return;
                var domainService = CreateDomainService(flow);
                if (flow.Nodes == null)
                    return;
                foreach (var node in flow.Nodes)
                {
                    SetNodeDelegate(node, domainService);
                }
                if (DomainServices.ContainsKey(flow.Id))
                    DomainServices.Remove(flow.Id);
                DomainServices.Add(flow.Id, domainService);
            }
        
        }

        #endregion

        #region 得到级别

        /// <summary>
        /// 得到缓存值
        /// </summary>

        private static string WorkflowLevelCacheKey = "WorkflowEngineLevel";
        /// <summary>
        /// 得到缓存值
        /// </summary>
        /// <returns></returns>
        protected virtual string GetWorkflowLevelCacheKey(long id)
        {
            return string.Format("{0}{1}", WorkflowLevelCacheKey, id);
        }

        /// <summary>
        /// 填充级别
        /// </summary>
        /// <returns></returns>
        protected virtual IList<LevelEntity> GetLevels()
        {
            var infos = CacheRepository.Get<IList<LevelEntity>>(WorkflowLevelCacheKey);
            if (infos == null)
            {
                var query = new QueryInfo();
                query.Query<LevelEntity>();
                infos = Repository.GetEntities<LevelEntity>(query);
                CacheRepository.Set(WorkflowLevelCacheKey, infos, DateTime.MaxValue);
            }
            return infos;
        }

        #endregion


        /// <summary>
        /// 添加服务
        /// </summary>
        /// <param name="flow"></param>
        protected virtual IDomainService CreateDomainService(FlowEntity flow)
        {
            return Winner.Creator.Get<Winner.Creation.IFactory>().Get<IDomainService>(flow.ClassName);
        }

        /// <summary>
        /// 设置节点委托
        /// </summary>
        /// <param name="node"></param>
        /// <param name="domainService"></param>
        protected virtual void SetNodeDelegate(NodeEntity node,IDomainService domainService)
        {
            try
            {
                if(!string.IsNullOrEmpty(node.ConditionMethod))
                    node.ConditionDelegate = (DelegateNodeMethod)Delegate.CreateDelegate(typeof(DelegateNodeMethod), domainService, node.ConditionMethod);
                if (!string.IsNullOrEmpty(node.BeforeMethod))
                    node.BeforeDelegate = (DelegateNodeMethod)Delegate.CreateDelegate(typeof(DelegateNodeMethod), domainService, node.BeforeMethod);
                if (!string.IsNullOrEmpty(node.AfterMethod))
                    node.AfterDelegate = (DelegateNodeMethod)Delegate.CreateDelegate(typeof(DelegateNodeMethod), domainService, node.AfterMethod);
            }
            catch (Exception ex)
            {
                Winner.Creator.Get<Winner.Log.ILog>().AddException(ex);
            }

        }

        #endregion

        #region 执行事件

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="url"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public virtual bool Execute(string url, string name)
        {
            switch (name)
            {
                case "ClearWorkflowCache":
                    ClearWorkflowCache();
                    break;
                case "ClearWorkflowAccountCache":
                    ClearAccountCache(url);
                    break;
            }
            return true;
        }
        /// <summary>
        /// 清除缓存
        /// </summary>
        protected virtual void ClearWorkflowCache()
        {

            Action updateGroupAccountCache = UpdateGroupAccountCache;
            updateGroupAccountCache.BeginInvoke(null, null);

            CacheRepository.Remove(CacheKey);
            CacheRepository.Remove(WorkflowLevelCacheKey);
        }
        /// <summary>
        /// 更新缓存
        /// </summary>
        protected virtual void UpdateGroupAccountCache()
        {
            for (int i = 0; ; i++)
            {
                var infos = GetGroupAccounts(i);
                if (infos == null || infos.Count == 0)
                    break;
                foreach (var info in infos)
                {
                    if (info.Account == null)
                        continue;
                    UpdateGroupAccountCache(info.Account.Id);
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        protected virtual IList<GroupAccountEntity> GetGroupAccounts(int pageIndex)
        {
            var query=new QueryInfo();
            query.SetPageIndex(pageIndex).SetPageSize(200).Query<GroupAccountEntity>().Select(it => it.Account.Id);
            return Repository.GetEntities<GroupAccountEntity>(query);
        }
        /// <summary>
        /// 更新缓存
        /// </summary>
        protected virtual void UpdateAuditorAccountCache()
        {
            for (int i = 0; ; i++)
            {
                var infos = GetAuditorAccounts(i);
                if (infos == null || infos.Count == 0)
                    break;
                foreach (var info in infos)
                {
                    if (info.Account == null)
                        continue;
                    UpdateAuditorAccountCache(info.Account.Id);
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        protected virtual IList<AuditorAccountEntity> GetAuditorAccounts(int pageIndex)
        {
            var query = new QueryInfo();
            query.SetPageIndex(pageIndex).SetPageSize(200).Query<AuditorAccountEntity>().Select(it => it.Account.Id);
            return Repository.GetEntities<AuditorAccountEntity>(query);
        }
        /// <summary>
        /// 清除缓存
        /// </summary>
        protected virtual void ClearAccountCache(string url)
        {
            var ids = HttpUtility.ParseQueryString(url).Get("accountid");
            if (!string.IsNullOrWhiteSpace(ids))
            {
                var accountIds = ids.Split(',');
                foreach (var accountId in accountIds)
                {
                    var id = accountId.Convert<long>();
                    if (id == 0)
                        continue;
                    UpdateGroupAccountCache(id);
                    UpdateAuditorAccountCache(id);

                }
            }
        }
        /// <summary>
        /// 更新缓存
        /// </summary>
        /// <param name="accountId"></param>
        protected virtual void UpdateGroupAccountCache(long accountId)
        {
            var key = GetGroupAccountCacheKey(accountId);
            CacheRepository.Remove(key);
        }
        /// <summary>
        /// 更新缓存
        /// </summary>
        /// <param name="accountId"></param>
        protected virtual void UpdateAuditorAccountCache(long accountId)
        {
            var key = GetAuditorAccountCacheKey(accountId);
            CacheRepository.Remove(key);
        }
        #endregion
    }
}
