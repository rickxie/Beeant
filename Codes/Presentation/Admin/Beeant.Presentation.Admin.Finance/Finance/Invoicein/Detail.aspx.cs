using Dependent;
using Beeant.Application.Services;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Finance;
using Beeant.Basic.Services.WebForm.Pages;

namespace Beeant.Presentation.Admin.Finance.Finance.Invoicein
{
    public partial class Detail : WorkflowPageBase<InvoiceinEntity>
    {
         
        protected override InvoiceinEntity GetEntity()
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