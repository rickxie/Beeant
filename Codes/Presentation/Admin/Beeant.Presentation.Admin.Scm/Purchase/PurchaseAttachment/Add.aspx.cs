using Component.Extension;
using Beeant.Domain.Entities.Authority;
using Beeant.Domain.Entities.Purchase;
using Beeant.Basic.Services.WebForm.Extension;
using Beeant.Basic.Services.WebForm.Pages;
using Beeant.Domain.Entities.Management;
using Winner.Persistence;

namespace Beeant.Presentation.Admin.Scm.Purchase.PurchaseAttachment
{
    public partial class Add : AddPageBase<PurchaseAttachmentEntity>
    {
        /// <summary>
        /// 给ID赋值
        /// </summary>
        /// <returns></returns>
        protected override PurchaseAttachmentEntity FillEntity()
        {
            var info = base.FillEntity();
            if (info.SaveType == SaveType.Add)
                info.Purchase = new PurchaseEntity { Id = Request["PurchaseId"].Convert<long>() };
            return info;
        }
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

        protected override void LoadEntity()
        {
            var purchase = this.GetEntity<PurchaseEntity>(Request["PurchaseId"].Convert<long>());
            if (purchase.Status != PurchaseStatusType.WaitHandle)
                ((AuthorizePageBase) Page).InvalidateData("您没有权限在该状态下添加采购单附件");
            base.LoadEntity();
        }


    }
}