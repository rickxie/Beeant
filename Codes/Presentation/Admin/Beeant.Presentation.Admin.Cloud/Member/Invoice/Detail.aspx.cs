using System.Collections.Generic;
using System.Linq;
using Dependent;
using Beeant.Application.Services;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Agent;
using Beeant.Domain.Entities.Finance;
using Beeant.Domain.Entities.Member;
using Beeant.Basic.Services.WebForm.Pages;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Presentation.Admin.Cloud.Member.Invoice
{
    public partial class Detail : DetailPageBase<InvoiceEntity>
    {
        protected override InvoiceEntity GetEntity()
        {
            var info = base.GetEntity();
            if (info != null && info.Account != null)
            {
                info.Account = Ioc.Resolve<IApplicationService, AccountEntity>().GetEntity<AccountEntity>(info.Account.Id);
                if (info.Account != null)
                {
                    var query = new QueryInfo();
                    query.Query<AgentEntity>().Where(it => it.Account.Id == info.Account.Id);
                    var agents = Ioc.Resolve<IApplicationService, AgentEntity>().GetEntities<AgentEntity>(query);
                    if (agents == null || agents.Count == 0)
                        info.Name = info.Account.RealName ?? "";
                    else
                        info.Name = agents.FirstOrDefault().Name ?? (info.Account.RealName ?? "");
                }
            }
            return info;
        }
    }
}