using Beeant.Application.Services;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Log;
using Dependent;

namespace Beeant.Presentation.Admin.Log.Log.Login
{
    public partial class Detail : Basic.Services.WebForm.Pages.DetailPageBase<LoginEntity>
    {
        protected override LoginEntity GetEntity()
        {
            var info = base.GetEntity();
            if (info != null && info.Account != null)
            {
                info.Account = Ioc.Resolve<IApplicationService>().GetEntity<AccountEntity>(info.Account.Id);
            }
            return info;
        }
    }
}