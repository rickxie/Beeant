using Beeant.Application.Services;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Log;
using Dependent;

namespace Beeant.Presentation.Admin.Log.Log.Error
{
    public partial class Detail : Basic.Services.WebForm.Pages.DetailPageBase<ErrorEntity>
    {
        protected override ErrorEntity GetEntity()
        {
            var info= base.GetEntity();
            if (info != null && info.Account != null)
            {
                info.Account = Ioc.Resolve<IApplicationService>().GetEntity<AccountEntity>(info.Account.Id);
            }
            return info;
        }
    }
}