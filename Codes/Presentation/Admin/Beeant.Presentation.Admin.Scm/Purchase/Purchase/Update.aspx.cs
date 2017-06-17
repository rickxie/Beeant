using System;
using Dependent;
using Beeant.Application.Services;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Purchase;
using Beeant.Domain.Entities.Wms;
using Beeant.Basic.Services.WebForm.Pages;

namespace Beeant.Presentation.Admin.Scm.Purchase.Purchase
{
    public partial class Update : WorkflowPageBase<PurchaseEntity>
    {
       
        protected override void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ddlType.LoadData();
            }
            base.Page_Load(sender, e);
        }
        protected override PurchaseEntity GetEntity()
        {
            var info = base.GetEntity();
            if (info != null && info.Account != null)
            {
                info.Account = Ioc.Resolve<IApplicationService, AccountEntity>().GetEntity<AccountEntity>(info.Account.Id);
                info.Storehouse = Ioc.Resolve<IApplicationService, StorehouseEntity>().GetEntity<StorehouseEntity>(info.Storehouse.Id);
            }
            return info;
        }

      

    }
}