using Component.Extension;
using Beeant.Domain.Entities.Order;
using Beeant.Basic.Services.WebForm.Extension;
using Beeant.Basic.Services.WebForm.Pages;

namespace Beeant.Presentation.Admin.Erp.Order.OrderProduct
{
    public partial class Add : AddPageBase<OrderProductEntity>
    {

        public override bool IsUpdatePanel
        {
            get { return false; }
            set
            {
                base.IsUpdatePanel = value;
            }
        }
        protected override OrderProductEntity FillEntity()
        {
            var info= base.FillEntity();
            if (info != null)
            {
                info.Order = new OrderEntity { Id = Request.QueryString["OrderId"].Convert<long>() };
            }
            return info;
        }

        protected override void LoadEntity()
        {
            var order = this.GetEntity<OrderEntity>(Request.QueryString["OrderId"].Convert<long>());
            if (order.Status != OrderStatusType.WaitHandle)
                ((AuthorizePageBase) Page).InvalidateData("不能新增订单明细");
            base.LoadEntity();
        }

        protected override void SetResult(bool rev, System.Collections.Generic.IList<Winner.Filter.ErrorInfo> errors)
        {
            base.SetResult(rev,errors);
            if (errors == null || errors.Count == 0)
            {
                hfGoods.Visible = true;
                hfGoods.HRef = string.Format("{0}{1}", hfGoods.HRef, Request.QueryString["OrderId"]);
            }
              
        }
    }
}