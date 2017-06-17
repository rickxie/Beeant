using Dependent;
using Beeant.Application.Services;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Agent;
using Beeant.Domain.Entities.Authority;
using Beeant.Domain.Entities.Finance;
using Beeant.Basic.Services.WebForm.Pages;
using Beeant.Domain.Entities.Management;

namespace Beeant.Presentation.Admin.Cloud.Agent.Agent
{
    public partial class Detail : DetailPageBase<AgentEntity>
    {

        protected override AgentEntity GetEntity()
        {
            var info = base.GetEntity();
            if (info != null && info.Account != null)
            {
                info.Account = Ioc.Resolve<IApplicationService, AccountEntity>().GetEntity<AccountEntity>(info.Account.Id);
            }
            return info;
        }
    }
}