using Dependent;
using Beeant.Application.Services;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Cms;
using Beeant.Basic.Services.WebForm.Pages;

namespace Beeant.Presentation.Admin.Cloud.Cms.Cms
{
    public partial class Update : UpdatePageBase<CmsEntity>
    {

        protected override CmsEntity GetEntity()
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