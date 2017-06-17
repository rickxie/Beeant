using System;
using Dependent;
using Beeant.Application.Services;
using Beeant.Basic.Services.WebForm.Pages;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Order;
using Winner.Persistence;

namespace Beeant.Presentation.Admin.Erp.Order.Order
{
    public partial class Update : WorkflowPageBase<OrderEntity>
    {
        public override bool IsFillAllEntity
        {
            get { return false; }
            set { base.IsFillAllEntity = value; }
        }
        public override SaveType SaveType => SaveType.Modify;

        protected override OrderEntity GetEntity()
        {
            var info = base.GetEntity();
            if (info != null && info.Account != null)
            {
                info.Account = Ioc.Resolve<IApplicationService, AccountEntity>().GetEntity<AccountEntity>(info.Account.Id);
            }
            return info;
        }
        protected override void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
               
                ddlType.LoadData();
            }
           
            base.Page_Load(sender, e);
        }

    }
}