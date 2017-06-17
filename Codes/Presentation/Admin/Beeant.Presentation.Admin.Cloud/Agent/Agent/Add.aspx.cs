using System.Collections.Generic;
using Dependent;
using Beeant.Application.Services;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Agent;
using Beeant.Basic.Services.WebForm.Pages;
using Winner.Persistence;

namespace Beeant.Presentation.Admin.Cloud.Agent.Agent
{
    public partial class Add : AddPageBase<AgentEntity>
    {
        public override bool IsUpdatePanel
        {
            get { return false; }
            set
            {
                base.IsUpdatePanel = value;
            }
        }
        protected override AgentEntity GetEntity()
        {
            var info = base.GetEntity();
            if (info != null && info.Account!=null)
            {
                info.Account =Ioc.Resolve<IApplicationService, AccountEntity>().GetEntity<AccountEntity>(info.Account.Id);
            }
            return info;
        }
        /// <summary>
        /// 填充
        /// </summary>
        /// <returns></returns>
        protected override AgentEntity FillEntity()
        {
            var info= base.FillEntity();
            return info;
        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="info"></param>
        protected override void Save(AgentEntity info)
        {
            if (info == null) return;
            var rev = Ioc.Resolve<IApplicationService, AgentEntity>().Save(info);
            if (rev)
            {
                var accountNumber = new AccountNumberEntity
                {
                    Account = info.Account,
                    Tag = "AgentId",
                    NumberEntity = info,
                    Name = "代理编号",
                    SaveType = SaveType.Add
                };
                Ioc.Resolve<IApplicationService, AccountNumberEntity>().Save(accountNumber);
            }
            SetResult(rev, info.Errors);
        }
    }
}