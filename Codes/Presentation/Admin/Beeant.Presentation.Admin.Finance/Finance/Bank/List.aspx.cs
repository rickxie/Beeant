using System.Collections.Generic;
using System.Linq;
using Dependent;
using Beeant.Application.Services;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Finance;
using Beeant.Basic.Services.WebForm.Pages;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Presentation.Admin.Finance.Finance.Bank
{
    public partial class List : ListPageBase<BankEntity>
    {

        protected override IList<BankEntity> GetEntities()
        {
            var infos = base.GetEntities();
            if (infos != null)
            {
                SetAccounts(infos);
            }
            return infos;
        }

        protected virtual void SetAccounts(IList<BankEntity> infos)
        {
            var accountIds =
                   infos.Where(it => it.Account != null && it.Account.Id!=0)
                        .Select(it => it.Account.Id)
                        .ToArray();
            if (accountIds.Length > 0)
            {
                var query = new QueryInfo();
                query.Query<AccountEntity>().Where(it => accountIds.Contains(it.Id));
                var accounts = Ioc.Resolve<IApplicationService, AccountEntity>().GetEntities<AccountEntity>(query);
                if (accounts != null)
                {
                    foreach (var info in infos)
                    {
                        if (info.Account == null) continue;
                        info.Account = accounts.FirstOrDefault(it => it.Id == info.Account.Id);
                    }
                }
            }
        }
    }
}