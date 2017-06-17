using Component.Extension;
using Beeant.Domain.Entities.Authority;
using Beeant.Domain.Entities.Purchase;
using Beeant.Basic.Services.WebForm.Extension;
using Beeant.Basic.Services.WebForm.Pages;
using Beeant.Domain.Entities.Management;

namespace Beeant.Presentation.Admin.Scm.Purchase.PurchaseItem
{
    public partial class Add : AddPageBase<PurchaseItemEntity>
    {

        public override bool IsUpdatePanel
        {
            get { return false; }
            set
            {
                base.IsUpdatePanel = value;
            }
        }
        protected override PurchaseItemEntity FillEntity()
        {
            var info= base.FillEntity();
            if (info != null)
            {
                info.Purchase = new PurchaseEntity { Id = Request.QueryString["PurchaseId"].Convert<long>() };
            }
            return info;
        }

        protected override void LoadEntity()
        {
            var purchase = this.GetEntity<PurchaseEntity>(Request["PurchaseId"].Convert<long>());
            if (purchase.Status != PurchaseStatusType.WaitHandle)
                ((AuthorizePageBase) Page).InvalidateData("您没有权限在该状态下添加采购单明细");
            base.LoadEntity();
        }

    }
}