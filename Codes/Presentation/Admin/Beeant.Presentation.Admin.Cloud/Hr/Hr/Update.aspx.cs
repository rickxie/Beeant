using Dependent;
using Beeant.Application.Services;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Hr;
using Beeant.Basic.Services.WebForm.Pages;

namespace Beeant.Presentation.Admin.Cloud.Hr.Hr
{
    public partial class Update : UpdatePageBase<HrEntity>
    {

        protected override HrEntity GetEntity()
        {
            var info = base.GetEntity();
            if (info != null && info.Account != null)
            {
                info.Account =
                    Ioc.Resolve<IApplicationService, AccountEntity>().GetEntity<AccountEntity>(info.Account.Id);
            }
            return info;
        }
    }
}