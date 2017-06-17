using Beeant.Application.Services;
using Beeant.Domain.Entities.Finance;
using Dependent;
using Beeant.Domain.Entities.Account;
using Beeant.Basic.Services.WebForm.Pages;

namespace Beeant.Presentation.Admin.Finance.Finance.Bank
{
    public partial class Add : AddPageBase<BankEntity>
    {

        protected override BankEntity GetEntity()
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