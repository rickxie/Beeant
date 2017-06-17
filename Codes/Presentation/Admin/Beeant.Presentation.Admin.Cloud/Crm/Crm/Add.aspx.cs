using Dependent;
using Beeant.Application.Services;
using Beeant.Domain.Entities.Account;
using Beeant.Basic.Services.WebForm.Pages;
using Beeant.Domain.Entities.Crm;

namespace Beeant.Presentation.Admin.Cloud.Crm.Crm
{
    public partial class Add : AddPageBase<CrmEntity>
    {
   
        protected override CrmEntity GetEntity()
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