using Dependent;
using Beeant.Application.Services;
using Beeant.Domain.Entities.Purchase;
using Beeant.Basic.Services.WebForm.Pages;

namespace Beeant.Presentation.Admin.Scm.Purchase.PurchaseItem
{
    public partial class Update : UpdatePageBase<PurchaseItemEntity>
    {
        public override bool IsUpdatePanel
        {
            get { return false; }
            set
            {
                base.IsUpdatePanel = value;
            }
        }
        public override bool IsFillAllEntity
        {
            get
            {
                return false;
            }
            set
            {
                base.IsFillAllEntity = value;
            }
        }

        protected override PurchaseItemEntity GetEntity()
        {
            var info = base.GetEntity();
            if (info != null)
            {
                info.Purchase = Ioc.Resolve<IApplicationService, PurchaseEntity>().GetEntity<PurchaseEntity>(info.Purchase.Id);
                if (info.Purchase == null || info.Purchase.Status != PurchaseStatusType.WaitHandle)
                    ((AuthorizePageBase)Page).InvalidateData("您没有权限在该状态下修改采购单明细");
            }
         
            return info;
        }
   
    }
}