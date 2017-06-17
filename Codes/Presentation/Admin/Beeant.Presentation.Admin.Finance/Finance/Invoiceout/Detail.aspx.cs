using Dependent;
using Beeant.Application.Services;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Finance;
using Beeant.Basic.Services.WebForm.Pages;

namespace Beeant.Presentation.Admin.Finance.Finance.Invoiceout
{
    public partial class Detail : WorkflowPageBase<InvoiceoutEntity>
    {
        
        protected override InvoiceoutEntity GetEntity()
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