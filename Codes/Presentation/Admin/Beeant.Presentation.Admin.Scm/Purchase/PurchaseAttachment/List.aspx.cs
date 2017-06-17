using System.Linq;
using Component.Extension;
using Beeant.Domain.Entities.Purchase;
using Beeant.Basic.Services.WebForm.Extension;
using Beeant.Basic.Services.WebForm.Pages;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Presentation.Admin.Scm.Purchase.PurchaseAttachment
{
    public partial class List : ListPageBase<PurchaseAttachmentEntity>
    {
        /// <summary>
        /// 根据订单Id来查询游客记录
        /// </summary>
        /// <param name="query"></param>
        protected override void SetQueryWhere(QueryInfo query)
        {
            query.Query<PurchaseAttachmentEntity>().Where(it => it.Purchase.Id == Request.QueryString["PurchaseId"].Convert<long>());
            base.SetQueryWhere(query);

        }

        protected override void LoadData()
        {
            var purchase = this.GetEntity<PurchaseEntity>(Request["PurchaseId"].Convert<long>());
            if (purchase.Status != PurchaseStatusType.WaitHandle)
                ((AuthorizePageBase) Page).InvalidateData("您没有权限在该状态下操作采购单附件");
            base.LoadData();
        }
    }
}