using Dependent;
using Beeant.Application.Services;
using Beeant.Domain.Entities.Authority;
using Beeant.Domain.Entities.Purchase;
using Beeant.Basic.Services.WebForm.Extension;
using Beeant.Basic.Services.WebForm.Pages;
using Beeant.Domain.Entities.Management;

namespace Beeant.Presentation.Admin.Scm.Purchase.PurchaseAttachment
{
    public partial class Update : UpdatePageBase<PurchaseAttachmentEntity>
    {
        public override bool IsUpdatePanel
        {
            get
            {
                return false;
            }
            set
            {
                base.IsUpdatePanel = value;
            }
        }

        public override bool IsFillAllEntity
        {
            get { return false; }
            set
            {
                base.IsFillAllEntity = value;
            }
        }


  

        protected override PurchaseAttachmentEntity GetEntity()
        {
            var info = base.GetEntity();
            if (info != null)
            {
                info.Purchase = Ioc.Resolve<IApplicationService, PurchaseEntity>().GetEntity<PurchaseEntity>(info.Purchase.Id);
                if (info.Purchase == null || info.Purchase.Status != PurchaseStatusType.WaitHandle)
                    ((AuthorizePageBase)Page).InvalidateData("您没有权限在该状态下修改采购单附件");
            }
            return info;
        }
    }
}