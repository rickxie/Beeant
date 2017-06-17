using Dependent;
using Beeant.Application.Services;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Authority;
using Beeant.Domain.Entities.Finance;
using Beeant.Domain.Entities.Supplier;
using Beeant.Basic.Services.WebForm.Pages;
using Beeant.Domain.Entities.Management;

namespace Beeant.Presentation.Admin.Scm.Supplier.Supplier
{
    public partial class Detail : DetailPageBase<SupplierEntity>
    {
        protected override SupplierEntity GetEntity()
        {
            var info = base.GetEntity();
            if (info != null && info.Account != null)
            {
                info.Account = Ioc.Resolve<IApplicationService, AccountEntity>().GetEntity<AccountEntity>(info.Account.Id);
            }
            if (info != null && info.Service != null)
            {
                info.Service = Ioc.Resolve<IApplicationService, UserEntity>().GetEntity<UserEntity>(info.Service.Id);
            }
            return info;
        }
    }
}